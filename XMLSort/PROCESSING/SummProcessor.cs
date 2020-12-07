using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLSort.OUTPUT;


namespace XMLSort.PROCESSING
{
    class SummProcessor
    {
        private struct RAValidation
        {
            public int RNum;
            public bool IsPresent;
            public RAValidation(int rnum, bool isPresent = false)
            {
                this.RNum = rnum;
                this.IsPresent = isPresent;
            }
        }
        private List<ReportGenerator.SummInfo> SummInfos;
        public SummProcessor(List<ReportGenerator.SummInfo> summInfos)
        {
            SummInfos = new List<ReportGenerator.SummInfo>(summInfos);
        }
        
        public string ProcessSumms()
        {
            decimal totalSumm;
            var output = new StringBuilder();
            output.AppendLine("Данные по суммам В ГУ/УПФР");
            var resultsGroupedByGU = SummInfos.GroupBy(summInfo => summInfo.GUNum);
            foreach (var group in resultsGroupedByGU)
            {
                var validationList = GetValidationList(group.Key);
               
                totalSumm = 0;
                foreach (var summInfo in group)
                {
                    //flag present Districts and sum up Summs
                    var targetRA = validationList.Find(element => element.RNum == summInfo.RANum);
                    targetRA.IsPresent = true;
                    totalSumm += summInfo.Summ;
                }
                output.AppendLine(String.Format("Сумма по ГУ/УПФР №{0} : {1} руб.", group.Key, string.Format("{0:#,##0.00}",totalSumm)));
                output.AppendLine(GetMissingRA(validationList));
            }

            return output.ToString();
        }
        private List<RAValidation> GetValidationList(int guNum)
        {
            var allRAInGU = Globs.AllRaions.FindAll(element => element.GUNum == guNum);
            var output = new List<RAValidation>();
            foreach (var ra in allRAInGU)
            {
                output.Add(new RAValidation(ra.RNum));
            }
            return output;
        }
        private string GetMissingRA(List<RAValidation> rAValidations)
        {
            var output = new StringBuilder();
            foreach (var raVal in rAValidations)
            {
                if (raVal.IsPresent == false)
                {
                    output.AppendLine(String.Format("Район {0} отсутствует!", raVal.RNum));
                }
            }
            return output.ToString();
        }

    }
}
