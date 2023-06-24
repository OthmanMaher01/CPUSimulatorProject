using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CPU_Simulator
{
    class Clock
    {
        private List<Processor> processors;
        private int maxCycles;
        private int cycles = 1;

        public Clock(List<Processor> processors, int maxCycles)
        {
            this.processors = processors;
            this.maxCycles = maxCycles;
        }

        public bool isRunning()
        {
            return cycles <= maxCycles;
        }

        public int getCycles()
        {
            return cycles;
        }

        async public void run()
        {

            while (isRunning())
            {
            
                    Thread.Sleep(1000);

                    Console.WriteLine($"At cycle {getCycles()}");
                    cycles++;

                //Notify the simulator so it can add newly created
                //tasks to the queue
                lock (processors)
                {
                    Monitor.Pulse(processors);
                }



            }


        }
    }
}
