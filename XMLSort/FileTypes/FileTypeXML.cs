using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
        string UniversalPathToType = "ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/ТипМассиваПоручений";
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
        public void ProcessFile(string PathToFile)
        {
            var UnknownXMLFile = new FileTypeUnknown();
            switch (GetXMLType(PathToFile))
            {
                case XMLType.TYPE_OSNOVN:
                    break;
                case XMLType.TYPE_RAZ_UDER:
                    break;
                case XMLType.TYPE_UNKNOWN:
                    UnknownXMLFile.ProcessFile(PathToFile);
                    return;
                default:
                    UnknownXMLFile.ProcessFile(PathToFile);
                    return;
            }

        }
    }
}
