/*
 * Project:	    A04 – TASKS
 * Author:	    Hoang Phuc Tran - 8789102
 * Date:		November 2, 2022
 * Description: An application use Tasks to develop a “multi-threaded” application that writes to a
file. It will be monitoring the size of the file, and when it becomes a certain size, the
application will close gracefully.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace A04_Tasks
{
    /*
     * CLASS NAME:  Program
     * PURPOSE : use Tasks to develop a “multi-threaded” application that writes to a
     file. It will be monitoring the size of the file, and when it becomes a certain size, the
     application will close gracefully.
     */
    internal class Program
    {
        static int Main(string[] args)
        {
            // Create mutex
            Mutex mutex = new Mutex();

            // A list contains tasks
            List<Task> TaskList = new List<Task>();

            // Maximum tasks
            const int maximumTasks = 25;

            var checkFullPath = false;
            double length = 0;

            // Size of file
            double sizeOfFile = 0;

            // The pathname of a file
            string path;

            // Chekc if a command line contains 2 agurments
            if (args.Length == 2)
            {
                path = args[0];

                // Validate the root of the file
                checkFullPath = Path.IsPathRooted(path);

                // Convert string to double
                var isNumeric = double.TryParse(args[1], out double size);
                sizeOfFile = size;

                // Check if 2 agruments are correct
                if (isNumeric == true && size >= 1000 && size <= 20000000 && checkFullPath == true)
                {
                    // Check if the file exists
                    if (File.Exists(path))
                    {
                        Console.WriteLine("The file already exists, Do you want to overwrite? [y/n]");

                        string userInput = Console.ReadLine();
                        
                        // User wants to overrite the file
                        if (userInput == "y" || userInput == "Y")
                        {
                            // This task monitor the size of the file at 0.5 second intervals
                            Task.Factory.StartNew(() =>
                            {
                                // Check if the file is lower than the expected size
                                while (length < sizeOfFile)
                                {
                                    length = new System.IO.FileInfo(path).Length;
                                    Console.WriteLine(length);
                                    System.Threading.Thread.Sleep(500);
                                }
                                if (length > sizeOfFile)
                                {
                                    Console.WriteLine("The final size of the file is " + length);
                                }
                            });

                            try
                            {
                                // clear the file
                                File.WriteAllText(path, string.Empty);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("The pathname of the file is incorrect.");
                                Console.WriteLine("Type /? for more information.\n");
                                return -1;
                            }

                            // Create 25 tasks
                            for (int i = 0; i < maximumTasks; i++)
                            {
                                // Using mutex to limit tasks acess the file 
                                if (mutex.WaitOne())
                                {
                                    // using try and finally to release the mutex
                                    try
                                    {
                                        var task = new Task(() => WokerFile(path, sizeOfFile));
                                        task.Start();
                                        TaskList.Add(task);
                                    }
                                    //Release the mutex
                                    finally
                                    {
                                        mutex.ReleaseMutex();
                                    }
                                }
                            }                          
                            // Wait all tasks complete
                            Task.WhenAll(TaskList.ToArray());
                        }
                    }
                    // If the file does not exist
                    
                }
                else
                {
                    Console.WriteLine("Your arguments are not correct.");
                    Console.WriteLine("Type /? for more information.\n");
                }
            }
            // Check if the user enters the /? switch after the command as the first command line argument. 
            else if (args.Length == 1 && args[0] == "/?")
            {
                Console.WriteLine("The first argument is the full pathname of the file (e.g., C:\\MinGW\\lib\\text.txt).");
                Console.WriteLine("The second arugment is the size of the file (Minimum is 1000 and Maximum is 20-000-000).\n");
            }
            else
            {
                Console.WriteLine("Your arguments are not correct.");
                Console.WriteLine("Type /? for more information.\n");

            }
            Console.ReadKey();
            return 0;
        }

        /*  -- Method Header Comment
	    Name	: WokerFile
	    Purpose : this property will return and set the data member (interestRate).
	    Inputs	:	a string    filePath
                    a double    fileSize
	    Outputs	:	NONE
	    Returns	:	NONE
        */
        static void WokerFile(string filePath, double fileSize)
        {
            double length = 0;
            while (length < fileSize)
            {
                // Create a new guid
                string guid = Guid.NewGuid().ToString();
                // Append the guid to a file
                File.AppendAllText(filePath, guid);
                length = new System.IO.FileInfo(filePath).Length;
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
