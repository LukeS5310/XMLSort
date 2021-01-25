using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLSort.LOGGING
{
    class Logger
    {
        private StringBuilder LogString = new StringBuilder();
        public int ErrsFound = 0;
        private string PrePend = "";
       
        public void WriteLogMsg(string message, bool IsError = false, bool UsePrePend = false)
        {
            if (IsError == true)
            {
                ErrsFound++;
            }
            if (UsePrePend == true)
            {
                message = string.Join("\r\n", PrePend, message);
            }
            message = string.Format("{0} {1}", TimeStamp(), message);
           
            LogString.AppendLine(message);
           
            PrePend = "";
        }
        public void AddPrePend(string _PrePend)
        {
            PrePend = _PrePend;
        }
        private string TimeStamp(bool GiveDate = false, bool IsForFile = false)
        {
            if (GiveDate == true) return string.Format("[{0}]", DateTime.Now.ToString("dd.MM.yyyy"));
            if (IsForFile == true) return string.Format("{0}", DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));


            return string.Format("[{0}]", DateTime.Now.ToString("HH:mm:ss"));
        }
        public string SaveLogFile(string Folder = null, bool forceSave = false)
        {
            if (Folder == null) Folder = System.AppDomain.CurrentDomain.BaseDirectory;
            string FileToSave = null;
            if (ErrsFound > 0 || forceSave == true)
            {
                try
                {
                    FileToSave = Path.Combine(Folder, string.Format("Отчёт {0}.txt", TimeStamp(IsForFile: true)));
                    File.WriteAllText(FileToSave, LogString.ToString());
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(String.Format("Ошибка сохранения {0}", ex.Message.ToString()));
                }

            }
            return FileToSave; 
        }

    }
}
