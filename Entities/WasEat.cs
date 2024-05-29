using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSustain.Entities
{
    [Serializable]
    internal class WasEat
    {
        public readonly ulong player;

        public int lastCount { get; set; } = 0;

        public LimitedQueue<long> foodseat {  get; set; }

        public WasEat()
        {
            this.player = new ulong();
            foodseat = new LimitedQueue<long>();
        }

        public WasEat(ulong player)
        {
            this.player = player;
            foodseat = new LimitedQueue<long>();
        }
        public WasEat(ulong player, long firstItem )
        {
            this.player = player;
            foodseat = new LimitedQueue<long>();
            foodseat.Enqueue( firstItem );
        }
    }
}
