using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLSort.FileTypes
{
    class FileTypeResolver
    {
        public FileTypes.IFileIN ResolveFileType(string PathToFile)
        {
            string FileExtension = Path.GetExtension(PathToFile);
            switch (FileExtension.ToUpperInvariant())
            {
                case ".XML":
                    return new FileTypeXML();
                case ".ZIP":
                    return new FileTypeZIP();
                default:
                    System.Windows.MessageBox.Show(FileExtension);
                    return new FileTypeUnknown();
                    
            }
           
        }
    }
}
