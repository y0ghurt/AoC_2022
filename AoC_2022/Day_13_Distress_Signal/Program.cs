using System.Data;

namespace day_13
{
    class DistressSignal
    {
        static List<Tuple<DataList, DataList>> pairs = new List<Tuple<DataList, DataList>>();
        static List<DataList> packets = new List<DataList>();
        static int integer;
        static Type intType = typeof(int);
        static string filePath = "resources\\day_13_first-input.txt";

        static void Main(string[] args)
        {
            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            GetPairs();
            int indexSum = 0;

            for(int i = 0; i < pairs.Count(); i++)
            {
                TFM tfm = ComparePair(pairs[i].Item1, pairs[i].Item2);
                if (tfm == TFM.True || tfm == TFM.Maybe)
                {
                    indexSum += (i + 1);
                }
            }

            Console.WriteLine("First task: " + indexSum);
        }

        static void SecondTask()
        {
            GetLines();
            DataList dlTwo = new DataList("[[2]]");
            DataList dlSix = new DataList("[[6]]");
            packets.Add(dlTwo);
            packets.Add(dlSix);
            bool swapped = true;
            while(swapped)
            {
                swapped = false;
                for(int i = 0; i < packets.Count() - 1; i++)
                {
                    DataList current = packets[i];
                    DataList next = packets[i + 1];
                    TFM result = ComparePair(current, next);
                    if (result == TFM.False)
                    {
                        SwapPackets(packets.IndexOf(current), packets.IndexOf(next));
                        swapped = true;
                    }
                }
            }
            Console.WriteLine("Second task: " + ((packets.IndexOf(dlTwo) + 1) * (packets.IndexOf(dlSix) + 1)));
        }

        static void GetPairs()
        {
            int row = 0;
            string[] lines = File.ReadAllLines(filePath);
            while(row < lines.Length)
            {
                if (lines[row].Length == 0)
                {
                    row++;
                    continue;
                }

                pairs.Add(new Tuple<DataList, DataList>(new DataList(lines[row]), new DataList(lines[row+1])));
                row += 2;
            }
        }

        static void SwapPackets(int firstIndex, int secondIndex)
        {
            DataList tmpDL = packets[firstIndex];
            packets[firstIndex] = packets[secondIndex];
            packets[secondIndex] = tmpDL;
        }

        static void GetLines()
        {
            foreach (string line in File.ReadLines(filePath))
            {
                if (line.Length > 0)
                {
                    packets.Add(new DataList(line));
                }
            }
        }

        static TFM ComparePair(DataList left, DataList right)
        {
            for(int i = 0; 1 == 1; i++)
            {
                object leftObject = left.GetObject(i);
                object rightObject = right.GetObject(i);

                if(leftObject == null && rightObject != null)
                {
                    return TFM.True;
                }

                if(leftObject != null && rightObject == null) 
                {
                    return TFM.False;
                }

                if(leftObject == null && rightObject == null)
                {
                    return TFM.Maybe;
                }

                Type leftType = leftObject.GetType();
                Type rightType = rightObject.GetType();

                if (leftType.Equals(intType) && rightType.Equals(intType)) {
                    if ((int)leftObject > (int)rightObject) return TFM.False;
                    if ((int)leftObject < (int)rightObject) return TFM.True;
                    continue;
                }

                if (leftType.Equals(intType)) leftObject = new DataList("[" + (int)leftObject + "]");
                if (rightType.Equals(intType)) rightObject = new DataList("[" + (int)rightObject + "]");

                TFM tfm = ComparePair((DataList)leftObject, (DataList)rightObject);
                if(tfm == TFM.False || tfm == TFM.True)
                {
                    return tfm;
                }
            }
        }

        enum TFM
        {
            True = 1, False = 0, Maybe = 2
        }

        class DataList
        {
            private Dictionary<int, DataList> lists = new Dictionary<int, DataList>();
            private Dictionary<int, int> dataPoints = new Dictionary<int, int>();
            private int maxId = 0;

            public DataList(string listDefinition)
            {
                int nextId = 0;
                int indentationLevel = 0;
                string sublist = "";
                string number = "";
                foreach (char c in listDefinition)
                {
                    if (c == '[')
                    {
                        indentationLevel++;
                    }
                    if (indentationLevel == 1)
                    {
                        if (Char.IsDigit(c))
                        {
                            number += c;
                        } else if ((c == ',' || c == ']') && number.Length > 0)
                        {
                            dataPoints.Add(nextId, int.Parse(number));
                            nextId++;
                            maxId++;
                            number = "";
                        }
                    } else
                    {
                        if (c == ']')
                        {
                            sublist += c;
                            indentationLevel--;
                        }
                        if (indentationLevel == 1)
                        {
                            lists.Add(nextId, new DataList(sublist));
                            nextId++;
                            maxId++;
                            sublist = "";
                        } else
                        {
                            sublist += c;
                        }
                    }
                }
            }

            public object GetObject(int index)
            {
                if (index > maxId) return null;
                if (lists.Keys.Contains(index)) return lists[index];
                if (dataPoints.Keys.Contains(index)) return dataPoints[index];
                return null;
            }
        }
    }
}