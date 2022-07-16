using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace CausalityTest
{
    public class ReaderProcess
    {
        public ConcurrentDictionary<string, Array> PosInfo { get; protected set; }
        public string DataPath { get; protected set; }

        public ReaderProcess(ConcurrentDictionary<string, Array> posInfo, string dataPath)
        {
            PosInfo = posInfo;
            DataPath = dataPath;
        }

        public void Execute()
        {
            while (true)
            {
                // Reading Prices
                using (StreamReader sr = new StreamReader(DataPath + "\\prices.csv"))
                {
                    string currentLine;
                    string heads = "";
                
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (heads.Equals(""))
                        {
                            heads = currentLine;
                        }
                        else
                        {
                            var currentLineArray = currentLine.Split(',');
                            // 0 index will be the key
                            // 1 index will be the value
                            PosInfo.AddOrUpdate(currentLineArray[0],
                                new double[] {Convert.ToDouble(currentLineArray[1]), 0.0, 0.0},
                                (key, valuesArray) =>
                                {
                                    valuesArray.SetValue(Convert.ToDouble(currentLineArray[1]), 0);

                                    return valuesArray;
                                }
                            );
                        }
                    }
                }
                
                // Reading Positions
                using (StreamReader sr = new StreamReader(DataPath + "\\positions.csv"))
                {
                    string currentLine;
                    string heads = "";
                
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (heads.Equals(""))
                        {
                            heads = currentLine;
                        }
                        else
                        {
                            var currentLineArray = currentLine.Split(',');
                            // 0 index will be the key
                            // 1 index will be the value
                            PosInfo.AddOrUpdate(currentLineArray[0],
                                new double[] {0.0, Convert.ToDouble(currentLineArray[1]), 0.0},
                                (key, valuesArray) =>
                                {
                                    valuesArray.SetValue(Convert.ToDouble(currentLineArray[1]), 1);

                                    return valuesArray;
                                }
                            );
                        }
                    }
                }
                
                // Reading Betas
                using (StreamReader sr = new StreamReader(DataPath + "\\betas.csv"))
                {
                    string currentLine;
                    string heads = "";
                
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (heads.Equals(""))
                        {
                            heads = currentLine;
                        }
                        else
                        {
                            var currentLineArray = currentLine.Split(',');
                            // 0 index will be the key
                            // 1 index will be the value
                            PosInfo.AddOrUpdate(currentLineArray[0],
                                new double[] {0.0, 0.0, Convert.ToDouble(currentLineArray[1])},
                                (key, valuesArray) =>
                                {
                                    valuesArray.SetValue(Convert.ToDouble(currentLineArray[1]), 2);

                                    return valuesArray;
                                }
                            );
                        }
                    }
                }
                
                // Console.WriteLine("Sleeping 1 second from Reader");
                Thread.Sleep(1000);
            }

        }
    }
}