using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NotificationService.Core.DTO;
using NotificationService.Infrastructure.Kafka.Handlers;
using System.Text.RegularExpressions;

namespace EmployeeService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicController : ControllerBase
    {
        private readonly string _connectionString;
        public DynamicController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Endpoint để thực thi câu lệnh SQL động
        [HttpPost("execute-query")]
        public IActionResult ExecuteQuery([FromBody] string sqlQuery)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var result = connection.Query(sqlQuery);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        [HttpPost("parse-search-sqlcmd")]
        public ActionResult<List<SearchStage>> ParseSearchSQLCMD(string input)
        {
            var stages = new List<SearchStage>();
            var stageParts = input.Split("||", StringSplitOptions.RemoveEmptyEntries);

            
            foreach (var rawStage in stageParts)
            {
                Console.WriteLine(rawStage.Trim() + "-----------------");
                var trimmed = rawStage.Trim();
                if (!trimmed.StartsWith("stage")) continue;

                // Extract stage number
                var stageNumberMatch = Regex.Match(trimmed, @"stage\s+(\d+)\s*:");
                if (!stageNumberMatch.Success)
                    throw new Exception($"Invalid stage header: {trimmed}");

                int stageNumber = int.Parse(stageNumberMatch.Groups[1].Value);

                // Extract 4 segments inside {...}
                List<string> segments = new();
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
                }

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
