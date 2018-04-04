using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFS_1003893;

namespace PFS_1003893
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get all the drives available on the system 
            //DriveInfo[] systemDrives = DriveInfo.GetDrives();
            //String rootDirectory = systemDrives[0].Name + "PFS_1003893";
            //PortableFileSystem myFileSystem = new PortableFileSystem(rootDirectory);
            //Console.WriteLine(rootDirectory);
            //This variable counts the number of times the user has entered open command
            int count = 0;
            int killCommandCount = 0;
            String userCommand = "";
            MyFileSystem myFileSystem = null;
            while (!userCommand.Equals("quit"))
            {
                Console.Write("PFS_1003893>");
                userCommand = Console.ReadLine();
                String[] userCommands = userCommand.Split(' ');
                //Operation to be performed for the open command
                if (userCommand.Equals("open"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("You cannot use open command because the file system has been killed");
                    }
                    else
                    {
                        if (count == 0)
                        {
                            myFileSystem = new MyFileSystem();
                            myFileSystem.initializeDisk(40);
                            count++;
                        }
                        else
                        {
                            Console.WriteLine("You cannot create the file system again");
                        }
                    }
                }
                //Operation to be performed for the put command
                else if (userCommands[0].Equals("put"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("You cannot use put command because the file system has been killed");
                    }
                    else
                    {
                        if (userCommands.Length == 1)
                        {
                            Console.WriteLine("Please mention the file name for the put command");
                        }
                        else if (userCommands.Length > 2)
                        {
                            Console.WriteLine("Invalid put command");
                        }
                        else
                        {
                            String path = @"D:\" + userCommands[1] + ".txt";
                            FileStream myFile = System.IO.File.Open(path, FileMode.Open);
                            char[] fileInput = new char[myFile.Length];
                            int fileName = 0;
                            using (StreamReader sReader = new StreamReader(myFile))
                            {
                                for (int i = 0; i < fileInput.Length; i++)
                                {
                                    fileInput[i] = Convert.ToChar(sReader.Read());
                                }
                            }
                            //Console.Write(fileInput);
                            //Console.WriteLine();
                            //Console.WriteLine(fileInput.Length);
                            if (userCommands[1].Equals("text1"))
                            {
                                fileName = 1;
                            }
                            else if (userCommands[1].Equals("text2"))
                            {
                                fileName = 2;
                            }
                            else
                            {
                                fileName = 3;
                            }
                            int fileDescriptor = myFileSystem.createFile(fileName);
                            Console.WriteLine("File Descriptor is " + fileDescriptor);
                            if (fileDescriptor == -1)
                            {
                                Console.WriteLine("No free Inodes available or no space in the file table");
                            }
                            else
                            {
                                int result = myFileSystem.writeInFreeDataBlocks(fileDescriptor, fileInput);
                                if (result == 1)
                                {
                                    Console.WriteLine("Data successfully written into the file system");
                                }
                                else
                                {
                                    Console.WriteLine("File system is full");
                                }
                            }
                        }
                    }
                }
                //Operation to be performed for the get command
                else if (userCommands[0].Equals("get"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("You cannot use get command because the file system has been killed");
                    }
                    else
                    {
                        if (userCommands.Length == 1)
                        {
                            Console.WriteLine("Please mention the file name for the get command");
                        }
                        else if (userCommands.Length > 2)
                        {
                            Console.WriteLine("Invalid get command");
                        }
                        else
                        {
                            if (userCommands[1].Equals("text1"))
                            {
                                myFileSystem.readFromDataBlocks(1);
                            }
                            else if (userCommands[1].Equals("text2"))
                            {
                                myFileSystem.readFromDataBlocks(2);
                            }
                            else
                            {
                                myFileSystem.readFromDataBlocks(3);
                            }
                        }
                    }                   
                }
                //Operation to be performed for the rm command
                else if(userCommands[0].Equals("rm"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("You cannot use rm command because the file system has been killed");
                    }
                    else
                    {
                        if (userCommands.Length == 1)
                        {
                            Console.WriteLine("Please mention the file name for the rm command");
                        }
                        else if (userCommands.Length > 2)
                        {
                            Console.WriteLine("Invalid rm command");
                        }
                        else
                        {
                            if (userCommands[1].Equals("text1"))
                            {
                                myFileSystem.removeFileContents(1);
                            }
                            else if (userCommands[1].Equals("text2"))
                            {
                                myFileSystem.removeFileContents(2);
                            }
                            else
                            {
                                myFileSystem.removeFileContents(3);
                            }
                        }
                    }             
                }
                //Operation to be performed for the dir command
                else if (userCommands[0].Equals("dir"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("You cannot use dir command because the file system has been killed");
                    }
                    else
                    {
                        if (userCommands.Length > 1)
                        {
                            Console.WriteLine("Invalid dir command");
                        }
                        else
                        {
                            int[] files = myFileSystem.getFilesList();
                            int noOfFiles = 0;
                            for (int i = 0; i < files.Length; i++)
                            {
                                if (files[i] == 1)
                                {
                                    Console.Write("text1");
                                    Console.Write(" ");
                                    noOfFiles++;
                                }
                                else if (files[i] == 2)
                                {
                                    Console.Write("text2");
                                    Console.Write(" ");
                                    noOfFiles++;
                                }
                                else if (files[i] == 3)
                                {
                                    Console.Write("text3");
                                    Console.Write(" ");
                                    noOfFiles++;
                                }
                            }
                            if (noOfFiles == 0)
                            {
                                Console.WriteLine("No files available in the file system");
                            }
                            else
                            {
                                Console.WriteLine();
                            }
                        }
                    }                    
                }
                //Operation to be performed for the kill command
                else if (userCommands[0].Equals("kill"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("File system has already been killed");
                    }
                    else
                    {
                        if (userCommands.Length > 1)
                        {
                            Console.WriteLine("Invalid kill command");
                        }
                        else
                        {
                            String path = @"D:\PFS_1003893.txt";
                            if (!System.IO.File.Exists(path))
                            {
                                Console.WriteLine("File system not exists");
                            }
                            else
                            {
                                killCommandCount = 1;
                                System.IO.File.Delete(path);
                                Console.WriteLine("File system has been deleted");
                            }
                        }
                    }                    
                }
                else if(userCommands[0].Equals("run"))
                {
                    if(killCommandCount == 1)
                    {
                        Console.WriteLine("You cannot use run command because the file sytem has been killed");
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
            ////Operation to be performed for the quit command
            if (userCommand.Equals("quit"))
            {
                if(killCommandCount == 1)
                {
                    Console.WriteLine("File System has already been killed and you cannot use quit command");
                }
                else
                {
                    Console.WriteLine("Shutting down....");
                }
            }
        }
    }
}
