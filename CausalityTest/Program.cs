using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace CausalityTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dataPath = args[0]; 
            
            var initialCapacity = 100; // We set an initial capacity of the dictionary to avoid dictionary resizing
            var concurrencyLevel = 3; // Number of Threads that will interact with the dictionary
            var posInfo = new ConcurrentDictionary<string, Array>(concurrencyLevel, initialCapacity);
            var outputEvent = new AutoResetEvent(false);
            var outputStack = new Stack<string>();
            var loggingQueue = new Queue<string>();

            var reader = new ReaderProcess(posInfo, dataPath);
            var hedgeCalculation = new HedgeCalculationProcess(posInfo, outputEvent, outputStack, loggingQueue);
            var output = new OutputProcess(outputEvent, outputStack);
            var logging = new LoggingProcess(loggingQueue);
            var pricing = new PricingProcess();
            
            // Creating Threads
            Thread readerThread = new Thread(new ThreadStart(reader.Execute));
            Thread hedgeCalculationThread = new Thread(new ThreadStart(hedgeCalculation.Execute));
            Thread outputThread = new Thread(new ThreadStart(output.Execute));
            Thread loggingThread = new Thread(new ThreadStart(logging.Execute));
            Thread pricingThread = new Thread(new ThreadStart(pricing.Execute));

            readerThread.Start();
            hedgeCalculationThread.Start();
            outputThread.Start();
            loggingThread.Start();
            pricingThread.Start();
        }
    }
}