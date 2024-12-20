using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace MyPhotoWorker
{
    //internal class Cameras : List<Camera>
    internal class Cameras : ObservableCollection<Camera>
    {
        public Cameras()
        {
            ReadCameras();
        }
        public void ReadCameras()
        {
            if (System.IO.File.Exists(App.AppExePath + "cameras.txt")) ReadCamerasFromFile(App.AppExePath + "cameras.txt");
            else
            {
            //Add(new Camera("Canon PowerShot S110", "_s110", "", ""));
            //Add(new Camera("iPhone 7", "_i6", "", ""));
            //Add(new Camera("iPhone 6", "_i6", "", ""));
            //Add(new Camera("iPod 5", "_tipod", "", ""));
            //Add(new Camera("iPhone 4", "_i4", "", ""));
            //Add(new Camera("Samsung WB550", "_wb550", "", ""));
            //Add(new Camera("Canon EOS300", "_300D", "", ""));
            //Add(new Camera("Minolta Dimage", "_DIMGX", "", ""));
            Add(new Camera("", "Unbekannte Kamera", "", ""));
            }
        }
        public void ReadCamerasFromFile(string cfile)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(cfile);
            while ((line = file.ReadLine()) != null)
            {
                string[] carr = null;
                carr = line.Split(';');
                if (carr.Length == 4)
                {
                    Add(new Camera(carr[1], carr[0], carr[3], carr[2]));
                }
                if (carr.Length == 3)
                {
                    Add(new Camera(carr[1], carr[0], "" , carr[2]));
                }
                if (carr.Length == 2)
                {
                    Add(new Camera(carr[1], carr[0], "", ""));
                }
            }
            file.Close();
        }

        public void AddCamera()
        {
            Add(new Camera("", "_new", "", ""));
        }
        public Camera GetCameraByShortName(string isname)
        {
            foreach (Camera c in this)
            {
                if (c.ShortName == isname) return c;
            }
            return this.Last<Camera>();
        }
        public String GetShortNameByCamName(string isname)
        {
            if (isname == null) isname = "";
            foreach (Camera c in this)
            {
                if (c.Name == isname) return c.ShortName;
            }
            try
            { 
                
                return "_"+isname.Trim();
            }
            catch (Exception e)
            {
                return "";
            }
            //return this.Last<Camera>().ShortName;
        }
        public bool Save()
        {
            string[] lines = new string[this.Count];
            int i = 0;
            foreach(Camera c in this)
            {
                lines[i] = c.GetCSVString();
                i++;
            }
            try
            {
                System.IO.File.WriteAllLines(App.AppExePath + "cameras.txt", lines);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }
    }
}
