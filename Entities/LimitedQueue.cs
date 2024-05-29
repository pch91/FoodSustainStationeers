using FoodSustain.Config;
using System;
using System.Collections.Generic;

namespace FoodSustain.Entities
{
    [Serializable]
    internal class LimitedQueue<T> : Queue<T>
    {
        private readonly int limit;

        public LimitedQueue(int limit)
            : base(limit)
        {
            this.limit = limit;
        }

        public LimitedQueue() {
            this.limit = Configs.getconfigs<int>("intFoodStorage");
        }

        public new void Enqueue(T item)
        {
            while (Count >= limit)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }

        public int getLimit()
        {
            return limit;
        }
    }
}
