using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
//using CopyBlock.Properties;

namespace CopyBlock
{
    class MyThreadWrite
    {
        private Thread _thrd;
        private MyThreadRead _read;
        private string toFileName;
        private double _fileLength;
        private readonly AutoResetEvent _autoReset;

        public MyThreadWrite(AutoResetEvent autoReset, MyThreadRead read, string path)
        {
            _read = read;
            toFileName = path;
            _autoReset = autoReset;
        }

        public double FileLength
        {
            get
            {
                return _fileLength;
            }
        }


        public void Start()
        {
            _thrd = new Thread(Write);
            _thrd.Start();
        }

        public void Write()
        {
                // get length all file
            using (FileStream read = File.OpenRead(_read.FromFileName))
            {
                _fileLength = read.Length;
            }

            while (_read.Position < _fileLength)
            {
                // в главном окне создать этот обьект

                // started reading
                _read.Start();
               
                // wait reading thread 
                _autoReset.WaitOne();

                // and now writing  to file (but now we need read again...)

                // right writeing last block
                if (_fileLength < _read.Position)
                {
                    using (FileStream write = new FileStream(toFileName, FileMode.Append))
                    {
                        write.Write(_read.Array, 0, (int)(_read.Array.Length - (_read.Position - _fileLength)));
                    }
                }
                else
                {

                    using (FileStream write = new FileStream(toFileName, FileMode.Append))
                    {
                        write.Write(_read.Array, 0, _read.Array.Length);
                    }
                }

                MainWindow.barLevel = (((double)(_read.Position / _fileLength)) * 100.0);
            }
            System.Windows.Forms.MessageBox.Show("Записано");
        }
    }
}
