using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLSort.OUTPUT;

namespace XMLSort.PROCESSING
{
    class ManCounter // form stats in order GU total, then M and MO, then per RA (Breakdown per GU) so the function must call consequently
    {
        private List<ReportGenerator.SummInfo> SummInfos;
        public ManCounter(List<ReportGenerator.SummInfo> summInfos)
        {
            SummInfos = new List<ReportGenerator.SummInfo>(summInfos);
        }
        public string ProcessManCount()
        {
            var stmp = new StringBuilder();
            //count for each group
            //Per GU
            stmp.AppendLine("Подсчет людей по ГУ:");
            stmp.AppendLine(CountMenInGroups(SummInfos.GroupBy(summInfo => summInfo.GUNum),"ГУ/УПФР №"));
            //per dis
            stmp.AppendLine("Подсчет по Москве и области:");
            stmp.AppendLine(CountMenInGroups(SummInfos.GroupBy(summInfo => summInfo.Dis), ""));
            //per RA
            stmp.AppendLine("Подсчет людей по Районам:");
            stmp.AppendLine(CountMenInGroups(SummInfos.GroupBy(summInfo => summInfo.RANum), "Район"));
            //per bank
            stmp.AppendLine("Подсчет людей по Банкам:");
            stmp.AppendLine(CountMenInGroups(SummInfos.GroupBy(summInfo => summInfo.BankCode), "Код Банка"));

            return stmp.ToString();
        }
       private string CountMenInGroups<TProperty>(IEnumerable<IGrouping<TProperty, ReportGenerator.SummInfo>> inputGroups, string prePend)
        {
            var stmp = new StringBuilder();
            int count;
           
            foreach (var group in inputGroups)
            {
                count = 0;
                foreach (var item in group)
                {
                    count += item.ManCount;
                }
                stmp.AppendLine(String.Format("{0} {1} : {2} чел.", prePend, group.Key, count));
            }
            
            return stmp.ToString();
        }
        
    }
}
