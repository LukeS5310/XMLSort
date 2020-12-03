using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLSort.INPUT
{
    class FileGrabber
    {
        public void GetFiles(string INDirPath = null)
        {
            if (INDirPath == null)
            {
                INDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IN"); //default
            }

            FileInfo[] INFiles = new DirectoryInfo(INDirPath).GetFiles();


        }

    }
}
