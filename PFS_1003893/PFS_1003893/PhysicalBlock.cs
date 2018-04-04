using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    class PhysicalBlock
    {
        #region Private fields for Physical Block
        private PhysicalBlock nextPB;
        private char[] data = new char[252];
        private int pointerToNextPB;
        #endregion

        public PhysicalBlock NextPB
        {
            get
            {
                return nextPB;
            }
            set
            {
                nextPB = value;
            }
        }

        public int PointerToNextPB
        {
            get
            {
                return pointerToNextPB;
            }
            set
            {
                pointerToNextPB = value;
            }
        }

        public char this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }


    }
}
