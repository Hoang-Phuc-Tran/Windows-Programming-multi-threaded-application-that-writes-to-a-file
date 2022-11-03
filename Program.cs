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
