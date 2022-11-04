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
            // Maximum tasks
            const int maximumTasks = 25;
            
            // tasks
            Task[] tasks;
            tasks = new Task[maximumTasks];
            Task monitor;

            // Create a FileNew class
            FileNew fileIO = new FileNew();

            // Check if the expected size is reached
            fileIO.validateSize = false;
           
            var checkFullPath = false;
            //double length = 0;

            // Size of file
            int sizeOfFile = 0;

            // The pathname of a file
            string path;

            // Chekc if a command line contains 2 agurments
            if (args.Length == 2)
            {
                path = args[0];

                // Validate the root of the file
                checkFullPath = Path.IsPathRooted(path);

                // Convert string to double
                var isNumeric = Int32.TryParse(args[1], out int size);
                sizeOfFile = size;

                // Check the pathname of a file and the size of the file
                if (checkFullPath == false && (isNumeric == false || size < 1000 || size > 20000000))
                {
                    Console.WriteLine("The pathname of the file is incorrect.");
                    Console.WriteLine("Your size of the file is not correct.");
                    Console.WriteLine("Type /? for more information.\n");
                }
                // Check the pathname of the file
                else if (checkFullPath == false)
                {
                    Console.WriteLine("The pathname of the file is incorrect.");
                    Console.WriteLine("Type /? for more information.\n");
                }
                // Check if 2 agruments are correct
                else if (isNumeric == true && size >= 1000 && size <= 20000000 && checkFullPath == true)
                {
                    // Check if a file exist or not
                    
                    
                }
                else
                {
                    Console.WriteLine("Your size of the file is not correct.");
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
    }
}
