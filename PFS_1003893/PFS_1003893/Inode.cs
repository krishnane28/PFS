using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    class Inode
    {
        //variable inodeSize defines the size of the file's inode and is of 64 bytes 
        private int inodeSize = 64;
        //flag is used to indicate whether the inode is in use or not
        //0 indicates it is unused and 1 indicates it is in use
        private int flag = 0;
        public LinkedList<int> dataBlocks = new LinkedList<int>();
        public int pointerSize = 13;
        public int InodeFlag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
            }
        }
        //variable owner denotes the owner who created the file
        private int owner = 0;
        public int FileOwner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }
        //variable fileSize contains the size of the file
        private int fileSize = 0;
        public int FileSize
        {
            get
            {
                return fileSize;
            }
            set
            {
                fileSize = value;
            }
        }
        //variable pointer points to the data block where the contents of the file are stored
        private int[] pointer = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public int this[int index]
        {
            get
            {
                return pointer[index];
            }
            set
            {
                pointer[index] = value;
            }
        }
        //fileCreatedDate contains the date on which the file has been created
        private DateTime fileCreatedDate;
        public DateTime FileCreatedDate
        {
            get
            {
                return fileCreatedDate;
            }
            set
            {
                fileCreatedDate = value;
            }
        }
        //fileCreatedTime contains the time at which the file has been created
        private DateTime fileCreatedTime;
        public DateTime FileCreatedTime
        {
            get
            {
                return fileCreatedTime;
            }
            set
            {
                fileCreatedTime = value;
            }
        }
        //remarks contains the remarks for the file
        private String remarks = "N/A";
        public String Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
            }
        }

    }
}
