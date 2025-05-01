using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Core.DTO
{
    public class TemplateDTO
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }

        public string TemplateBody { get; set; }
        public string TemplateHeader { get; set; }
        public List<SearchStage> SearchStages { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class SearchStage
    {
        public int StageNumber { get; set; }
        public string SqlCommand { get; set; }
        public List<(string Variable, int Stage)> DependenceVariables { get; set; }
        public List<string> OutputVariables { get; set; }
        public List<int> DependencyStages { get; set; }
    }
}
