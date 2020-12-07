using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XMLSort.LOGGING;
using System.Xml.Linq;
using System.Xml.XPath;
using XMLSort.PROCESSING;

namespace XMLSort.OUTPUT
{
    class ReportGenerator
    {
        //TODO: REWORK, cause rn it counts summs per bank actually
        public struct SummInfo
        {
            public decimal Summ;
            public int GUNum;
            public int RANum;
            public SummInfo(int GUNum, int RANum, decimal Summ)
            {
                this.GUNum = GUNum;
                this.RANum = RANum;
                this.Summ = Summ;
            }
        }
        
        private readonly string DefFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OUT");
        public void GenerateReport(string FolderPath = null)
        {
            List<SummInfo> Summs = new List<SummInfo>();
            Logger CurrentLogger = new Logger();
            
            if (FolderPath == null) FolderPath = DefFolder; // set to default
            DirectoryInfo[] InnerDirs = new DirectoryInfo(FolderPath).GetDirectories();
            
            foreach (DirectoryInfo dir in InnerDirs)
            {
                if (dir.Name == "Неопознанные файлы") continue;
             
               // int GUNum = GetGUNumFromFolder(dir.Name);
                ReadXMLFilesInFolder(dir.GetFiles("*-SPIS-*"), Summs);

            }
            var summProcessor = new SummProcessor(Summs);
            CurrentLogger.WriteLogMsg(summProcessor.ProcessSumms());
            CurrentLogger.SaveLogFile(forceSave: true);
        }
       
        private void ReadXMLFilesInFolder(FileInfo[] Files, List<SummInfo> Summs)
        {
           
            int GUNum = -1;
            int RANum = -1;
            string dis = "";
            
            string SummPath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СуммаПоЧастиМассива";
            string RaPath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СистемныйНомерМассива";
            foreach (FileInfo File in Files)
            {
                decimal Summ = decimal.Parse(XElement.Load(File.FullName).XPathSelectElement(SummPath)?.Value.Replace(".",",") ?? "0");
                FileTypes.FileTypeXML.GetXMLDisGU(File.FullName, ref GUNum, ref dis);
                //TODO - GET RA
                RANum = int.Parse(XElement.Load(File.FullName).XPathSelectElement(RaPath).Value.Split('-')[1]);
                Summs.Add(new SummInfo(GUNum, RANum, Summ));
            }
            //stats built - time to analyze
           
        }
       
    }
}
