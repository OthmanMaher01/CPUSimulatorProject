using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPU_Simulator
{
    class Scheduler
    {
        private List<Processor> processors;
        private PriorityQueue<Task> tasksQueue;
        private Clock clock;

        public Scheduler(List<Processor> processors, PriorityQueue<Task> tasksQueue, Clock clock)
        {
            this.processors = processors;
            this.tasksQueue = tasksQueue;
            this.clock = clock;
        }

        public void assignTasks(int cycle)
        {
            foreach (var processor in processors)
            {
                if (processor.getAvailable() && !tasksQueue.isEmpty())
                {
                    Task assignedTask = tasksQueue.Dequeue();
                    processor.assign(assignedTask, cycle + assignedTask.executionTime - 1);
                }
            }
        }

        public void run()
        {
            while (clock.isRunning())
            {
                int cycle = clock.getCycles();

                // waiting the simulator to notify the scheduler to assign the tasks to the processors
                lock (tasksQueue) {
                    Monitor.Wait(tasksQueue);
                }

                assignTasks(cycle);
            }

        }
    }
}
