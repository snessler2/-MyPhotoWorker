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
using System.Windows.Shapes;

namespace MyPhotoWorker
{
    /// <summary>
    /// Interaktionslogik für WinProgress.xaml
    /// </summary>
    public partial class WinProgress : Window
    {
        private int _Max = 1;

        public int Max { get { return _Max; } set { _Max = value; pbmax.Content = value.ToString(); pbar.Maximum = value; } }

        //Action EmptyDelegate = delegate () { };

        public WinProgress()
        {
            InitializeComponent();
            pbar.Minimum = 0;
        }
        public void SetProgressValue(int val)
        {
            pbvalue.Content = val.ToString();
            pbar.Value = val;
            //pbar.Dispatcher.Invoke(EmptyDelegate, System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
