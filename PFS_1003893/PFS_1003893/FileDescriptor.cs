using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    //This class represents each file in my file system
    class FileDescriptor
    {
        private Inode iNode;
        public Inode _iNode
        {
            get
            {
                return iNode;
            }
            set
            {
                iNode = value;
            }
        }
        private int fileName;
        public int FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
        private int seekPointer;
        public int SeekPointer
        {
            get
            {
                return seekPointer;
            }
            set
            {
                seekPointer = value;
            }
        }
        public FileDescriptor(Inode _iNode, int _fileName)
        {
            this.iNode = _iNode;
            this.fileName = _fileName;
        }
    }
}
