using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmployeeService.API.Kafka.Producer;
using NotificationService.Core.DTO;
using NotificationService.Core.Services;
using NotificationService.Core.Services.SeparateService;
using NotificationService.Infrastructure.Kafka.KafkaEntity;

namespace NotificationService.Infrastructure.Kafka.Handlers
{
    public class TemplateHandler : IKafkaHandler<KafkaRequest<TemplateRequest>>
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEventProducer _eventProducer;
        
        public TemplateHandler(IEmailTemplateService emailTemplateService, IEventProducer eventProducer)
        {
            _emailTemplateService = emailTemplateService;
            _eventProducer = eventProducer;
        }
        public async Task HandleAsync(KafkaRequest<TemplateRequest> message)
        {
            EmailTemplateDTO template = await _emailTemplateService.GetByIdAsync(Guid.Parse(message.Filter.TemplateId));
            Console.WriteLine("Template DTO : {0}", template);
            List<SearchStage> searchStages = ParseSearchSQLCMD(template.SearchSQLCMD);
            KafkaResponse<TemplateDTO> kafkaResponse = new KafkaResponse<TemplateDTO>
            {
                RequestType = "Template",
                CorrelationId = message.CorrelationId,
                Timestamp = DateTime.UtcNow,
                Filter = new TemplateDTO { TemplateId = template.TemplateId, TemplateName = template.TemplateName, TemplateBody = template.TemplateBody, TemplateHeader = template.TemplateHeader, SearchStages = searchStages, DepartmentId = template.DepartmentId }
            };
            await _eventProducer.PublishAsync("TemplateInfo", null, "TemplateInfo", kafkaResponse);

        }

        public static List<SearchStage> ParseSearchSQLCMD(string input)
        {
            var stages = new List<SearchStage>();
            var stageParts = input.Split("||", StringSplitOptions.RemoveEmptyEntries);


            foreach (var rawStage in stageParts)
            {
                Console.WriteLine(rawStage.Trim().ToLower() + "-----------------");
                var trimmed = rawStage.Trim();
                if (!trimmed.StartsWith("stage")) continue;

                // Extract stage number
                var stageNumberMatch = Regex.Match(trimmed, @"stage\s+(\d+)\s*:");
                if (!stageNumberMatch.Success)
                    throw new Exception($"Invalid stage header: {trimmed}");

                int stageNumber = int.Parse(stageNumberMatch.Groups[1].Value);

                // Extract 4 segments inside {...}
                /*List<string> segments = new();
                int index = 0;
                while (segments.Count < 4 && index < trimmed.Length)
                {
                    int start = trimmed.IndexOf('{', index);
                    if (start == -1) break;

                    int end = trimmed.IndexOf('}', start + 1);
                    if (end == -1) break;

                    string content = trimmed.Substring(start + 1, end - start - 1);
                    segments.Add(content.Trim());
                    index = end + 1;
                }*/
                var segmentMatches = Regex.Matches(trimmed, @"\{(.*?)\}", RegexOptions.Singleline);
                var segments = segmentMatches.Cast<Match>()
                                             .Select(m => m.Groups[1].Value.Trim())
                                             .ToList();

                if (segments.Count != 4)
                    throw new Exception($"Invalid stage format at stage {stageNumber}");

                string sql = segments[0];

                // Parse DependenceVariables
                var depVars = new List<(string Variable, int Stage)>();
                var rawDeps = segments[1].Split("],", StringSplitOptions.RemoveEmptyEntries);
                foreach (var dep in rawDeps)
                {
                    var clean = dep.Replace("[", "").Replace("]", "").Trim();
                    if (string.IsNullOrWhiteSpace(clean)) continue;
                    var parts = clean.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2) continue;
                    depVars.Add((parts[0].Trim(), int.Parse(parts[1].Trim())));
                }

                // Parse OutputVariables
                var outVars = segments[2].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(v => v.Trim())
                                        .ToList();

                // Parse DependencyStages
                var depStages = segments[3].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => int.Parse(s.Trim()))
                                        .ToList();

                Console.WriteLine("Stage added : {0} - {1} - {2} - {3} - {4} ", stageNumber, sql, depVars, outVars, depStages);
                stages.Add(new SearchStage
                {
                    StageNumber = stageNumber,
                    SqlCommand = sql,
                    DependenceVariables = depVars,
                    OutputVariables = outVars,
                    DependencyStages = depStages
                });
            }

            return stages;
        }



    }
}
