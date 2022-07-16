using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CausalityTest
{
    public class PricingProcess
    {
        public Dictionary<string, Array> Symbols { get; protected set; }
        public string DataPath { get; protected set; }

        public Random Change { get; protected set; }

        public PricingProcess()
        {
            Symbols = new Dictionary<string, Array>()
            {
                {"AAPL", new double[] {138.2, 10}},
                {"MSFT", new double[] {259.58, 30}},
                {"SPY", new double[] {381.24, -4}},
                {"BABA", new double[] {116, -15}}
            };
            DataPath = @".\Data\";
            Change = new Random();

        }

        public void Execute()
        {
            // Wait added to ensure the first result is based on the default values of the test
            Thread.Sleep(60000);
            
            while (true)
            {
                var heads = true;
                foreach (var sym in Symbols)
                {
                    // Changing Prices
                    var pricePath = DataPath + "prices.csv";

                    try
                    {
                        
                        if (heads)
                        {
                            using (var sw = new StreamWriter(pricePath, false))
                            {
                                heads = false;
                                var change = Change.Next(-100, 100);
                                var newPrice = Convert.ToDouble(sym.Value.GetValue(0)) * (1 + Convert.ToDouble(change) / 100);
                                // change = Change.Next(-100, 100);
                                // var newPosition = (int) Math.Round(Convert.ToDouble(sym.Value.GetValue(1)) * (1 + change/100));
                                sw.WriteLine("Ticker,Price");
                                sw.WriteLine(sym.Key + "," + newPrice);
                            }
                        }
                        else
                        {
                            using (var sw = new StreamWriter(pricePath, true))
                            {
                                var change = Change.Next(-100, 100);
                                var newPrice = Convert.ToDouble(sym.Value.GetValue(0)) * (1 + Convert.ToDouble(change) / 100);
                                sw.WriteLine(sym.Key + "," + newPrice);
                            }
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(Change.Next(1000, 5000));
                        continue;
                    }

                }

                var waitTime = Change.Next(1000, 5000);
                Thread.Sleep(waitTime);
            }
        }
    }
}