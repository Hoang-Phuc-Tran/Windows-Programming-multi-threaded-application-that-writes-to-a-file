/*
 * Project:	    A04 – TASKS
 * Author:	    Hoang Phuc Tran - 8789102
 * Date:		November 2, 2022
 * Description: This file contains a FileNew class and its properties. This class is used to 
 * write to a file and monitor the size of a file.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace A04_Tasks
{
    /*
     * CLASS NAME:  FileNew
     * PURPOSE : a FileNew class and its properties. This class is used to 
     * write to a file and monitor the size of a file.
     */
    internal class FileNew
    {
        private StreamWriter writeFile = null;
        Mutex mutex = null;
        public volatile bool validateSize = false;

        /*  -- Method Header Comment
        Name	: FileNew -- CONSTRUCTOR
        Purpose : to instantiate a new FileNew object - given a set of attribute values
        Inputs	: NONE
        Outputs	: NONE
        Returns	: Nothing
        */
        public FileNew()
        {
            if (!Mutex.TryOpenExisting("Mutex", out mutex))
            {
                mutex = new Mutex(true, "Mutex");
                mutex.ReleaseMutex();
            }
        }

        /*  -- Method Header Comment
	    Name	: monitor
	    Purpose : this property will monitor the size of a file
	    Inputs	:	a string             pathName
                    a int                size
                    an array Task        taskArray
	    Outputs	:	the size of a file at 0.5 second intervals
	    Returns	:	NONE
        */
        internal void monitor(string pathName, int size, Task[] taskArray)
        {
            FileInfo fileNew = null;
            while (!validateSize)
            {
                // Check if the pathName does exist
                if (File.Exists(pathName))
                {
                    fileNew = new FileInfo(pathName);
                // Check if the maximum size is reached
                    if (fileNew.Length >= size)
                     {
                         validateSize = true;
                     }
                }
                if (validateSize)
                {
                    // Wait to all taskArray finish
                    foreach (Task aTask in taskArray)
                    {
                        aTask.Wait();
                    }
                }
                Console.WriteLine(fileNew.Length);
                Thread.Sleep(500);
            }
            Console.WriteLine("The final size of the file is " + fileNew.Length);
        }

        /*  -- Method Header Comment
	    Name	: WriteToFile
	    Purpose : this property will write a random data to a file if the expected size of the file is not reached
	    Inputs	:	a string    pathName
	    Outputs	:	NONE
	    Returns	:	NONE
        */
        internal void WriteToFile(string pathName)
        {
            while (!validateSize)
            {
                mutex.WaitOne();
                try
                {
                    using (writeFile = new StreamWriter(pathName, true))
                    {
                        // Write a random data
                        writeFile.WriteLine(Guid.NewGuid());
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Exception: " + exception.Message);
                }

                finally
                {
                    // Close file
                    if (writeFile != null)
                    {
                        writeFile.Close();
                    }
                }
                mutex.ReleaseMutex();
            }
        }
    }
}
