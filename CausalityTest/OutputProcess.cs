using System;
using System.Threading;
using System.Collections.Generic;

namespace CausalityTest
{
    public class OutputProcess
    {
        public AutoResetEvent OutputEvent { get; protected set; }
        public Stack<string> OutputStack { get; protected set; }

        public OutputProcess(AutoResetEvent autoResetEvent, Stack<string> outputStack)
        {
            OutputEvent = autoResetEvent;
            OutputStack = outputStack;
        }

        public void Execute()
        {
            while (true)
            {
                OutputEvent.WaitOne();
                OutputEvent.Reset();

                var line = OutputStack.Pop();
                
                Console.WriteLine(line);
            }
        }
    }
}