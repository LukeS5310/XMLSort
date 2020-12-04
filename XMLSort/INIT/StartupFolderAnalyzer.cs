using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLSort.INIT
{
    class StartupFolderAnalyzer
    {
        public string ReadableResult;
        public int ReadyState = 0; // 0=unresolved 1=ready -1=err
        public void Analyze(string StartupPath = "")
        {
            string INDir = Path.Combine(StartupPath, "IN");
            string OUTDir = Path.Combine(StartupPath, "OUT");
            //generate directories if not exists
            if (Directory.Exists(INDir) == false)
            {
                try
                {
                    Directory.CreateDirectory(INDir);
                }
                catch (Exception)
                {

                    ReadyState = -1;
                    ReadableResult = "Папка IN не может быть создана!";
                    return;
                }
            }
            else //working with IN xddd
            {
                if (new DirectoryInfo(INDir).GetFiles().Length == 0)
                {
                    ReadyState = -1;
                    ReadableResult = "Нет файлов в папке IN!";
                    return;
                }
            }
            if (Directory.Exists(OUTDir) == false)
            {
                try
                {
                    Directory.CreateDirectory(OUTDir);
                }
                catch (Exception)
                {
                    ReadyState = -1;
                    ReadableResult = "Папка OUT не может быть создана!";
                    return;
                }
            }
            else //work with OUT x----D
            {
                if (new DirectoryInfo(OUTDir).GetFiles().Length > 0 || new DirectoryInfo(OUTDir).GetDirectories().Length > 0) //OLD FILES DETECTED! MOVING TO ENGAGE
                {
                    try
                    {
                        string NewName = string.Format("OUT-{0}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                        Directory.Move(OUTDir, Path.Combine(StartupPath, NewName));
                        Directory.CreateDirectory(OUTDir);
                        System.Windows.MessageBox.Show(string.Format("Обнаружены файлы в папке OUT - Она была переименована в {0}",NewName));
                    }
                    catch (Exception)
                    {
                        ReadyState = -1;
                        ReadableResult = "К папке OUT отсутствует доступ!";
                        return;

                    }
                }
            }

            ReadyState = 1;
            ReadableResult = string.Format("Готово к работе");
        }
    }
}
