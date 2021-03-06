﻿using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;

namespace XMLSort.FileTypes
{
    class FileTypeXML : IFileIN
    {
        enum XMLType
        {
            TYPE_OSNOVN,
            TYPE_RAZ_UDER,
            TYPE_UNKNOWN
              
        }
        const string UniversalPathToType = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/ТипМассиваПоручений";
        const string UniversalPathToBank = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/НомерБанка";
        const string UniversalPathToDate = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/Месяц";
        
        private XMLType GetXMLType(string XMLFilePath)
        {
            try
            {
                switch (XElement.Load(XMLFilePath).XPathSelectElement(UniversalPathToType).Value.ToString().ToUpper())
                {
                    case "РАЗОВЫЕ ВЫПЛАТЫ":
                        return XMLType.TYPE_RAZ_UDER;
                    case "ОСНОВНОЙ":
                        return XMLType.TYPE_OSNOVN;

                    default:
                        return XMLType.TYPE_UNKNOWN;
                }
            }
            catch (Exception)
            {

                return XMLType.TYPE_UNKNOWN;
            }
           
            
        }

        private string GetXMLBank(string XMLFilePath) //GETS THE BANK CODE VIA UNIVERSAL PATH IF FAILS RETURNS FOUR ZEROES
        {
            return XElement.Load(XMLFilePath).XPathSelectElement(UniversalPathToBank)?.Value ?? "0000";
        }

       private int GetXMLPril(string XMLFilePath)
        {

          
            int PriNum = 32;
            if (Globs.CurrentXMLPref == Globs.PrefferedXMLType.TYPE_UDER) PriNum = 36;
            return PriNum;
        }
        private string GetXMLMonth(string XMLFilePath)
        {
            string CurrMonth = XElement.Load(XMLFilePath).XPathSelectElement(UniversalPathToDate)?.Value ?? "";
            switch (CurrMonth)
            {
                case "1":
                    return CurrMonth;
                case "2":
                    return CurrMonth;
                case "3":
                    return CurrMonth;
                case "4":
                    return CurrMonth;
                case "5":
                    return CurrMonth;
                case "6":
                    return CurrMonth;
                case "7":
                    return CurrMonth;
                case "8":
                    return CurrMonth;
                case "9":
                    return CurrMonth;
                case "10":
                    return "a";
                case "11":
                    return "b";
                case "12":
                    return "c";


                default:
                    return "";
            }
        }
        public static void GetXMLDisGU(string XMLFilePath, ref int FGUNum, ref string FDis) //have to access from elsewhere
        {
            string UniversalPathToRA = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СистемныйНомерМассива";
            string stmp = XElement.Load(XMLFilePath).XPathSelectElement(UniversalPathToRA)?.Value ?? "0";
            string[] data = stmp.Split('-');
            if (data.Length < 1) 
            {
                FGUNum = -1;
                FDis = "";
                return;
            }
            var result = Globs.AllRaions.First(item => item.RNum == int.Parse(data[1]));
            FGUNum = result.GUNum;
            FDis = result.Dis ?? "";

        }
        public void ProcessFile(string PathToFile)
        {
            string OutPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OUT");
            string FinalPath;
            string TargetDir;
            string FType = "";
            string FDis = "";
            string FMonth;
            int FPartNum = Globs.PartNum;
            string FBankCode;
            int FGUNum = -1;
            int FPrNum;
            
            var UnknownXMLFile = new FileTypeUnknown();
            switch (GetXMLType(PathToFile)) //GETS TYPE AND DO STUFF WITH A SPECIFIC TYPE ALSO DITCHES THE INVALID FILES
            {
                case XMLType.TYPE_OSNOVN:
                    break;
                case XMLType.TYPE_RAZ_UDER:
                    if (Globs.CurrentXMLPref == Globs.PrefferedXMLType.TYPE_UNDEFINED)
                    {
                        
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            new Window1().ShowDialog();
                           
                        });
                       
                    }
                    if (Globs.CurrentXMLPref == Globs.PrefferedXMLType.TYPE_RAZOV) FType = "РВ";
                    
                    break;
                case XMLType.TYPE_UNKNOWN:
                    UnknownXMLFile.ProcessFile(PathToFile);
                    return;
                default:
                    UnknownXMLFile.ProcessFile(PathToFile);
                    return;
            }
            //Assign BankCode
            FBankCode = GetXMLBank(PathToFile);
            //ASSIGN GUNum and dis?

            GetXMLDisGU(PathToFile, ref FGUNum, ref FDis);
            //assign PriNum
            FPrNum = GetXMLPril(PathToFile);
            //assign Month
            FMonth = GetXMLMonth(PathToFile);

            TargetDir = string.Join("", FBankCode, FGUNum.ToString("D2"), FMonth, FPartNum, FPrNum, FType, FDis);
            FinalPath = Path.Combine(OutPath, TargetDir,Path.GetFileName(PathToFile));
            if (Directory.Exists(Path.Combine(OutPath, TargetDir)) == false) Directory.CreateDirectory(Path.Combine(OutPath, TargetDir));
           
            File.Move(PathToFile, FinalPath);

        }
    }
}
