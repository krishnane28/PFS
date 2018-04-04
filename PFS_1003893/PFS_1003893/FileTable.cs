using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    class FileTable
    {
        private int total_files = 4;
        private int[] bitmap;
        public int this[int index]
        {
            get
            {
                return bitmap[index];
            }
            set
            {
                bitmap[index] = value;
            }
        }
        public FileDescriptor[] fileDescriptor;
        public FileTable()
        {
            fileDescriptor = new FileDescriptor[total_files];
            bitmap = new int[total_files];
            for(int i = 0; i < total_files; i++)
            {
                bitmap[i] = 0;
            }
        }
        public int allocate()
        {
            for(int i = 0; i < total_files; i++)
            {
                if(bitmap[i] == 0)
                {
                    return i;
                }
            }
            //-1 implies file table is full i.e. File system is full and we cannot store any additional files
            return -1;
        }
        public void deallocate(int bitmapLocation)
        {
            bitmap[bitmapLocation] = 0;
        }
        public int add(Inode iNode, int fileName, int _fileDescriptor)
        {
            if(bitmap[_fileDescriptor] != 0)
            {
                return -1;
            }
            else
            {
                bitmap[_fileDescriptor] = 1;
                fileDescriptor[_fileDescriptor] = new FileDescriptor(iNode, fileName);
                return 0;
            }
        }
        public Inode getInode(int _fileDescriptor)
        {
            if(bitmap[_fileDescriptor] == 0)
            {
                return null;
            }
            else
            {
                return fileDescriptor[_fileDescriptor]._iNode;
            }
        }
        public void freeFileTable(int fileDescriptor)
        {
            bitmap[fileDescriptor] = 0;
        }
    }
}
