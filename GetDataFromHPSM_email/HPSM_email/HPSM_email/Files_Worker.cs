using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prop = HPSM_email.Properties;
using System.IO;
using System.Diagnostics;

namespace HPSM_email
{
    class Files_Worker
    {
        string _path = prop.HPSMSettings.Default.pathToFiles; //Директория на сетевом диске (192.168.99.110)
        string _lastFile = prop.HPSMSettings.Default.prevFileName;
        DateTime _lastFileCreatedDate = prop.HPSMSettings.Default.prevFileCreatedDate;

        //Get all files from working directory
        public void Work(out bool flag, out string message)
        {
            try
            {
                List<string> files = GetFiles(_path, "*?.xls|*.xlsx");

                for (int k = 0; k < files.Count; k++)
                {
                    for (int j = 0; j < files.Count - k - 1; j++)
                    {
                        Debug.WriteLine(new FileInfo(files[j]) + "\t" + new FileInfo(files[j]).CreationTime);
                        Debug.WriteLine(new FileInfo(files[j + 1]) + "\t" + new FileInfo(files[j + 1]).CreationTime);
                        Debug.WriteLine("\n");

                        if (new FileInfo(files[j]).CreationTime < new FileInfo(files[j + 1]).CreationTime)
                        {
                            string tmp = files[j];
                            files[j] = files[j + 1];
                            files[j + 1] = tmp;
                        }
                    }
                }
                cleanOldFiles(ref files);
                if (files.Count == 0)
                {
                    flag = false;
                    message = "Warning: Import file is missing";
                    return;
                }
                else if ((Path.GetFileName(files[0]) == _lastFile) &&  (new FileInfo(files[0]).CreationTime == _lastFileCreatedDate))
                {
                    flag = false;
                    message = "Warning: Old file version";
                    return;
                }
                else
                {
                    prop.HPSMSettings.Default.prevFileName = Path.GetFileName(files[0]);
                    prop.HPSMSettings.Default.prevFileCreatedDate = new FileInfo(files[0]).CreationTime;
                    prop.HPSMSettings.Default.Upgrade();
                    prop.HPSMSettings.Default.Save();
                    flag = true;
                    message = "SUCCSSES IMPORT";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                message = ex.ToString();
                return;
            }
        }

        static List<string> GetFiles(string path, string searchPattern)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
            {
                List<string> tmp = new List<string>();
                tmp.AddRange(Directory.GetFiles(path, sp));

                for (int n = 0; n < tmp.Count; n++)
                {
                    if (new FileInfo(tmp[n]).Extension == sp.Substring(sp.IndexOf('.')))
                        files.Add(tmp[n]);
                }
            }
            return files;
        }

        static void cleanOldFiles(ref List<string> files)
        {
            int filesLimit = 31;
            if (files.Count > filesLimit)
            {
                List<string> removeFiles = files.GetRange(filesLimit, files.Count - filesLimit);
                files.RemoveRange(filesLimit, files.Count - filesLimit);
                deleteFiles(ref removeFiles);
            }
        }

        static void deleteFiles(ref List<string> removeFiles)
        {
            foreach (var file in removeFiles)
                File.Delete(file);
        }
    }
}
