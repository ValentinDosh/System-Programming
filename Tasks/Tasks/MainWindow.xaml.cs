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

namespace Tasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _tokenSourse;
        private CancellationToken _token;
        private List<string> _listPath;
        private List<Task> _listTask;
        private string _folderPath;
        private string _toPath;

        public MainWindow()
        {
            InitializeComponent();
            _tokenSourse = new CancellationTokenSource();
            _token = _tokenSourse.Token;
            _listPath = new List<string>();
            _folderPath = _toPath = String.Empty;
        }

        private void Button_Click_files_from(object sender, RoutedEventArgs e)
        {
            _listPath.Clear();
            _folderPath = String.Empty;
            var openFile = new OpenFileDialog();
            openFile.Multiselect = true;

            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxFrom.Text = openFile.FileName;
                _listPath.AddRange(openFile.FileNames);
            }
        }

        private void Button_Click_To_Path(object sender, RoutedEventArgs e)
        {
            var getFolderPath = new FolderBrowserDialog();

            if(getFolderPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxTo.Text = getFolderPath.SelectedPath;
            }
        }

        private void Button_Click_Copy(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxTo.Text) && !String.IsNullOrEmpty(textBoxFrom.Text))
            {
                // если поле имеет содержить путь-строку
                if (!String.IsNullOrEmpty(textBoxTo.Text))
                {
                    _toPath = textBoxTo.Text + _folderPath;
                    Directory.CreateDirectory(_toPath);
                }
                else
                {
                    _toPath = textBoxTo.Text;
                }

                foreach (var path in _listPath)
                {
                    Task.Run(() =>
                    {
                        byte[] strBuffer = new byte[4096];
                        var streamRead = new FileStream(path, FileMode.Open);
                        var streamWrite = new FileStream(_toPath + "\\" + System.IO.Path.GetFileName(path), FileMode.Create);

                        while (true)
                        {
                            if (_token.IsCancellationRequested)
                                break;
                            if (streamWrite.Length == streamRead.Length)
                                break;
                            var readLength = streamRead.Read(strBuffer, 0, strBuffer.Length);
                            streamWrite.Write(strBuffer, 0, readLength);
                        }
                        streamRead.Close();
                        streamWrite.Close();
                    });
                }
            }
        }

        private void Button_Click_Folder_From(object sender, RoutedEventArgs e)
        {
            var open_window = new FolderBrowserDialog();
            if (open_window.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxFrom.Text = open_window.SelectedPath;
                _folderPath = System.IO.Path.GetFileName(open_window.SelectedPath);
                _listPath.AddRange(Directory.GetFiles(open_window.SelectedPath));
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            _tokenSourse.Cancel();
        }
    }
}
