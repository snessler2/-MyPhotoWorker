using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Shapes;

namespace MyPhotoWorker
{
    public partial class OptionWindow : Window
    {
        Cameras cams = null;
        MediaArchiv ma = null;

        internal OptionWindow(Cameras ic, MediaArchiv ma_in)
        {
            InitializeComponent();
            cams = ic;
            this.DataContext = cams;
            ma = ma_in;
            //lbxCams.ItemsSource = cams;
            //foreach (Camera c in cams)
            //{
            //    lbxCams.Items.Add(c);
            //}
            //lbxCams.SelectedItem = lbxCams.Items.GetItemAt(0);
            cams.CollectionChanged += OnCollectionChanged;
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                Camera c1 = null;
                foreach (Camera ca in e.NewItems)
                {
                    c1 = ca;
                }
                lbxCams.SelectedItem = c1;
                //lbxCams.SelectedItem = lbxCams.Items.GetItemAt(lbxCams.Items.Count-1);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Camera c = lbxCams.SelectedItem as Camera;
            c.ShortName = tbxcamshort.Text;
            c.Name = tbxcammodel.Text;
            c.Description = tbxcamdescription.Text;
            if (cams.Save()) MessageBox.Show("Kameras in camera.txt gespeichert.");
        }
        private void lbxCams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //int s = lbxCams.SelectedIndex;
            //this.DataContext = null;
            //this.DataContext = cams;
            //lbxCams.SelectedIndex=s;
            SetValuesFromSelectedItem();
            lbxCams.Items.Refresh();

        }
        private void SetValuesFromSelectedItem()
        {
            Camera c = lbxCams.SelectedItem as Camera;
            if (c == null) return;
            //tbxcamshort.Text = c.ShortName;
            Binding myBinding1 = new Binding("ShortName");
            myBinding1.Source = c;
            myBinding1.Mode = BindingMode.TwoWay;
            myBinding1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(tbxcamshort, TextBox.TextProperty, myBinding1);
            //tbxcammodel.Text = c.Name;
            Binding myBinding2 = new Binding("Name");
            myBinding2.Source = c;
            myBinding2.Mode = BindingMode.TwoWay;
            myBinding2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(tbxcammodel, TextBox.TextProperty, myBinding2);

            //tbxcamdescription.Text = c.Description;
            Binding myBinding3 = new Binding("Description");
            myBinding3.Source = c;
            myBinding3.Mode = BindingMode.TwoWay;
            myBinding3.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(tbxcamdescription, TextBox.TextProperty, myBinding3);
            if (System.IO.File.Exists(App.AppExePath + c.ShortName + ".png"))
                ui_img_camera.Source = new BitmapImage(new Uri(App.AppExePath + c.ShortName + ".png", UriKind.Absolute));
            else
                ui_img_camera.Source = new BitmapImage(new Uri("/MyPhotoWorker;component/img/" + c.ShortName + ".png", UriKind.Relative));
        }
        private void ui_rmb_add_cam_Click(object sender, RoutedEventArgs e)
        {
            AddCamera();
            //Camera c = new Camera("", "_new", "", "");
            //cams.Add(c);
            //lbxCams.Items.Refresh();
            //lbxCams.Items.Add(c);
            //lbxCams.SelectedItem = lbxCams.Items.GetItemAt(lbxCams.Items.Count-1);
        }
        private void AddCamera()
        {
            cams.AddCamera();
        }
        private void RemoveCamera()
        {
            Console.WriteLine("Lösche");
            cams.Remove(lbxCams.SelectedItem as Camera);
            lbxCams.SelectedItem = lbxCams.Items.GetItemAt(0);
        }

        private void ui_rmb_del_cam_Click(object sender, RoutedEventArgs e)
        {
            RemoveCamera();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbxCams.SelectedItem = lbxCams.Items.GetItemAt(0);
        }

        private void Remove_camera_Click(object sender, RoutedEventArgs e)
        {
            RemoveCamera();
        }

        private void Add_camera_Click(object sender, RoutedEventArgs e)
        {
            AddCamera();
        }

        private void ui_btn_searchlastfile_Click(object sender, RoutedEventArgs e)
        {
            Camera c = lbxCams.SelectedItem as Camera;
            SearchLastPictureInArchiv(c.ShortName);
        }
        private void SearchLastPictureInArchiv(string camshortname)
        {
            MessageBox.Show("Letztes Bild von: " + camshortname + "\n"  + ma.SearchLastFileForCamera(camshortname));
        }
    }
}
