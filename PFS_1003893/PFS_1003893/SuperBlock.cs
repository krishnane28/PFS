using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    //This class represents the super block in my file system
    class SuperBlock
    {
        //This field contains the total number of blocks in my file system
        private int fileSystemBlocks;
        public int FileSystemBlocks
        {
            get
            {
                return fileSystemBlocks;
            }
            set
            {
                fileSystemBlocks = value;
            }
        }
        //This field contains the total number of free blocks available in the file system
        private int freeBlocks;
        public int FreeBlocks
        {
            get
            {
                return freeBlocks;
            }
            set
            {
                freeBlocks = value;
            }
        }
    }
}
