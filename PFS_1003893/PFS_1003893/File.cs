using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    class File
    {
        private String fileName;
        private int fileSize;
        private DateTime creationTime;
        private DateTime creationDate;
        private int startingBlockId;
        private int endingBlockId;
        private String remarks;
        public String FileName
        {
            get
            {
                return fileName;
            }
        }
        public int FileSize
        {
            get
            {
                return fileSize;
            }
        }
        public File()
        {

        }
    }
}
