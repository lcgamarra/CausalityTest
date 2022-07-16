using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace CausalityTest
{
    public class LoggingProcess
    {
        public Queue<string> LoggingQueue { get; protected set; }

        public LoggingProcess(Queue<string> loggingQueue)
        {
            LoggingQueue = loggingQueue;
        }

        public void Execute()
        {
            while (true)
            {
                while (true)
                {
                    try
                    {
                        var line = LoggingQueue.Dequeue();

                        string dir = @".\Logs";
                        string path = @".\Logs\HedgeModule.txt";

                        if (!File.Exists(path))
                        {
                            if (Directory.Exists(dir))
                            {
                                File.Create(path);
                                using (var tw = new StreamWriter(path, true))
                                {
                                    tw.WriteLine(line);
                                }
                            }
                            else
                            {
                                Directory.CreateDirectory(dir);

                                using (StreamWriter sw = File.AppendText(path))
                                {
                                    sw.WriteLine(line);
                                }
                            }
                        }
                        else if (File.Exists(path))
                        {
                            using (var tw = new StreamWriter(path, true))
                            {
                                tw.WriteLine(line);
                            }
                        }
                    
                    }
                    catch (InvalidOperationException e)
                    {
                        break; // break when the queue is empty
                    }   
                }

                Thread.Sleep(120000); // Executes every 2 minutes
            }
        }
        
    }
}