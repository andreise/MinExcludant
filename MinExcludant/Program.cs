using System;
using System.Collections.Generic;

namespace MinExcludant
{

    sealed class MininumExcludantComputer
    {

        bool allowValueDuplicatesMode;

        public bool AllowValueDuplicatesMode { get { return this.allowValueDuplicatesMode; } }

        private readonly Dictionary<int, int> valueSet;

        private readonly Dictionary<int, int> mexHistory;

        private int currentMinimumExcludant;

        public int CurrentMinimumExcludant { get { return this.currentMinimumExcludant; } }

        public MininumExcludantComputer(bool allowValueDuplicatesMode, int capacity)
        {
            this.allowValueDuplicatesMode = allowValueDuplicatesMode;
            if (capacity < 0)
            {
                this.valueSet = new Dictionary<int, int>();
                this.mexHistory = new Dictionary<int, int>();
            }
            else
            {
                this.valueSet = new Dictionary<int, int>(capacity);
                this.mexHistory = new Dictionary<int, int>(capacity);
            }
        }

        public MininumExcludantComputer(bool allowValueDuplicatesMode) : this(allowValueDuplicatesMode, -1) { }

        public void Push(int value)
        {
            if (this.valueSet.ContainsKey(value))
            {
                if (this.allowValueDuplicatesMode)
                    this.valueSet[value]++;
                return;
            }

            this.valueSet.Add(value, 1);
        }

        public void Pop(int value)
        {
            if (!this.valueSet.ContainsKey(value))
                return;

            if (this.valueSet[value] > 1)
            {
                this.valueSet[value]--;
                return;
            }

            this.valueSet.Remove(value);
        }

    }

    static class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
