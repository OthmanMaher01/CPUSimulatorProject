using System;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Simulator
{
    class Task : IComparable
    {
        public int id { get; }
        public int creationTime { get; }
        public int executionTime { get; }
        public int priority { get; }

        public Task(int id, int creationTime, int executionTime, int priority)
        {
            this.id = id;
            this.creationTime = creationTime;
            this.executionTime = executionTime;
            this.priority = priority;
        }

        public int CompareTo(object ob)
        {
            Task o = ob as Task;
            if (priority > o.priority)
            {
                return -1;
            }
            if (priority < o.priority)
            {
                return 1;
            }
            else
            {
                if (executionTime > o.executionTime)
                {
                    return -1;
                }
                if (executionTime < o.executionTime)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }


    }
}
