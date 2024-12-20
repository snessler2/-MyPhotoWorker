using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections;
using System.Collections.ObjectModel;


namespace MyPhotoWorker
{
    //class MediaFiles : List <MediaFile>
    class MediaFiles : ObservableCollection<MediaFile>
    {
        private int _counter = 0;

        public int counter { get { return _counter; } set { _counter = value; } }

        Action EmptyDelegate = delegate () { };
        //Action EmptyDelegate1 = delegate () { };

        public MediaFiles()
        {
            //Add(new MediaFile("meine"));
        }

        public MediaFile GetMediaFileById(int iid)
        {
            if (iid==0) return this.ElementAt(iid);
            return this.ElementAt(iid-1);
        }

        public void ReadWorkDir(string iWorkdir, Cameras cams, bool oldname)
        {
            DirectoryInfo _dir;
            if (!Directory.Exists(iWorkdir)) return; 
            List<string> filesAll = new List<string>();
            this.Clear();
            counter = 0;
            try
            { 
                _dir = new DirectoryInfo(iWorkdir);
                string ext = "";
                //Jede Datei im ausgewählten Verzeichnis
                foreach (FileInfo cf in _dir.GetFiles())
                {
                    ext = cf.Extension.ToLower();
                    if ((ext.Equals(".jpg")) || (ext.Equals(".mov")) || (ext.Equals(".mp4")) || (ext.Equals(".mpg")) || (ext.Equals(".avi")))
                    {
                        filesAll.Add(cf.FullName);
                    }
                    
                }
                _dir.Refresh();
            }
            catch
            {
                _dir = null;
            }
            //System.Windows.MessageBox.Show(filesAll.Count.ToString() + " Mediadateien gefunden! Bitte etwas Geduld nach dem Klick auf OK!");
            //((MainWindow)System.Windows.Application.Current.MainWindow).ui_taskitem.ProgressValue = 0.1;
            int max = filesAll.Count;
            WinProgress wp = new WinProgress();
            wp.Max = max;

            wp.Show();
            foreach (string f in filesAll)
            {
                counter++;
                wp.SetProgressValue(counter);
                wp.Dispatcher.Invoke(EmptyDelegate, System.Windows.Threading.DispatcherPriority.Background);
                //wp.pbar.Dispatcher.Invoke(EmptyDelegate, System.Windows.Threading.DispatcherPriority.Background);
                ((MainWindow)System.Windows.Application.Current.MainWindow).ui_taskitem.ProgressValue = counter / max;
                this.Add(new MediaFile(counter, f, cams, oldname));
            }
            ((MainWindow)System.Windows.Application.Current.MainWindow).ui_taskitem.ProgressValue = 0;
            //Doppelte Dateinamen mit Nummern versehen
            int i = 0;
            string myname = "nichtdrin";
            Hashtable hashtable = new Hashtable();
            foreach (MediaFile mf in this)
            {
                Console.WriteLine("------foreach-----mf.MediaDate=" + mf.MediaDate + "Id="+mf.Id);
                // Diese Schleifen testen alle Nummern auf vorhandensein !!
                do
                {
                    if (i == 0)
                    {
                        myname = mf.MediaDate;
                    }
                    else
                    {
                        Console.WriteLine("i-Wert="+ i);
                        myname = mf.MediaDate + "_" + SerienNumberZero(i);
                        mf.FileNo = SerienNumberZero(i);
                        if (i == 1)
                        {
                            if (!hashtable.ContainsKey(mf.MediaDate + "_00"))
                            {
                                Console.WriteLine("Setze vorher 00");
                                GetMediaFileById(mf.Id - 1).FileNo = "00";
                                hashtable.Add(mf.MediaDate + "_00", "");
                            }
                        }
                        Console.WriteLine("FileNo=" + mf.FileNo);
                    }
                    Console.WriteLine("myname=" + myname);
                    i++;
                }
                while (hashtable.ContainsKey(myname));
                Console.WriteLine("hashtable.Add=" + myname);
                hashtable.Add(myname, "");
                i = 0;
            }
            wp.Close();
        }
        public string SerienNumberZero(int i)
        {
            string ret = i.ToString();
            if (ret.Length == 1) ret = "0" + ret;
            //Console.WriteLine(i+" neu "+ret);
            return ret;
        }
        public void ReadWorkDir_old(string iWorkdir, Cameras cams)
        {
            if (Directory.Exists(iWorkdir))
            {
                this.Clear();
                counter = 0;
                
                //MessageBox.Show(path + " ist vorhanden");
                try
                {
                    List<string> filesAll = new List<string>();
                    foreach (string s in Directory.EnumerateFiles(iWorkdir, "*.jpg"))
                    {
                        filesAll.Add(s);
                    }
                    foreach (string s in Directory.EnumerateFiles(iWorkdir, "*.mov"))
                    {
                        filesAll.Add(s);
                    }
                    foreach (string s in Directory.EnumerateFiles(iWorkdir, "*.mp4"))
                    {
                        filesAll.Add(s);
                    }
                    foreach (string s in Directory.EnumerateFiles(iWorkdir, "*.mpg"))
                    {
                        filesAll.Add(s);
                    }
                    foreach (string s in Directory.EnumerateFiles(iWorkdir, "*.avi"))
                    {
                        filesAll.Add(s);
                    }
                   
                    string[] files = filesAll.ToArray();
                    if (files.Count() < 1) return;
                    Array.Sort(files);
                    foreach (string f in files)
                    {
                        counter++;
                        this.Add(new MediaFile(counter, f, cams, false));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                int i = 0;
                string myname = "nichtdrin";
                Hashtable hashtable = new Hashtable();
                foreach (MediaFile mf in this)
                {
                        do
                        {
                            if (i == 0)
                            {
                                myname = mf.MediaDate;
                            }
                            else
                            {
                                myname = mf.MediaDate + "_" + i.ToString();
                                mf.FileNo = i.ToString();
                            }
                            i++;
                            //Console.WriteLine(myname);
                        }
                        while (hashtable.ContainsKey(myname));
                        hashtable.Add(myname, "");
                    i = 0;
                }
            }
        }
    }
}
