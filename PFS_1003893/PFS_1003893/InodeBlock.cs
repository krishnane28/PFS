using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    //This class represents the set of blocks containing the inode for the file
    class InodeBlock
    {
        //This variable iNode represents the individual file in the inode blocks 
        public Inode[] iNode = new Inode[4];
        #region Constructor for the InodeBlock class
        public InodeBlock()
        {
            for(int i = 0; i < 256/64; i++)
            {
                iNode[i] = new Inode();
            }
        }
        #endregion
    }
}
