using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using System.IO;

namespace XMLSort.FileTypes
{
    class FileTypeZIP : IFileIN
    {
        string ExtractFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TEMP");

        public void ProcessFile(string PathToFile)
        {
            if (Directory.Exists(ExtractFolder)==true) //cleanup failsafe
            {
                Directory.Delete(ExtractFolder);
            }
            using (ZipFile zip = ZipFile.Read(PathToFile))
            {
                zip.ExtractSelectedEntries("name = *.xml", null,ExtractFolder);
               
            }
            var Grab = new INPUT.FileGrabber();
            Grab.GetFiles(ExtractFolder);
            //CLEANUP
            File.Delete(PathToFile);
            Directory.Delete(ExtractFolder);
        }

    }
}
