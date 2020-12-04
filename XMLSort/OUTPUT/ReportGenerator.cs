using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XMLSort.LOGGING;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XMLSort.OUTPUT
{
    class ReportGenerator
    {
        //TODO: REWORK, cause rn it counts summs per bank actually
        private struct SummInfo
        {
            public decimal Summ;
            public int GUNum;
            public string RANum;
            public SummInfo(int GUNum, string RANum, decimal Summ)
            {
                this.GUNum = GUNum;
                this.RANum = RANum;
                this.Summ = Summ;
            }
        }
        
        private readonly string DefFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OUT");
        public void GenerateReport(string FolderPath = null)
        {
            Logger CurrentLogger = new Logger();
            if (FolderPath == null) FolderPath = DefFolder; // set to default
            DirectoryInfo[] InnerDirs = new DirectoryInfo(FolderPath).GetDirectories();
            
            foreach (DirectoryInfo dir in InnerDirs)
            {
                if (dir.Name == "Неопознанные файлы") continue;
             
               // int GUNum = GetGUNumFromFolder(dir.Name);
                ReadXMLFilesInFolder(dir.GetFiles("*-SPIS-*"),CurrentLogger);

            }
            CurrentLogger.SaveLogFile(forceSave: true);
        }
       
        private void ReadXMLFilesInFolder(FileInfo[] Files, Logger Logger)
        {
            System.Windows.MessageBox.Show(Files.Length.ToString());
            int GUNum = -1;
            string RANum = "";
            List<SummInfo> Summs = new List<SummInfo>();
            string SummPath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СуммаПоЧастиМассива";
            foreach (FileInfo File in Files)
            {
                decimal Summ = decimal.Parse(XElement.Load(File.FullName).XPathSelectElement(SummPath)?.Value.Replace(".",",") ?? "0");
                FileTypes.FileTypeXML.GetXMLDisGU(File.FullName,ref GUNum,ref RANum);
                Summs.Add(new SummInfo(GUNum, RANum, Summ));
            }
            //stats built - time to analyze
            Logger.WriteLogMsg(string.Format("Сумма по ГУ/УПФР № {0} : {1} руб.", GUNum, CountGUSumm(Summs)));

        }
        private decimal CountGUSumm(List<SummInfo> SummInfos)
        {
            decimal result = 0;
            foreach (SummInfo SummInf in SummInfos)
            {
                result += SummInf.Summ;
            }

            return result;
        }
    }
}
