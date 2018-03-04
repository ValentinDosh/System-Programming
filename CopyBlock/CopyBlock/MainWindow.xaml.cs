using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CopyBlock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyThreadRead _read;
        MyThreadWrite _write;
        AutoResetEvent _autoReset;
        public static double barLevel;

        public MainWindow()
        {
            InitializeComponent();
            _autoReset = new AutoResetEvent(false);
        }


        // get path from(button)
        private void Button_Click_FromFilePath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openF = new OpenFileDialog();
            if (openF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxFrom.Text = openF.FileName;
                _read = new MyThreadRead(_autoReset, openF.FileName);
            }
        }

        // get path to(button)
        private void Button_Click_ToFilePath(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveF = new SaveFileDialog();
            if (saveF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxTo.Text = saveF.FileName;
                _write = new MyThreadWrite(_autoReset, _read, saveF.FileName);
            }
        }


        private void Button_Click_Copy(object sender, RoutedEventArgs e)
        {
            if (TextBoxTo.Text == "" || TextBoxFrom.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Введите путь");
            }
            else
            {
                _write.Start();
               
                new Thread(ProgressBarState).Start();

            }

        }

        private void ProgressBarState()
        {
            while (!(_write.FileLength < _read.Position)){
                Dispatcher.Invoke(() => ProgressBarCopy.Value = barLevel);
            }
        }
    }
}
