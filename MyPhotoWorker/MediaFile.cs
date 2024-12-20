using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MyPhotoWorker
{
    class MediaFile : INotifyPropertyChanged
    {
        private int _Id = 0;
        private string _FileName = "";
        private string _FileExtension = "";
        private string _FileNameNew = "_";
        private string _IptcKeys = "_";
        private bool _IsSetOldName = false;
        private string _MediaDate = "";
        private string _MediaDateType = ""; // E...Exif T...Tool F...File
        private string _FileNo = "";
        private DateTime _FileDate;
        private string _CamName = "";
        private string _CamSName = "";
        private string _FullFileName = "";
        private string _Info = "";

        Regex r = new Regex("^[1-2][0-9]{3}");
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get { return _Id; } set { _Id = value; } }
        public string FileName { get { return _FileName; } set { _FileName = value; } }
        public string FileExtension { get { return _FileExtension; } set { _FileExtension = value; } }
        public string FileNameNew { get { return _FileNameNew; } set { _FileNameNew = value; Changed(); } }
        public string IptcKeys { get { return _IptcKeys; } set { _IptcKeys = value; } }
        public bool IsSetOldName { get { return _IsSetOldName; } set { _IsSetOldName = value; BuildNewName(); } }
        public string MediaDate { get { return _MediaDate; } set { _MediaDate = value; BuildNewName(); } }
        public string MediaDateType { get { return _MediaDateType; } set { _MediaDateType = value; } }
        public string FileNo { get { return _FileNo; } set { _FileNo = value; BuildNewName(); } }
        public DateTime FileDate { get { return _FileDate; } set { _FileDate = value; } }
        public string CamName { get { return _CamName; } set { _CamName = value; } }
        public string CamSName { get { return _CamSName; } set { _CamSName = value; BuildNewName(); } }
        public string FullFileName { get { return _FullFileName; } set { _FullFileName = value; } }
        public string Info { get { return _Info; } set { _Info = value; BuildNewName(); } }

        public MediaFile(int iid, string ifilename, Cameras cams, bool oldname)
        {
            Id = iid;
            IsSetOldName = oldname;
            FullFileName = ifilename;
            FileName = ifilename.Substring(ifilename.LastIndexOf('\\')+1);
            FileName = FileName.Split('.')[0];
            FileInfo fi = new FileInfo(FullFileName);
            FileDate = fi.LastWriteTime;
            FileExtension = System.IO.Path.GetExtension(FullFileName).ToLower().Substring(1);
            if (FileExtension == "jpg")
            {
                SetExifMetaData(cams);
                IptcKeys = ReadIptcCategories(ifilename);
            }
            else SetMovieMetaData(cams);

        }
        private string ReadIptcCategories(string filepath)
        {
            string ret = "";
            var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            if (metadata != null)
                if (metadata.Keywords != null)
                {
                    ret = metadata.Keywords.Aggregate((old, val) => old + "*" + val); 
                }
            stream.Close();
            return ret;
        }
        private void SetExifMetaData(Cameras cams)
        {
            string ret = "";
            MemoryStream data = new MemoryStream(File.ReadAllBytes(FullFileName));
            BitmapSource img = BitmapFrame.Create(data);
            BitmapMetadata meta = (BitmapMetadata)img.Metadata;
            if (meta.DateTaken == null)
            {
                SetFileChangeDate();
                return;
            }
            try
            {
                //if (CamSName == "") CamSName = meta.CameraModel;
                //ret = meta.CameraModel + meta.DateTaken;
                ret = meta.DateTaken.Substring(6, 4) + "-";
                ret += meta.DateTaken.Substring(3, 2) + "-";
                ret += meta.DateTaken.Substring(0, 2) + "_";
                ret += meta.DateTaken.Substring(11, 2) + "-";
                ret += meta.DateTaken.Substring(14, 2) + "-";
                ret += meta.DateTaken.Substring(17, 2);
                MediaDate = ret;
                MediaDateType = "E";
                CamName = meta.CameraModel;
                //if (cams.GetShortNameByCamName(meta.CameraModel) != "") ;
                CamSName = cams.GetShortNameByCamName(meta.CameraModel);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " +e.ToString());
                SetFileChangeDate();
                //MediaDate = MediaDate+"f";
                //MediaDate = MediaDate+"_fdat_"+FileName;
            }
        }

        private string SetExifToolCreationDate()
        {
            string extool = App.AppExePath + "exiftool.exe";
            if (!System.IO.File.Exists(extool))
            {
              
                return "";
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            // -CreationDate für Apple Filme
            // -CreateDate für ander Filme
            if (CamName.Contains("iPhone"))
                p.StartInfo.Arguments = "-S -t -d \"%Y-%m-%d_%H-%M-%S\" -CreationDate \"" + FullFileName+"\"";
            else
            {
                if (CamName.Contains("RX10M4"))
                    p.StartInfo.Arguments = "-S -t -d \"%Y-%m-%d_%H-%M-%S\" -FileModifyDate \"" + FullFileName + "\"";
                else
                {
                    p.StartInfo.Arguments = "-S -t -d \"%Y-%m-%d_%H-%M-%S\" -CreateDate \"" + FullFileName + "\"";
                }
            }
                
            //Console.WriteLine(p.StartInfo.Arguments);
            p.StartInfo.FileName = extool;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                //Console.WriteLine("ETline: "+ FullFileName);
                //Console.WriteLine("ETline: "+ line);
                //Console.WriteLine(r.IsMatch(line).ToString());
                if (r.IsMatch(line)) return line;
                //if (line.Length > 6) return line;
            }
            return "";
        }
        private string GetExifToolCameraModel(String ExifAttr)
        {
            string extool = App.AppExePath + "exiftool.exe";
            if (!System.IO.File.Exists(extool))
            {

                return "";
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = "-S -t -"+ ExifAttr + " \"" + FullFileName+"\"";
            Console.WriteLine(p.StartInfo.Arguments);
            p.StartInfo.FileName = extool;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            Console.WriteLine("Vor der Auswertung");
            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                Console.WriteLine(line);
                if (line.Length > 6) return line;
            }
            Console.WriteLine("Nach der Auswertung");
            return "";
        }
        private void SetMovieMetaData(Cameras cams)
        {
            CamName = GetExifToolCameraModel("Model").Trim();
            if (CamName.Length<3)
                CamName = GetExifToolCameraModel("DeviceModelName").Trim();
            CamSName = cams.GetShortNameByCamName(CamName);

            //string ret = "";
            //if ((CamSName == "_i6")|| (CamSName == "_i7"))
            //{
                MediaDate= SetExifToolCreationDate();
                //System.Windows.MessageBox.Show(": "+MediaDate.ToString());
                if (MediaDate != "")
                {
                    MediaDateType = "T";
                    return;
                }
            SetFileChangeDate();
            //}

            //string ret = System.IO.Path.GetFileNameWithoutExtension(FullFileName).ToLower();
            //FileInfo fi = new FileInfo(FullFileName);
            //string lwt = fi.CreationTime.ToString();
            //string lwt = fi.LastWriteTime.ToString();
            //ui_txt_message.Text += lwt+"\n";
            //ret += lwt.Substring(6, 4) + "-";
            //ret += lwt.Substring(3, 2) + "-";
            //ret += lwt.Substring(0, 2) + "_";
            //ret += lwt.Substring(11, 2) + "-";
            //ret += lwt.Substring(14, 2) + "-";
            //ret += lwt.Substring(17, 2);
            //MediaDate = ret;
            //MediaDate = FileDate.ToString("yyyy-MM-dd_HH-mm-ss");
            //MediaDateType = "F";
            //fi = null;
        }
        private void SetFileChangeDate()
        {
            //string ret = "";
            //FileInfo fi = new FileInfo(FullFileName);
            //string lwt = fi.LastWriteTime.ToString();
            //string lwt = FileDate.ToString();
            //ret += lwt.Substring(6, 4) + "-";
            //ret += lwt.Substring(3, 2) + "-";
            //ret += lwt.Substring(0, 2) + "_";
            //ret += lwt.Substring(11, 2) + "-";
            //ret += lwt.Substring(14, 2) + "-";
            //ret += lwt.Substring(17, 2);
            //MediaDate = ret;
            MediaDate = FileDate.ToString("yyyy-MM-dd_HH-mm-ss");
            MediaDateType = "F";
            //fi = null;
        }

        private void BuildNewName()
        {
            FileNameNew = MediaDate;
            if (FileNo != "") FileNameNew += "_" + FileNo;
            if (CamSName == "" && CamName!="") FileNameNew += "_" + CamName;
            else FileNameNew += CamSName;
            if (Info != "") FileNameNew += "_" + Info;
            if (IsSetOldName) FileNameNew += "_" + FileName;
            FileNameNew += "."+FileExtension;
        }
        private void Changed([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
