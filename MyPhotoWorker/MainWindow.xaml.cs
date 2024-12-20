using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using MyPhotoWorker.Properties;
using System.Diagnostics;
using System.Globalization;
//using System.Windows.Forms;
//"Die Bildablage muss in der ersten Verzeichnisebene die Jahreszahlen und in der zweiten Verzeichnisebene die Monatszahlen besitzen.
//Dateinamensvormat: JJJJ-MM-TT_hh-mm-ss
namespace MyPhotoWorker
{
    public partial class MainWindow : Window
    {
        //internal static double COUNTER = 0;
        //internal static double COUNTERMAX = 1;
        internal static Cameras AllCameras = new Cameras();
        //Camera CurrentCamera = null;
        MediaArchiv CurrentArchiv = null;
        //MediaFiles AllFiles = new MediaFiles();
        MediaFiles AllFiles = new MediaFiles();
        // löschen ! List<string> AddInfoList = new List<string> { "beruf" };
        String Version = "MyPhotoWorker " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
       

        //public Cameras AllCameras { get { return _AllCameras; } set { _AllCameras = value; } }

        public MainWindow()
        {
            InitializeComponent();
            Settings setting = new Settings();
            if (Directory.Exists(setting.ArchivDirectory))
            {
                CurrentArchiv = new MediaArchiv(setting.ArchivDirectory);
            }
            else
            {
                SelectArchivDir();
                
            }
            SetWindowTitle();
            ui_txt_workdir.Text = setting.WorkDirectory;
            if (File.Exists(App.AppExePath + "background.jpg"))
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(App.AppExePath + "background.jpg", UriKind.Absolute)));
            }
            this.DataContext = AllFiles;
            FillUiData();
            //ui_taskitem.ProgressValue = counter;
        }
        private void SetWindowTitle()
        {
            if (CurrentArchiv != null)
            {
                this.Title = Version + "  -  ARCHIV: " + CurrentArchiv.Path;
            }
            else
            {
                this.Title = Version + "  -  ARCHIV: unbekannt";
            }
        }
        
        private void FillUiData()
        {
            //Jahre
            int year = DateTime.Now.Year;
            do
            {
                cbx_jahr.Items.Add(year.ToString());
                year--;
            } while (year>1979);
            cbx_jahr.SelectedItem = DateTime.Now.Year.ToString();
            //Monate
            string[] month = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            foreach (string s in month) cbx_monat.Items.Add(s);
            cbx_monat.SelectedItem = "06";
            //Tag
            string[] day = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
            foreach (string s in day) cbx_tag.Items.Add(s);
            cbx_tag.SelectedItem = "15";
            //Stunde
            string[] hour = new string[] { "  ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"};
            foreach (string s in hour) cbx_stunde.Items.Add(s);
            cbx_stunde.SelectedItem = "  ";
            //Minute
            string[] minute = new string[] { "  ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };
            foreach (string s in minute) cbx_minute.Items.Add(s);
            for (int i = 10; i < 60; i++)
            {
                cbx_minute.Items.Add(i.ToString());
            }
            cbx_minute.SelectedItem = "  ";
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //DataGridCell cellbackground = new DataGridCell();
            //cellbackground.Background()
            if (e.PropertyName == "Id")
            {
                e.Column.Header = "Id";
                //e.Cancel = true; blendet die Spalte aus
            }
            else if (e.PropertyName == "FileName") { e.Column.Header = "Dateiname"; e.Column.CellStyle = new Style
            { Setters = {new Setter(ContentControl.BackgroundProperty, Brushes.LightGray)} };
            }
            else if (e.PropertyName == "FileExtension") { e.Column.Header = "Typ"; }
            else if (e.PropertyName == "FileNameNew") { e.Column.Header = "Neuer Name"; }
            else if (e.PropertyName == "IptcKeys") { e.Column.Header = "IPTC Keywords"; }
            else if (e.PropertyName == "MediaDate") { e.Cancel = true; }
            else if (e.PropertyName == "MediaDateType") { e.Cancel = true; }
            else if (e.PropertyName == "FileDate") { e.Cancel = true; }
            else if (e.PropertyName == "FileNo") { e.Cancel = true; }
            else if (e.PropertyName == "CamSName") { e.Cancel = true; }
            else if (e.PropertyName == "Info") { e.Cancel = true; }
            else if (e.PropertyName == "CamName") { e.Cancel = true; }
            else if (e.PropertyName == "FullFileName") { e.Cancel = true; }


        }

        public string SelectDir(string seltext, string defaultdir)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.SelectedPath = defaultdir;
            fbd.ShowNewFolderButton = true;
            fbd.Description = seltext;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                defaultdir = fbd.SelectedPath+@"\";
            return defaultdir;
        }
        private void read_workdir_Click(object sender, RoutedEventArgs e)
        {
            ReadWorkDir();
        }
        public void ReadWorkDir()
        {
            string path = ui_txt_workdir.Text;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Workdir not exist");
                return;
            }
            AllFiles.ReadWorkDir(path, AllCameras, (bool)ui_chk_oldname.IsChecked);
            //MessageBox.Show("Dateien verarbeitet!");
        }
        private void FileRename_Click(object sender, RoutedEventArgs e)
        {
            FileRenameInWorkDir();
        }
        public void FileRenameInWorkDir()
        {
            string path = ui_txt_workdir.Text;
            foreach (MediaFile mf in AllFiles)
            {
                try
                {
                    File.Move(mf.FullFileName, path + mf.FileNameNew);
                }
                catch (Exception e)
                {
                Console.WriteLine("ERROR: "+mf.FullFileName + "->" + path + mf.FileNameNew+"->"+e.ToString());

                }
            }
            ReadWorkDir();
        }
        private void ui_cbx_cameras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ChangeCameraPicture();
            //MessageBox.Show("Aktuell: "+CurrentCamera.Name);
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SelectArchivDir();
        }
        private void SelectArchivDir()
        {
            string newarchiv=@"c:\";

            if (CurrentArchiv==null)
            {
                newarchiv = SelectDir("Archivverzeichnis auswählen", newarchiv);
            }
            else
            {
                 if (CurrentArchiv.IsAvailable)
                {
                    newarchiv = SelectDir("Archivverzeichnis auswählen", CurrentArchiv.Path);
                }
                else
                {
                    newarchiv = SelectDir("Archivverzeichnis auswählen", newarchiv);
                }
            }
            Settings setting = new Settings();
            setting.ArchivDirectory = newarchiv;
            setting.Save();
            CurrentArchiv = new MediaArchiv(setting.ArchivDirectory);
            //CurrentArchiv.Path = newarchiv;
            SetWindowTitle();
        }
        private void ui_btn_workdir_Click(object sender, RoutedEventArgs e)
        {
            ui_txt_workdir.Text = SelectDir("Arbeitsverzeichnis auswählen", ui_txt_workdir.Text);
            Settings setting = new Settings();
            setting.WorkDirectory = ui_txt_workdir.Text;
            setting.Save();
        }
        private void ui_btn_movetoarchiv_Click(object sender, RoutedEventArgs e)
        {
            CurrentArchiv.MoveMediaFilesToArchiv(AllFiles);
            ReadWorkDir();
        }
        private void StartExplorerWithPath(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                MessageBox.Show("Directory: " + path + " not exists!");
                return;
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.Arguments = path;
            p.StartInfo.FileName = "explorer.exe";
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            p.Start();
        }
        private void ui_btn_openworkdir_Click(object sender, RoutedEventArgs e)
        {
            StartExplorerWithPath(ui_txt_workdir.Text);
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            OpenArchivDir();
        }
        private void OpenArchivDir()
        {
            if (CurrentArchiv.IsAvailable)
            {
                StartExplorerWithPath(CurrentArchiv.Path);
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Einstellungen");
            OptionWindow owin = new OptionWindow(AllCameras, CurrentArchiv);
            owin.Show();
        }

        private void ui_chk_showall_Checked(object sender, RoutedEventArgs e)
        {
            AddColumnToGrid(ui_data_allfiles, 1, "Erzeugungsdatum", "MediaDate");
            AddColumnToGrid(ui_data_allfiles, 1, "-", "MediaDateType");
            AddColumnToGrid(ui_data_allfiles, 1, "Dateidatum", "FileDate");
            AddColumnToGrid(ui_data_allfiles, 1, "NR", "FileNo");
            AddColumnToGrid(ui_data_allfiles, 1, "kurz", "CamSName");
            AddColumnToGrid(ui_data_allfiles, 1, "Info", "Info");
            AddColumnToGrid(ui_data_allfiles, 1, "Model", "CamName");
            //AddColumnToGrid(ui_data_allfiles, 1, "Ort", "FullFileName");
        }
        private void AddColumnToGrid(DataGrid dg, int coltype, string colname, string bname)
        {
            switch (coltype)
            {
                case 1:
                    DataGridTextColumn TextColumn = new DataGridTextColumn();
                    TextColumn.Header = colname;
                    TextColumn.Binding = new Binding(bname);
                    dg.Columns.Add(TextColumn);
                    break;
                case 2:
                    DataGridTextColumn Column = new DataGridTextColumn();
                    break;
            }
            
        }
        private void ui_chk_showall_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 10; i > 3; i--)
            {
                ui_data_allfiles.Columns.Remove(ui_data_allfiles.ColumnFromDisplayIndex(i));
            }
            
        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            string ts = "";
            try
            {

                ts = ((MediaFile)(e.Row.DataContext)).FileExtension;
                //Console.WriteLine("Zeilenfarbe3" + ((System.Data.DataRowView)(e.Row.DataContext)).Row.ItemArray[2].ToString());
                if (ts != "jpg")
                {
                    e.Row.Background = new SolidColorBrush(Colors.Gold);
                }
            }
            catch
            {
            }
        }

        private void ui_btn_iptcwrite_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Schreibe IPTC");
            string path = ui_txt_workdir.Text;
            foreach (MediaFile mf in AllFiles)
            {

                if (mf.IptcKeys.Length > 1)
                {
                    Console.WriteLine("IPTC " + mf.IptcKeys);

                    //File.Move(mf.FullFileName, path + mf.FileNameNew); 
                }
                
            }
            ReadWorkDir();
        }

        private void ManFileRename_Click(object sender, RoutedEventArgs e)
        {
            ManFileRename();
        }
        private void ManFileRename()
        {
            //Namensteile festlegen
            string pre = cbx_jahr.SelectedItem + "-" + cbx_monat.SelectedItem;
            string no = "";
            string post = "";
            int i = 0;
            if (cbx_tag.SelectedValue.ToString() != "  ") pre += "-" + cbx_tag.SelectedItem;
            if (cbx_stunde.SelectedValue.ToString() != "  ") {
                pre += "_" + cbx_stunde.SelectedItem;
                if (cbx_minute.SelectedValue.ToString() != "  ") pre += "-" + cbx_minute.SelectedItem;
                //pre += "-test";
            }
            if ((bool)ckb_revno.IsChecked) i = AllFiles.Count+1;
            foreach (MediaFile mf in AllFiles)
            {
                if ((bool)ckb_revno.IsChecked) i--;
                else i++;
                if ((bool)ckb_zaehler.IsChecked) no = "_" + AllFiles.SerienNumberZero(i);
                if ((bool)ckb_oldname.IsChecked) post = "_" + mf.FileName;
                mf.FileNameNew = pre + no + post + "." + mf.FileExtension;
            }
        }
    }
}
