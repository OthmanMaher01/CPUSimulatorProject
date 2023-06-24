using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPU_Simulator
{
    class Simulator
    {
        private int numberOfProcessors;
        private int numberOfClocks;
        private string filePath;
        private List<Processor> processors = new List<Processor>();
        private List<Task> tasks = new List<Task>();
        private Clock clock;
        private Scheduler scheduler;
        private PriorityQueue<Task> tasksQueue = new PriorityQueue<Task>();

        public Simulator(int numberOfProcessors, int numberOfClocks, String filePath)
        {
            this.numberOfProcessors = numberOfProcessors;
            this.numberOfClocks = numberOfClocks;
            this.filePath = filePath;

            clock = new Clock(processors, numberOfClocks);
            scheduler = new Scheduler(processors, tasksQueue, clock);

            saveTasks();
        }

        public void createProcessors()
        {
            int processorId = 1;
            for (int i = 0; i < numberOfProcessors; i++)
            {
                Processor processor = new Processor(processorId, clock);
                Thread thread = new Thread(new ThreadStart(processor.run));
                thread.Start();
                processors.Add(processor);
                processorId++;
            }
        }

        public void saveTasks() {
            String data;
            StreamReader reader = new StreamReader(filePath);
            int idCounter = 1;


            try
            {
                reader.ReadLine();
                data = reader.ReadLine();
                while (data != null)
                {
                    List<string> list = data.Split().ToList();

                    int creationTime = Convert.ToInt32(list[0]);
                    int executionTime = Convert.ToInt32(list[1]);
                    int priority = Convert.ToInt32(list[2]);

                    Task task = new Task(idCounter, creationTime, executionTime, priority);
                    tasks.Add(task);
                    data = reader.ReadLine();
                 idCounter++;

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally {
                reader.Close();
            }
        
        }

        public void startingThreads()
        {
            Thread clockThread = new Thread(new ThreadStart(clock.run));
            createProcessors();
            Thread schedulerThread = new Thread(new ThreadStart(scheduler.run));
            clockThread.Start();
            schedulerThread.Start();
        }

        public void addingTasksToQueue()
        {
            int clockId = clock.getCycles();
            foreach (var task in tasks)
            {
                if (task.creationTime <= clockId)
                {
                    Console.WriteLine($"T{task.id} craeted and added to the queue");
                    tasksQueue.Enqueue(task);
                }
            }
            tasks.RemoveAll(task => task.creationTime <= clockId);
        }

        public void start() {

            startingThreads();

            while (clock.isRunning())
            {
                //Waiting the Clock thread to notify the simulator so it can add newly created
                //tasks to the queue
                lock (processors)
                {
                    Monitor.Wait(processors);
                }

                addingTasksToQueue();

                // notify the scheduler so it can assign tasks to processors
                lock (tasksQueue) {
                    Monitor.Pulse(tasksQueue);
                }

                // notify all the waiting processors
                lock (clock)
                {
                    Monitor.PulseAll(clock);

                }

            }
        }

    }
}
