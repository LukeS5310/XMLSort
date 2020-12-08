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
            public string Dis;
            public string BankName;
            public string BankCode;
            public SummInfo(int GUNum, int RANum, decimal Summ,string Dis, string BankCode, string BankName)
            {
                this.GUNum = GUNum;
                this.RANum = RANum;
                this.Summ = Summ;
                this.Dis = Dis;
                this.BankName = BankName;
                this.BankCode = BankCode;
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
            string[] bankNameCode;
            
            string SummPath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СуммаПоЧастиМассива";
            string RaPath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СистемныйНомерМассива";
            foreach (FileInfo File in Files)
            {
                decimal Summ = decimal.Parse(XElement.Load(File.FullName).XPathSelectElement(SummPath)?.Value.Replace(".",",") ?? "0");
                FileTypes.FileTypeXML.GetXMLDisGU(File.FullName, ref GUNum, ref dis);
                //TODO - GET RA
                RANum = int.Parse(XElement.Load(File.FullName).XPathSelectElement(RaPath).Value.Split('-')[1]);
                bankNameCode = GetBankName(File.FullName);
                switch (dis)
                {
                    case "М":
                        dis = "Москва";
                        break;
                    case "МО":
                        dis = "Московская область";
                        break;
                    default:
                        dis = "Неизвестно";
                        break;
                }
                Summs.Add(new SummInfo(GUNum, RANum, Summ, dis, bankNameCode[0],bankNameCode[1]));
            }
            //stats built - time to analyze
           
        }
        private string[] GetBankName(string xmlPath, bool isNameAppended = true)
        {
            string bankCodePath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/НомерБанка";
            string bankNamePath = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/Банк/НаименованиеОрганизации";
            string bankName;
            string bankCode;
            bankCode = XElement.Load(xmlPath).XPathSelectElement(bankCodePath)?.Value ?? "Нет кода банка";
            bankName = XElement.Load(xmlPath).XPathSelectElement(bankNamePath)?.Value ?? "Нет названия банка";
            if (isNameAppended == true) return new string[] {bankCode, bankName };
            else return new string[] { bankCode, "" };
        }
       
    }
}
