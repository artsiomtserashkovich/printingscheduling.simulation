using System.Collections.Generic;
using System.IO;
using TaaS.PrintingScheduling.Simulation.Core.PrintingResult;

namespace TaaS.PrintingScheduling.Simulation.WalltimeDashboard
{
    public class ReportGenerator<TTime> where TTime : struct
    {
        private const string FileTemplatePath = "B:\\homeproject\\projects\\TaaS.PrintingScheduling.Simulation\\src\\TaaS.PrintingScheduling.Simulation.WalltimeDashboard\\index.html";
        private const string DATA_KEY = "@RESULT_DATA";
        
        public void Generate(IReadOnlyCollection<JobExecutionResult<TTime>> results, string folderPath, string filePrefix)
        {
            var template = File.ReadAllText(FileTemplatePath);

            var resultPage = template.Replace(DATA_KEY, System.Text.Json.JsonSerializer.Serialize(results));
            
            File.WriteAllText(folderPath + $"result_{filePrefix}.html", resultPage);
        }
    }
}