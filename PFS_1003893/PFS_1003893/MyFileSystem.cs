using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFS_1003893
{
    class MyFileSystem
    {
        #region Private fields for MyFileSystem Class
        private SuperBlock superBlock;
        private Disk_1003893 myDisk;
        private InodeBlock inodeBlock;
        private FileTable fileTable;
        #endregion

        #region Constructor for my file system
        public MyFileSystem()
        {
            myDisk = new Disk_1003893();
            superBlock = new SuperBlock();
            fileTable = new FileTable();
        }
        #endregion

        #region Constructor for initializing my file system
        //This method initializes the file system with the data for the super block as well as 
        //data for the inode blocks
        public void initializeDisk(int size)
        {
            //This sets the total blocks available in the file system
            superBlock.FileSystemBlocks = size;
            //This sets the total available data blocks in the file system
            superBlock.FreeBlocks = size - 5;
            //This method writes the first block in the file which is the super block 
            //with the required data about the file system
            myDisk.write(0, superBlock);
            inodeBlock = new InodeBlock();
            //int count = 0;
            ////for(int i = 1; i <= inodeBlock.iNode.Length; i++)
            ////{
                //This method creates the inode block with the required inode data in each block
                ////myDisk.write(i, inodeBlock.iNode[count]);
                myDisk.write(inodeBlock);
            ////count++;
            ////}
            for (int i = 5; i < superBlock.FreeBlocks; i++)
            {
                myDisk.initializeDataBlocks(i);
            }
        }
        #endregion

        #region Create the file in the file system and insert data from the file for the "put" command 
        //This method checks for any free Inode and also any free space in the file table for storing
        //our new file and returns the file descriptor
        public int createFile(int fileName)
        {
            //InodeBlock iNodeBlock = new InodeBlock();
            ////for(int i = 1; i <= (superBlock.FileSystemBlocks); i++)
            ////{
                //This method reads the InodeBlock to check for any free Inode for the file
                ////myDisk.read(i, inodeBlock);
                myDisk.read(inodeBlock);
                for (int j = 0; j < inodeBlock.iNode.Length; j++)
                {
                    //Inode iNode = inodeBlock.iNode[j];
                    //If the InodeFlag in an Inode is 0 implies we have one free Inode available for a file
                    if(inodeBlock.iNode[j].InodeFlag == 0)
                    ////if(iNode.InodeFlag == 0)
                    {
                        //This method checks the bitmap in the file table to check we have space in the file table
                        //to create a file
                        int fileDescriptor = fileTable.allocate();
                        //If available then rewrite the Inode data with the file attributes
                        if(fileDescriptor >= 0)
                        {
                            inodeBlock.iNode[fileDescriptor].InodeFlag = 1;
                            inodeBlock.iNode[fileDescriptor].FileOwner = fileName;
                            inodeBlock.iNode[fileDescriptor].FileCreatedDate = DateTime.Now;
                            inodeBlock.iNode[fileDescriptor].FileCreatedTime = DateTime.Now;
                            inodeBlock.iNode[fileDescriptor].Remarks = "Created by " + fileName;
                            //iNode.InodeFlag = 1;
                            //iNode.FileOwner = fileName;
                            //iNode.FileSize = 0;
                            //iNode.FileCreatedDate = DateTime.Now;
                            //iNode.FileCreatedTime = DateTime.Now;
                            //iNode.Remarks = "Created by " + fileName;
                            for(int k = 0; k < inodeBlock.iNode[j].pointerSize; k++)
                            ////for(int k = 0; k < iNode.pointerSize; k++)
                            {
                                ////iNode[k] = 0;
                                inodeBlock.iNode[fileDescriptor][k] = 0;
                            }
                            //This method writes the selected free Inode with the required attributes of the file
                            ////myDisk.write(i, iNode);
                            myDisk.write(inodeBlock);
                            //This method sets the corresponding bitmap to 1 to indicate an open file
                            //is present at the mentioned index in the file table
                            fileTable[fileDescriptor] = 1;
                            //This creates the new file descriptor which represents the file in the file system
                            fileTable.fileDescriptor[fileDescriptor] = new FileDescriptor(inodeBlock.iNode[fileDescriptor]/*iNode*/, fileName);
                        }
                        return fileDescriptor;
                    }
                }
            ////}
            //-1 implies no Inode is available for a new file
            return -1; 
        }
        #endregion

        #region Read data from the physical blocks
        //This method reads data from the physical blocks in the file system
        public void readFromDataBlocks(int fileNumber)
        {
            myDisk.read(inodeBlock);
            int physicalBlockCount = 0;
            for(int i = 0; i < inodeBlock.iNode.Length; i++)
            {               
                if(inodeBlock.iNode[i].FileOwner == fileNumber)
                {
                    for(int j = 0; j < inodeBlock.iNode[i].pointerSize; j++)
                    {
                        if(inodeBlock.iNode[i][j] != 0)
                        {
                            myDisk.readDataBlocks(inodeBlock.iNode[i][j]);
                            //Console.WriteLine("Data blocks" + inodeBlock.iNode[i][j]);
                            physicalBlockCount++;
                        }
                    }
                    
                }
            }   
            if(physicalBlockCount == 0)
            {
                Console.WriteLine("No such file exists in the file system");
            }
        }
        #endregion

        #region Write data from the selected file into the free data blocks in my file system
        public int writeInFreeDataBlocks(int fileDescriptor, char[] fileInput)
        {
            char[][] inputToDataBlocks = new char[(fileInput.Length/252) + 1][];
            int readCount = 0;
            int physicalBlockNumber = -1;
            //inodeBlock.iNode[fileDescriptor] = fileTable.getInode(fileDescriptor);
            //Inode iNode = fileTable.getInode(fileDescriptor);
            //Inode iNode = inodeBlock.iNode[fileDescriptor];

            //////myDisk.read(fileDescriptor + 1, inodeBlock.iNode[fileDescriptor]); not this one
            myDisk.read(inodeBlock);
            ////myDisk.read(fileDescriptor + 1, inodeBlock);
            inodeBlock.iNode[fileDescriptor].Remarks = "Owner is " + fileDescriptor;
            inodeBlock.iNode[fileDescriptor].FileSize = fileInput.Length;
            char[] data = new char[myDisk.BlockSize]; 
            if(fileInput.Length <= 252)
            {

            }
            else
            {
                for(int i = 0; i <= (fileInput.Length / 252); i++)
                {
                    if(i == (fileInput.Length / 252))
                    {
                        inputToDataBlocks[i] = new char[fileInput.Length % 252];
                    }
                    else
                    {
                        inputToDataBlocks[i] = new char[252];
                    }          
                    physicalBlockNumber = myDisk.readDataBlocks(superBlock);
                    if (physicalBlockNumber == -1)
                    {
                        return -1;
                    }
                    else
                    {
                        for (int a = 0; a < 13; a++)
                        {
                            if (inodeBlock.iNode[fileDescriptor][a] == 0)
                            {
                                inodeBlock.iNode[fileDescriptor][a] = physicalBlockNumber;
                                inodeBlock.iNode[fileDescriptor].dataBlocks.AddLast(physicalBlockNumber);
                                break;
                            }
                        }
                        //iNode[i] = physicalBlockNumber;

                        for (int k = 0; k < (i == (fileInput.Length / 252) ? (fileInput.Length % 252) : 252); k++)
                        {
                            inputToDataBlocks[i][k] = fileInput[readCount];
                            readCount++;
                        }
                        myDisk.writeDataBlocks(physicalBlockNumber, inputToDataBlocks[i]);
                    }
                }
                //inodeBlock.iNode[fileDescriptor] = iNode;
                ////myDisk.write(fileDescriptor + 1, inodeBlock.iNode[fileDescriptor]);
                myDisk.write(inodeBlock);
            }
            return 1;
        }
        #endregion

        #region Get the list of files in the file system
        public int[] getFilesList()
        {
            myDisk.read(inodeBlock);
            int[] files = new int[4];
            for(int i = 0; i < inodeBlock.iNode.Length; i++)
            {
                if(inodeBlock.iNode[i].InodeFlag != 0)
                {
                    files[i] = inodeBlock.iNode[i].FileOwner;
                }
            }
            return files;
        }
        #endregion

        #region Allocate a new data block in my file system for the selected file's data
        //public int allocateDataBlock(int blockNumber, Inode iNode)
        //{
        //    if(superBlock.FreeBlocks == 0)
        //    {
        //        return -1;
        //    }
        //    int freeBlock = superBlock.FreeBlocks;
        //}
        #endregion

        #region Removes the mentioned file in the file system
        public void removeFileContents(int fileNumber)
        {
            myDisk.read(inodeBlock);
            int physicalBlockCount = 0;
            for (int i = 0; i < inodeBlock.iNode.Length; i++)
            {
                if (inodeBlock.iNode[i].FileOwner == fileNumber)
                {
                    fileTable.deallocate(i);
                    inodeBlock.iNode[i].InodeFlag = 0;
                    inodeBlock.iNode[i].FileOwner = 0;
                    inodeBlock.iNode[i].FileCreatedDate = Convert.ToDateTime(null);
                    inodeBlock.iNode[i].FileCreatedTime = Convert.ToDateTime(null);
                    inodeBlock.iNode[i].FileSize = 0;
                    inodeBlock.iNode[i].Remarks = "N/A";
                    for (int j = 0; j < inodeBlock.iNode[i].pointerSize; j++)
                    {
                        if (inodeBlock.iNode[i][j] != 0)
                        {                            
                            myDisk.removeFile(inodeBlock.iNode[i][j]);
                            //Console.WriteLine("Data blocks" + inodeBlock.iNode[i][j]);
                            inodeBlock.iNode[i][j] = 0;
                            physicalBlockCount++;
                        }
                    }
                    inodeBlock.iNode[i].dataBlocks.Clear();
                    foreach (var item in inodeBlock.iNode[i].dataBlocks)
                    {
                        inodeBlock.iNode[i].dataBlocks.AddLast(0);
                    }
                }
            }
            if (physicalBlockCount == 0)
            {
                Console.WriteLine("No such file exists in the file system");
            }
            else
            {
                myDisk.write(inodeBlock);
                Console.WriteLine("Mentioned file has been deleted successfully");
            }
        }
        #endregion
    }
}
