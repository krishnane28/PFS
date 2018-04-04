using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    class Disk_1003893
    {
        private int blockSize = 256;
        public int BlockSize
        {
            get
            {
                return blockSize;
            }
        }
        private int totalBlocks = 80;
        public int TotalBlocks
        {
            get
            {
                return totalBlocks;
            }
        }
        private FileStream myPFS;
        private String path = @"D:\PFS_1003893.txt";
        //This is the constructor for the Disk_1003893 class
        public Disk_1003893()
        {
            myPFS = System.IO.File.Create(path);
        }
        //This method makes the pointer in the file to point to the particular location in my file system
        public void seekBlock(int blockNumber)
        {
            myPFS.Seek(blockNumber * blockSize, SeekOrigin.Begin);
        }

        //This method is not currently used
        public void read(int blockNumber, Inode iNode)
        {
            myPFS = System.IO.File.Open(path, FileMode.Open);
            seekBlock(blockNumber);
            using (StreamReader sReader = new StreamReader(myPFS))
            {
                iNode.InodeFlag = Convert.ToInt32(sReader.Read());
                iNode.FileOwner = Convert.ToInt32(sReader.Read());                
                //iNode.FileCreatedTime = Convert.ToDateTime(sReader.Read());
                //iNode.FileCreatedDate = Convert.ToDateTime(sReader.Read());
                iNode.FileSize = Convert.ToInt32(sReader.Read());
                iNode.Remarks = Convert.ToString(sReader.Read());
                for (int j = 0; j < 13; j++)
                {
                    iNode[j] = Convert.ToInt32(sReader.Read());
                }
            }
        }

        //This method reads the Inodes in the Inode block in my file system
        public void read(InodeBlock inodeBlock)
        //public void read(int blockNumber, InodeBlock inodeBlock)
        {
            try
            {
                ////myPFS = System.IO.File.Open(path, FileMode.Open);
                ////seekBlock(blockNumber);
                for(int i = 0; i < inodeBlock.iNode.Length; i++)
                {
                    myPFS = System.IO.File.Open(path, FileMode.Open);
                    seekBlock(i + 1);
                    using (StreamReader sReader = new StreamReader(myPFS))
                    {
                        inodeBlock.iNode[i].InodeFlag = Convert.ToInt32(sReader.Read());
                        inodeBlock.iNode[i].FileOwner = Convert.ToInt32(sReader.Read());
                        inodeBlock.iNode[i].FileCreatedDate = Convert.ToDateTime(sReader.Read());
                        inodeBlock.iNode[i].FileCreatedTime = Convert.ToDateTime(sReader.Read());
                        inodeBlock.iNode[i].FileSize = Convert.ToInt32(sReader.Read());
                        inodeBlock.iNode[i].Remarks = Convert.ToString(sReader.Read());
                        for(int j = 0; j < 13; j++)
                        {
                            inodeBlock.iNode[i][j] = Convert.ToInt32(sReader.Read());
                        }
                        
                    }
                }
            }
            catch(Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
        }

        //This method checks whether there are available free data blocks in my file system
        public int readDataBlocks(SuperBlock superBlock)
        {            
            for (int i = 5; i < superBlock.FreeBlocks; i++)
            {
                myPFS = System.IO.File.Open(path, FileMode.Open);
                seekBlock(i);
                using (StreamReader sReader = new StreamReader(myPFS))
                {
                    if(sReader.Read() == '\0')
                    {
                        return i;
                    }
                }
            }
            //-1 implies no free data blocks available
            return -1;
        }

        //This method writes the required data in the super block in my file system
        public void write(int blockNumber, SuperBlock superBlock)
        {
            seekBlock(blockNumber);
            using (StreamWriter sWriter = new StreamWriter(myPFS))
            {
                sWriter.Write(superBlock.FileSystemBlocks);
                sWriter.Write(superBlock.FreeBlocks);
            }
        }

        //This method is not currently used 
        public void write(int blockNumber, Inode iNode)
        {
            myPFS = System.IO.File.Open(path, FileMode.Open);
            seekBlock(blockNumber);
            using (StreamWriter sWriter = new StreamWriter(myPFS))
            {
                sWriter.Write(iNode.InodeFlag);
                sWriter.Write(iNode.FileOwner);
                sWriter.Write(iNode.FileCreatedDate);
                sWriter.Write(iNode.FileCreatedTime);
                sWriter.Write(iNode.FileSize);
                sWriter.Write(iNode.Remarks);
                for (int j = 0; j < 13; j++)
                {
                    sWriter.Write(iNode[j]);
                }
                foreach(var item in iNode.dataBlocks)
                {
                    sWriter.Write(item);
                }
            }
        }
        //This method writes the required data in the respective Inodes in the Inode block
        public void write(InodeBlock iNodeBlock)
        ////public void write(int blockNumber, InodeBlock iNodeBlock)
        {
            ////myPFS = System.IO.File.Open(path, FileMode.Open);
            ////seekBlock(blockNumber);
            ////using (StreamWriter sWriter = new StreamWriter(myPFS))
            ////{
            try
            {
                for (int i = 0; i < iNodeBlock.iNode.Length; i++)
                {
                    myPFS = System.IO.File.Open(path, FileMode.Open);
                    seekBlock(i + 1);
                    using (StreamWriter sWriter = new StreamWriter(myPFS))
                    {
                        //Console.WriteLine(i);
                        sWriter.Write(iNodeBlock.iNode[i].InodeFlag);
                        sWriter.Write(iNodeBlock.iNode[i].FileOwner);
                        sWriter.Write(iNodeBlock.iNode[i].FileCreatedDate);
                        sWriter.Write(iNodeBlock.iNode[i].FileCreatedTime);
                        sWriter.Write(iNodeBlock.iNode[i].FileSize);
                        sWriter.Write(iNodeBlock.iNode[i].Remarks);
                        for (int j = 0; j < 13; j++)
                        {
                            sWriter.Write(iNodeBlock.iNode[i][j]);
                        }
                        foreach (var item in iNodeBlock.iNode[i].dataBlocks)
                        {
                            sWriter.Write(item);
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
           //// }
        }
        /*
        public void Write(int blockNumber, InodeBlock iNodeBlock)
        {
            try
            {
                myPFS = System.IO.File.Open(path, FileMode.Open);
                seekBlock(blockNumber);
                for(int i = 0; i < iNodeBlock.iNode.Length; i++)
                {
                    using (StreamWriter sWriter = new StreamWriter(myPFS))
                    {
                        sWriter.Write(iNodeBlock.iNode[i].InodeFlag);
                        sWriter.Write(iNodeBlock.iNode[i].FileOwner);
                        sWriter.Write(iNodeBlock.iNode[i].FileCreatedDate);
                        sWriter.Write(iNodeBlock.iNode[i].FileCreatedTime);
                        sWriter.Write(iNodeBlock.iNode[i].FileSize);
                        sWriter.Write(iNodeBlock.iNode[i].Remarks);
                        for (int j = 0; j < 13; j++)
                        {
                            sWriter.Write(iNodeBlock.iNode[i][j]);
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        */
        //This method creates the data blocks without data in it in my file system
        public void initializeDataBlocks(int blockNumber)
        {
            try
            {
                myPFS = System.IO.File.Open(path, FileMode.Open);
                seekBlock(blockNumber);
                using (StreamWriter sWriter = new StreamWriter(myPFS))
                {
                    //sWriter.Write(-1);
                    //for(int i = 0; i < blockSize - 4; i++)
                    //{
                    //    sWriter.Write(' ');                   
                    //}
                    sWriter.Write('\0');
                }
            }
            catch(Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
        }

        //This method reads data from the physical blocks in my file system
        public void readDataBlocks(int blockNumber)
        {
            try
            {
                myPFS = System.IO.File.Open(path, FileMode.Open);
                seekBlock(blockNumber);
                char[] data = new char[256];
                using (StreamReader sReader = new StreamReader(myPFS))
                {
                    sReader.Read(data, 0, data.Length - 4);
                }
                Console.WriteLine(data);
            }
            catch(Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
        }

        //This method writes data from the external file into the respective available free blocks 
        //in my file system
        public void writeDataBlocks(int blockNumber, char[] input)
        {
            try
            {
                myPFS = System.IO.File.Open(path, FileMode.Open);
                seekBlock(blockNumber);
                using (StreamWriter sWriter = new StreamWriter(myPFS))
                {
                    sWriter.Write(input);
                }
            }
            catch(Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
        }

        //This method removes the mentioned file from my file system
        public void removeFile(int blockNumber)
        {
            try
            {
                myPFS = System.IO.File.Open(path, FileMode.Open);
                seekBlock(blockNumber);
                using (StreamWriter sWriter = new StreamWriter(myPFS))
                {
                    for(int i = 0; i < 252; i++)
                    {
                        sWriter.Write("\0");
                    }
                }
            }
            catch(Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
        }
    }
}
