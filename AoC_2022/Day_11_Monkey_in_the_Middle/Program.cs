namespace day_11
{
    class MonkeyInTheMiddle
    {
        static Dictionary<long, Monkey> monkeys;
        static int worryReduction = 0;
        static void Main(string[] args)
        {
            worryReduction = 0;
            monkeys = GetMonkeys();
            FirstTask();

            worryReduction = 0;
            monkeys = GetMonkeys();
            SecondTask();
        }

        static void FirstTask()
        {
            for(long round = 0; round < 20; round++)
            {
                for(long monkeyId = 0; monkeyId < monkeys.Count; monkeyId++)
                {
                    monkeys[monkeyId].InspectAndThrow();
                }

            }

            List<long> inspectionCounters = new List<long>();
            foreach(Monkey monkey in monkeys.Values)
            {
                inspectionCounters.Add(monkey.GetInspectionCount());
            }

            inspectionCounters.Sort();

            Console.WriteLine("First task: " + (inspectionCounters[inspectionCounters.Count()-1] * inspectionCounters[inspectionCounters.Count() - 2]));
        }

        static void SecondTask()
        {
            for (long round = 0; round < 10000; round++)
            {
                for (long monkeyId = 0; monkeyId < monkeys.Count; monkeyId++)
                {
                    monkeys[monkeyId].InspectAndThrow(false);
                }

            }

            List<long> inspectionCounters = new List<long>();
            foreach (Monkey monkey in monkeys.Values)
            {
                inspectionCounters.Add(monkey.GetInspectionCount());
            }

            inspectionCounters.Sort();

            Console.WriteLine("Second task: " + (inspectionCounters[inspectionCounters.Count() - 1] * inspectionCounters[inspectionCounters.Count() - 2]));
        }

        static Dictionary<long, Monkey> GetMonkeys()
        {
            Dictionary<long, Monkey> monkeys = new Dictionary<long, Monkey>();

            int monkeyName = 0;
            List<long> items = new List<long>(); ;
            string monkeyOperation = "";
            int test = 0;
            int ifTrue = 0;
            int ifFalse = 0;
            int monkeyStep = 0;
            foreach ( string line in File.ReadLines("resources\\day_11_first-input.txt"))
            {

                if(monkeyStep == 0 && line.Trim().Length > 0)
                {
                    monkeyName = int.Parse(line.Trim().Split(" ")[1].Substring(0, line.Trim().Split(" ")[1].Length - 1));
                    items = new List<long>();
                    monkeyStep++;
                } else if (monkeyStep == 1)
                {
                    foreach (string item in line.Split(":")[1].Split(","))
                    {
                        items.Add(int.Parse(item.Trim()));
                    }
                    monkeyStep++;
                } else if (monkeyStep == 2)
                {
                    monkeyOperation = line.Split(":")[1].Trim();
                    monkeyStep++;
                } else if (monkeyStep == 3)
                {
                    test = int.Parse(line.Split("by")[1].Trim());
                    if (worryReduction == 0)
                    {
                        worryReduction = test;
                    }
                    else
                    {
                        worryReduction = worryReduction * test;
                    }
                    monkeyStep++;
                } else if (monkeyStep == 4)
                {
                    ifTrue = int.Parse(line.Split("monkey")[1].Trim());
                    monkeyStep++;
                } else if ( monkeyStep == 5)
                {
                    ifFalse = int.Parse(line.Split("monkey")[1].Trim());
                    monkeys.Add(monkeyName, new Monkey(items, monkeyOperation, test, ifTrue, ifFalse));
                    monkeyStep = 0;
                }
            }


            return monkeys;
        }

        class Monkey
        {
            private long inspectionCounter = 0;
            public readonly List<long> items;
            readonly string operation;
            readonly int testDivisibleBy;
            readonly int ifTrue;
            readonly int ifFalse;

            public Monkey(List<long> items,  string operation, int testDivisibleBy, int ifTrue, int ifFalse)
            {
                this.items = items;
                this.operation = operation;
                this.testDivisibleBy = testDivisibleBy;
                this.ifTrue = ifTrue;
                this.ifFalse = ifFalse;
            }

            public long GetInspectionCount()
            {
                return inspectionCounter;
            }

            public void ReceiveItem(long item)
            {
                items.Add(item);
            }

            public void InspectAndThrow()
            {
                InspectAndThrow(true);
            }

            public void InspectAndThrow(bool divideByThree)
            {
                if(items.Count == 0) {
                    return;
                }

                for(int i = 0; i < items.Count(); i++)
                {
                    inspectionCounter++;
                    long item = items[i];
                    long param1;
                    long param2;
                    string[] stuff = operation.Split(" ");

                    if (!(item % worryReduction == 0))
                    {
                        item = item % worryReduction;
                    }

                    if (stuff[2].Equals("old"))
                    {
                        param1 = item;
                    } else
                    {
                        param1 = long.Parse(stuff[2]);
                    }
                    if (stuff[4].Equals("old"))
                    {
                        param2 = item;
                    }
                    else
                    {
                        param2 = long.Parse(stuff[4]);
                    }
                    if (stuff[3].Equals("+"))
                    {
                        item = param1 + param2;
                    } else
                    {
                        item = param1 * param2;
                    }
                    if (divideByThree)
                    {
                        item = (long)item / 3;
                    }

                    bool isDivisibleBy = item % testDivisibleBy == 0;

                    monkeys[(isDivisibleBy ? ifTrue : ifFalse)].ReceiveItem(item);
                }

                items.Clear();
            }
        }
    }
}