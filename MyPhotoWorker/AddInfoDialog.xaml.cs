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
    /// Interaktionslogik für AddInfoDialog.xaml
    /// </summary>
    public partial class AddInfoDialog : Window
    {
        

        public AddInfoDialog(List<string> AddInfoList)
        {
            InitializeComponent();
            //lblQuestion.Content = "QQQQ";
            txtInfoAdd.Text = "";
            lboxInfoAdd.ItemsSource = AddInfoList;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            //AddInfoList.Add(Answer);
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //Später Optionsliste füllen
            txtInfoAdd.SelectAll();
            txtInfoAdd.Focus();
        }

        public string Answer
        {
            get { return txtInfoAdd.Text; }
        }

        private void lboxInfoAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtInfoAdd.Text = lboxInfoAdd.SelectedItem.ToString();
        }
    }
}
