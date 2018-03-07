using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopyBlock
{
    class MyThreadRead
    {
        private Thread thrd;
        private readonly AutoResetEvent _autoReset;
        
        private double positionStream;
        private string fromFileName;
        private byte[] array;


        public double Position
        {
            get
            {
                return positionStream;
            }
        }

        public string FromFileName
        {
            get
            {
                return fromFileName;
            }
        }

        public byte[] Array
        {
            get
            {
                return array;
            }
        }


        public MyThreadRead(AutoResetEvent _etc, string path)
        {
            _autoReset = _etc;
            array = new byte[4096];
            fromFileName = path;
        }

        public void Start()
        {
            thrd = new Thread(this.Read);
            thrd.Start();
        }

        private void Read()
        {
            using(FileStream read = File.OpenRead(fromFileName))
            {
                read.Seek((int)positionStream, SeekOrigin.Begin);
                read.Read(array, 0, array.Length);
                positionStream += array.Length;
                _autoReset.Set();
            }
        }
    }
}
