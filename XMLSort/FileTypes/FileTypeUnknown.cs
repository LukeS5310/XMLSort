using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLSort.FileTypes;
using System.IO;

namespace XMLSort.FileTypes
{
    class FileTypeUnknown : IFileIN
    {
        private string RootFolder = "Неопознанные файлы";
        public void ProcessFile(string PathToFile) //This is the class for all unrecognized files which moves them to specific directory
        {
            string OutPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OUT");
            string RootPath = Path.Combine(OutPath, RootFolder);
            if (Directory.Exists(RootPath) == false);
            {
                Directory.CreateDirectory(RootPath);
            }

            File.Move(PathToFile, Path.Combine(RootPath, Path.GetFileName(PathToFile))); //well not so brainfuck compared to things in xml class
        }
    }
}
