using System;
using System.Collections.Generic;
using System.Globalization;

namespace MinExcludant
{

    sealed class MininumExcludantComputer
    {

        private bool allowValueDuplicatesMode;

        public bool AllowValueDuplicatesMode
        {
            get { return this.allowValueDuplicatesMode; }
        }

        private readonly Dictionary<int, int> valueSet;

        private int currentMinimumExcludant;

        public int CurrentMinimumExcludant
        {
            get { return this.currentMinimumExcludant; }
        }

        public long LongCurrentMinimumExcludant
        {
            get { return unchecked((uint)this.currentMinimumExcludant); }
        }

        public MininumExcludantComputer(bool allowValueDuplicatesMode, int capacity)
        {
            this.allowValueDuplicatesMode = allowValueDuplicatesMode;
            if (capacity < 0)
                this.valueSet = new Dictionary<int, int>();
            else
                this.valueSet = new Dictionary<int, int>(capacity);
        }

        public MininumExcludantComputer(bool allowValueDuplicatesMode) : this(allowValueDuplicatesMode, -1)
        {
        }

        public void Push(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", value, "Value must be equals to or greater than zero.");

            if (this.valueSet.ContainsKey(value))
            {
                if (this.allowValueDuplicatesMode)
                    this.valueSet[value]++;
                return;
            }

            this.valueSet.Add(value, 1);

            if (value != this.currentMinimumExcludant)
                return;

            do
            {
                unchecked { this.currentMinimumExcludant++; }
            } while (this.valueSet.ContainsKey(this.currentMinimumExcludant));
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
            if (unchecked((uint)value) < unchecked((uint)this.currentMinimumExcludant))
                this.currentMinimumExcludant = value;
        }

    }

    static class Program
    {

        static int ParseNonnegativeInt32(string s)
        {
            return int.Parse(s, NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, NumberFormatInfo.InvariantInfo);
        }

        static int ReadNonnegativeInt32()
        {
            return ParseNonnegativeInt32(Console.ReadLine());
        }

        static int ReadIterations()
        {
            const int maxIterations = 150000;
            int iterations = ReadNonnegativeInt32();
            if (iterations > maxIterations)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Iteration count must be less than or equals to {0}.", maxIterations));
            return iterations;
        }

        static string[] SplitLine(string s, int maxCount)
        {
            string[] tempItems = s.Split(new char[] { '\u0020', '\u0009' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> items = new List<string>(Math.Min(tempItems.Length, maxCount));
            for (int i = 0; i < tempItems.Length && i < maxCount; i++)
                if (!string.IsNullOrWhiteSpace(tempItems[i]))
                    items.Add(tempItems[i]);
            return items.ToArray();
        }

        static Tuple<bool, int> ReadOperation()
        {
            const string addOperation = "+";
            const string removeOperation = "-";

            string s = Console.ReadLine();
            string[] items = SplitLine(s, 2);
            if (items.Length < 2)
                throw new ArgumentException("Two items per line was expected.");

            if (!(items[0] == addOperation || items[0] == removeOperation))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Operation code must be equals to '{0}' or '{1}'.", addOperation, removeOperation));

            return new Tuple<bool, int>(items[0] == addOperation, ParseNonnegativeInt32(items[1]));
        }

        static void Main(string[] args)
        {
            int iterations = ReadIterations();
            MininumExcludantComputer mexComputer = new MininumExcludantComputer(true, iterations);
            List<int> mexSequence = new List<int>(iterations);
            for (int i = 0; i < iterations; i++)
            {
                Tuple<bool, int> operation = ReadOperation();
                if (operation.Item1)
                    mexComputer.Push(operation.Item2);
                else
                    mexComputer.Pop(operation.Item2);
                mexSequence.Add(mexComputer.CurrentMinimumExcludant);
            }
            Console.WriteLine(string.Join("\u0020", mexSequence.ToArray()));
            //Console.ReadLine();
        }

    }
}
