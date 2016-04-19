﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using _1C_client.Properties;

namespace _1C_client
{
    class Files_Worker
    {
        string _path = Client_1C_Settings.Default.pathToFiles; //Директория на сетевом диске 
        string _lastFile = Client_1C_Settings.Default.prevFileName;
        DateTime _lastFileCreatedDate = Client_1C_Settings.Default.prevFileCreatedDate;
        readonly string _fileMask = Client_1C_Settings.Default.fileMask;

        public string _GetPath { get { return _path; } }
        public string _GetLastFile { get { return _lastFile; } }
        public DateTime _GetLastFileCreateDate { get {return _lastFileCreatedDate;} }

        //Get all files from working directory
        public void Work()
        {
            try
            {
                List<string> files = GetFilesWithMask(_path, _fileMask);
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
                    throw new _1CException("Warning: Import file is missing");
                }
                else if ((Path.GetFileName(files[0]) == _lastFile) &&  ((new FileInfo(files[0]).CreationTime).ToLongDateString() == _lastFileCreatedDate.ToLongDateString()))
                {
                    throw new _1CException("Warning: Old file version");
                }
                else
                {
                    //Save properties settings
                    Client_1C_Settings.Default.prevFileName = Path.GetFileName(files[0]);
                    Client_1C_Settings.Default.prevFileCreatedDate = new FileInfo(files[0]).CreationTime;
                    Client_1C_Settings.Default.Save();

                    Debug.WriteLine("New settings added:");
                    Debug.WriteLine(Client_1C_Settings.Default.pathToFiles + "\n" + Client_1C_Settings.Default.prevFileName + "\n" + Client_1C_Settings.Default.prevFileCreatedDate);
                }
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
            }
        }

        static List<string> GetFilesWithMask(string path, string fileMask) // Известна маска файла
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.EnumerateFiles(path, fileMask, SearchOption.TopDirectoryOnly));
            }
            catch (Exception ex)
            {
                throw new _1CException(ex.ToString());
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
                throw new _1CException(ex.ToString());
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
                throw new _1CException(ex.ToString());
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
                throw new _1CException(ex.ToString());
            }
        }
    }
}
