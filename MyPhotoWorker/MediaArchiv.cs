using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace MyPhotoWorker
{
    class MediaArchiv
    {
        private string _Path = null;
        private bool _IsAvailable = false;


        public string Path { get { return _Path; } set { _Path = value; } }
        public bool IsAvailable { get { return _IsAvailable; } set { _IsAvailable = value; } }

        public MediaArchiv(string ipath)
        {
            if (CheckMediaPath(ipath))
            {
                Path = ipath;
                IsAvailable = true;
            }
        }

        private bool CheckMediaPath(string ipath)
        {
            if (Directory.Exists(ipath))
            {
                return true;
            }
            return false;
        }
        
        public string SearchLastFileForCamera(string c_id)
        {
            string ret = "";
            if (!IsAvailable) return ret;
            try
            {
                string[] files = Directory.GetFiles(Path, "*" + c_id + "*.jpg", SearchOption.AllDirectories);
                if (files.Count() < 1) return ret;
                Array.Sort(files);
                ret = files.Last().ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ret;
        }
        public void MoveMediaFilesToArchiv(MediaFiles AllFiles)
        {
            if (!IsAvailable)
            {
                Console.WriteLine("Archivverzeichnis existiert nicht !");
                return;
            }
            string y;
            string m;
            int number;
            foreach (MediaFile mf in AllFiles)
            {
                if (mf.FileName.Length < 10) continue;
                y = mf.FileName.Substring(0, 4);
                try
                {
                    number = Int32.Parse(y);
                }
                catch (Exception)
                {
                    continue;
                }
                m = mf.FileName.Substring(5, 2);
                try
                {
                    number = Int32.Parse(m);
                }
                catch (Exception)
                {
                    continue;
                }
                if (number > 12) continue;
                MoveFromWorkToArchiv(y, m, mf.FullFileName);
            }

        }
        private void MoveFromWorkToArchiv(string year, string month, string fullfilename)
        {
            Console.WriteLine("Move File: " + fullfilename);
            string dir = Path + year + @"\";
            if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
            dir += month + @"\";
            if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
            string newfilepath = dir + System.IO.Path.GetFileName(fullfilename);
            //ui_txt_message.Text += year + "->" + month + "->" + fullfilename + "\n" + newfilepath + "\n";
            if (System.IO.File.Exists(newfilepath)) return;
            try
            {
                System.IO.File.Move(fullfilename, newfilepath);
            }
            catch (Exception)
            {
                //ui_txt_message.Text += "ERROR move file: " + fullfilename;
            }
        }
    }
}
