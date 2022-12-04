namespace day_04
{
    class CampCleanup
    {
        static void Main(string[] args)
        {
            List<List<Range>> rangePairs = GetRangePairs();

            FirstTask(rangePairs);
            SecondTask(rangePairs);
        }

        static void FirstTask(List<List<Range>> rangePairs)
        {
            int containedAssignments = 0;
            foreach (List<Range> rangePair in rangePairs)
            {
                int contained = 0;
                int pairCounter = 0;
                foreach(Range range in rangePair)
                {
                    for(int otherPairCounter = 0; otherPairCounter < rangePair.Count; otherPairCounter++)
                    {
                        Range otherRange = rangePair[otherPairCounter];
                        if(otherPairCounter != pairCounter)
                        {
                            if(range.low <= otherRange.low && range.high >= otherRange.high)
                            {
                                contained = 1;
                            }
                        }
                    }
                    pairCounter++;
                }
                containedAssignments += contained;
            }

            Console.WriteLine("First task: " + containedAssignments);
        }

        static void SecondTask(List<List<Range>> rangePairs)
        {
            int overlappingAssignments = 0;
            foreach (List<Range> rangePair in rangePairs)
            {
                int overlap = 0;
                int pairCounter = 0;
                foreach (Range range in rangePair)
                {
                    for (int otherPairCounter = 0; otherPairCounter < rangePair.Count; otherPairCounter++)
                    {
                        Range otherRange = rangePair[otherPairCounter];
                        if (otherPairCounter != pairCounter)
                        {
                            if (range.low <= otherRange.high && range.low >= otherRange.low ||
                                range.high <= otherRange.high && range.high >= otherRange.low)
                            {
                                overlap = 1;
                            }
                        }
                    }
                    pairCounter++;
                }
                overlappingAssignments += overlap;
            }
            Console.WriteLine("Second task: " + overlappingAssignments);
        }

        static List<List<Range>> GetRangePairs()
        {
            List<List<Range>> rangePairs = new List<List<Range>>();
            foreach (string line in File.ReadLines(@"resources\day_04_first-input.txt"))
            {
                List<Range> rangePair = new List<Range>();
                string[] rangeStrings = line.Split(',');
                foreach (string rangeString in rangeStrings)
                {
                    string[] lowHighString = rangeString.Split("-");
                    rangePair.Add(new Range(int.Parse(lowHighString[0]), int.Parse(lowHighString[1])));
                }
                rangePairs.Add(rangePair);
            }
            return rangePairs;
        }


        struct Range
        {
            public readonly int low;
            public readonly int high;

            public Range(int low, int high)
            {
                this.low = low;
                this.high = high;
            }

            public override string ToString()
            {
                return low + "-" + high;
            }
        }
    }
}