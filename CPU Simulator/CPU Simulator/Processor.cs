using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CPU_Simulator
{
    class Processor
    {

        private int id;
        private Clock clock;
        private int endCycle = 0;
        private Task workingTask;

        public Processor(int id, Clock clock)
        {
            this.id = id;
            this.clock = clock;
        }

        private bool isAvailable = true;
        public bool getAvailable()
        {
            return isAvailable;
        }

        public void assign(Task task, int finishCycle)
        {
            if (isAvailable)
            {
                isAvailable = false;
                endCycle = finishCycle;
                workingTask = task;
                Console.WriteLine($"assigning T{task.id} to P{id}");
            }

        }

        public void printProcessorsState(int clockCycle)
        {
            if (clockCycle == endCycle)
            {
                Console.WriteLine($"P{id}: T{workingTask.id} is finished");
                isAvailable = true;
                endCycle = 0;
                workingTask = null;
            }
            if (!isAvailable && workingTask != null)
            {
                Console.WriteLine($"P{id}: T{workingTask.id} is running");
            }
        }

        public void run() {

            while (clock.isRunning())
            {
                int clockCycle = clock.getCycles();

                //Waiting the simulator to notify the scheduler so the processor can work
                lock (clock) {
                    Monitor.Wait(clock);
                }

                printProcessorsState(clockCycle);

            }


        }

    }
}
