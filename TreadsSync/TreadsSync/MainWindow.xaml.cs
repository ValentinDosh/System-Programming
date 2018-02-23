using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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


namespace TreadsSync
{

    public partial class MainWindow : Window
    {
        object locker = new object();

        // Thread #1
        Thread _threadWriteInfo;
        string _pathInfoProcess;
        string _infoProcess;

        // Thread #2
        Thread _threadScreenShoots;
        // path direcctory for screenshots
        string _pathScreenShotsDirectory;
        // path screenshots
        string _pathScreenShots;
        //
        Graphics graph;
        Bitmap btm;
        // counter for name ScreenShoots
        static int counter = 1;

        // Thread #3
        Thread _countStringScreen;
        enum Stream
        {
            Read,
            Write
        }


        public MainWindow()
        {
            InitializeComponent();
            // 1
            _threadWriteInfo = null;
            _pathInfoProcess = "ProcessInfo.txt";
            // 2
            _threadScreenShoots = null;
            _pathScreenShotsDirectory = "ScreenShots";
            _pathScreenShots = Environment.CurrentDirectory + "\\" + _pathScreenShotsDirectory;
            graph = null;
            btm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            graph = Graphics.FromImage(btm);
            // 3
            _countStringScreen = null;
        }


        // handler for Thread #1
        private void Button_Click_Thread_1(object sender, RoutedEventArgs e)
        {
            if (_threadWriteInfo == null 
                || _threadWriteInfo.ThreadState == System.Threading.ThreadState.AbortRequested
                || _threadWriteInfo.ThreadState == System.Threading.ThreadState.Stopped
                || _threadWriteInfo.ThreadState == System.Threading.ThreadState.Aborted)
            {
                _threadWriteInfo = new Thread(WriteProcessInfo) { IsBackground = true};
                _threadWriteInfo.Start();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(_threadWriteInfo.ThreadState.ToString());
            }
        }


        // handler for Thread #2
        private void Button_Click_Thread_2(object sender, RoutedEventArgs e)
        {
            if (_threadScreenShoots == null
                || _threadScreenShoots.ThreadState == System.Threading.ThreadState.AbortRequested
                || _threadScreenShoots.ThreadState == System.Threading.ThreadState.Stopped
                || _threadScreenShoots.ThreadState == System.Threading.ThreadState.Aborted)
            {
                _threadScreenShoots = new Thread(SaveScreenShoots) { IsBackground = true };
                _threadScreenShoots.Start();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(_threadScreenShoots.ThreadState.ToString());
            }
        }


        // handler for Thread #3
        private void Button_Click_Thread_3(object sender, RoutedEventArgs e)
        {
            _countStringScreen = new Thread(CountingRowsScreen);
            _countStringScreen.Start();
        }


        // Stop Thread #1
        private void Stop_Thread_1(object sender, RoutedEventArgs e)
        {
            _threadWriteInfo.Abort();
        }


        // Stop Thread #2
        private void Stop_Thread_2(object sender, RoutedEventArgs e)
        {
            _threadScreenShoots.Abort();
        }


        // write info about process method
        private void WriteProcessInfo()
        {
            Process currentProcess = Process.GetCurrentProcess();

            while (true)
            {
                _infoProcess = String.Format("PID процесса: {0}\r\nИмя процесса: {1}\r\nВремя запуска: {2}\r\n",
                currentProcess.Id, currentProcess.ProcessName, currentProcess.StartTime);
                OpenFile(Stream.Write);
            }
        }


        // saveing screenshots method 
        private void SaveScreenShoots()
        {
            if (!Directory.Exists(_pathScreenShotsDirectory))
            {
                Directory.CreateDirectory(_pathScreenShotsDirectory);
            }

            while (true)
            { 
                graph.CopyFromScreen(0, 0, 0, 0, btm.Size);
                btm.Save(_pathScreenShots + "\\" + counter.ToString() + ".png");
                counter++;
                Thread.Sleep(10000);
            }
        }


        // counting rows and screenShoots and writing in file method
        private void CountingRowsScreen()
        {
            int lenthScreen;
            int lengthRows = OpenFile(Stream.Read);

            if (Directory.Exists(_pathScreenShotsDirectory))
            {
                lenthScreen = Directory.GetFiles(_pathScreenShots).Length;
            }
            else
            {
                lenthScreen = 0;
            }

            // for checking
            //System.Windows.Forms.MessageBox.Show(lenthScreen.ToString() + "   " + lengthRows.ToString());

            using (StreamWriter sw = new StreamWriter("CountRowsAndScreens.txt", true))
            {
                sw.WriteLine("Количество строк: {0}", lengthRows);
                sw.WriteLine("Количество скринов: {0}", lenthScreen);
                sw.WriteLine("Время проверки: " + DateTime.Now + "\r\n");
            }
        }


        // Openning file(locker) method
        private int OpenFile(Stream s)
        {
            lock (locker)
            {
                switch (s)
                {
                    case Stream.Read:
                        if (File.Exists(_pathInfoProcess))
                        {
                            return File.ReadAllLines(_pathInfoProcess).Length;
                        }
                        else
                            return 0;
                    case Stream.Write:
                        using (StreamWriter sw = new StreamWriter(_pathInfoProcess, true))
                        {
                            sw.WriteLine(_infoProcess);
                            Thread.Sleep(10000);
                        }
                        return 0;
                }
                return 0;
            }
        }
    }
}

