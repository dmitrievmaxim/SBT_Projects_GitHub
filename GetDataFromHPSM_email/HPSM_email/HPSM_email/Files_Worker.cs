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
        public void Work()
        {
            try
            {
                //List<string> files = GetFiles(_path, "*?.xls|*.xlsx");
                List<string> files = GetFiles(_path);
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
                    throw new HPSMException("Warning: Import file is missing");
                }
                else if ((Path.GetFileName(files[0]) == _lastFile) &&  (new FileInfo(files[0]).CreationTime == _lastFileCreatedDate))
                {
                    throw new HPSMException("Warning: Old file version");
                }
                else
                {
                    //Save properties settings
                    prop.HPSMSettings.Default.prevFileName = Path.GetFileName(files[0]);
                    prop.HPSMSettings.Default.prevFileCreatedDate = new FileInfo(files[0]).CreationTime;
                    prop.HPSMSettings.Default.Upgrade();
                    prop.HPSMSettings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }

        static List<string> GetFiles(string _path) // Неизвестно расширение файлов
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(_path));
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
            return files;
        }

        static List<string> GetFiles(string path, string searchPattern) //Известено расширение файлов
        {
            List<string> files = new List<string>();
            try
            {
                string[] searchPatterns = searchPattern.Split('|');
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
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
            return files;
        }

        static void cleanOldFiles(ref List<string> files)
        {
            int filesLimit = 31;
            try
            {
                if (files.Count > filesLimit)
                {
                    List<string> removeFiles = files.GetRange(filesLimit, files.Count - filesLimit);
                    files.RemoveRange(filesLimit, files.Count - filesLimit);
                    deleteFiles(ref removeFiles);
                }
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }

        static void deleteFiles(ref List<string> removeFiles)
        {
            try
            {
                foreach (var file in removeFiles)
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                throw new HPSMException(ex.ToString());
            }
        }
    }
}
