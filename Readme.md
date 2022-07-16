# Causality Group - Execution Developer Test
### Developer: Luis Gamarra

---
## Assumptions
* The prices and positions will be updated by the csv file every "n" milliseconds
* There could be new positions on the next csv file received, on additional symbols
* The price reading and the calculations will be in separated Threads for parallel processing
* Per every symbol will be an stack to save the most recent price
* The csv file will be read row by row since is likely that the number of positions wont pass the thousands
* The program that updates the CSV handles the possible error if the CSV if being used by the module
* The SPY position is only used by hedging
* The Program will be synchronized to start at 00 or 30 seconds to have a better understanding of when it is calculated
* Is better to separate the calculations from the output and logging handling because writing on the disk and gui operations could be time consuming
* There will be an output and logging Thread, the output will be activated right after the calculations are made and the logging will be activated every 120 seconds  
* Logging is important for troubleshooting the program
* I'll will add a producer Thread to visualize and test the program, this Thread will be executed only when the argument "test" is passed to the program
* I'll use the Object Oriented Programming 
* Position should be integer numbers unless the broker allows fractional positions
* I'm using a Concurrent dictionary to save the csv data in memory for the following reasons:
  * Can be accessed by multiple Threads and it's managed by the framework automatically to avoid human errors using manual locks
  * Since it's values are only modified in one of the threads them is safe to use it in this scenario
  * When a new symbol comes from the csv is easy to create a new key, complexity of O(n), where n is the number of symbols
  * Retrieving a value knowing the symbol name is very fast, It has a complexity of O(1)
* Since it is not know if the producer of data have a way to notify the solution there is a new CSV file the solution will try to read from the csv every second
* I'm assuming that the total hedge position is the summary of all partial hedges by every individual stock

## Flow Diagram
![alt text](FlowDiagram.png?raw=true "")