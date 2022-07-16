using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace CausalityTest
{
    public class HedgeCalculationProcess
    {
        public ConcurrentDictionary<string, Array> PosInfo { get; protected set; }
        public bool Syncronized { get; protected set; }
        public AutoResetEvent OutputEvent { get; protected set; }
        public Stack<string> OutputStack { get; protected set; }

        public Queue<string> LoggingQueue { get; protected set; }
        

        public HedgeCalculationProcess(ConcurrentDictionary<string, Array> posInfo, AutoResetEvent outputEvent, Stack<string> outputStack, Queue<string> loggingQueue)
        {
            PosInfo = posInfo;
            Syncronized = false;
            OutputEvent = outputEvent;
            OutputStack = outputStack;
            LoggingQueue = loggingQueue;
        }

        public void Execute()
        {
            while (true)
            {
                if (Syncronized)
                {
                    
                    // Hedge Calculation
                    var totalHedge = 0.0;
                    var SPYPrice = Convert.ToDouble(PosInfo["SPY"].GetValue(0)); 
                    foreach (var pos in PosInfo)
                    {
                        if (!pos.Key.Equals("SPY"))
                        {
                            var partialHedge = 0.0;
                            var price = Convert.ToDouble(pos.Value.GetValue(0));
                            var quantity = Convert.ToDouble(pos.Value.GetValue(1));
                            var beta = Convert.ToDouble(pos.Value.GetValue(2));

                            partialHedge = (-1 * beta * price * quantity) / SPYPrice;
                            totalHedge += partialHedge;
                            
                            // Console.WriteLine(partialHedge);
                        }
                    }
                    
                    var currentSPYPos = Convert.ToInt32(PosInfo["SPY"].GetValue(1));
                    var requiredPos = (int)Math.Round(totalHedge);
                    var adjusment = requiredPos - currentSPYPos;
                    
                    OutputStack.Push("Current SPY Position: " + currentSPYPos + 
                                     " | Required SPY Position: " + requiredPos +
                                     " | Adjustment: " + adjusment);
                    OutputEvent.Set();
                    
                    LoggingQueue.Enqueue(DateTime.Now + " -- Current SPY Position: " + currentSPYPos + 
                                         " | Required SPY Position: " + requiredPos +
                                         " | Adjustment: " + adjusment);
                    
                    
                    // Console.WriteLine("Total Hedge: " + ((int) Math.Round(totalHedge)));
                    
                    Thread.Sleep(30000);
                }
                else
                {
                    if (DateTime.Now.Second == 0 || DateTime.Now.Second == 30)
                    {
                        Console.WriteLine("Synchronized");
                        Console.WriteLine("Executing Hedge Calculation");
                        Syncronized = true;
                        // Continue immediately since the thread is already synchronized
                    }
                    else
                    {
                        Console.WriteLine("Not synchronized yet");
                        Thread.Sleep(500); // Waits half second until the time second is 00 or 30   
                    }
                }
            }
        }
    }
}