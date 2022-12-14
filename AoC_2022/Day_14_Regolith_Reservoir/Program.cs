using System.Runtime.CompilerServices;

namespace day_14
{
    class RegolithReservoir
    {
        static string filePath = "resources\\day_14_first-input.txt";
        static Dictionary<Tuple<int, int>, string> scanMap = new Dictionary<Tuple<int, int>, string>();
        static int restingSand = 0;
        static int hMin = 500;
        static int hMax = 500;
        static int vMax = 0;

        static void Main(string[] args)
        {
            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            ScanRocks();
            SimulateSand();
            //DrawScanMap();
            Console.WriteLine("");

            Console.WriteLine("First task: " + restingSand);
        }

        static void SecondTask()
        {
            ScanRocks();
            vMax += 2;
            for(int h = 500 - (vMax + 5); h < (500 + vMax) + 5; h++)
            {
                if (h < hMin) hMin = h;
                if (h > hMax) hMax = h;
                scanMap.Add(new Tuple<int, int>(h, vMax), "#");
            }
            SimulateSand();
            //DrawScanMap();
            Console.WriteLine("Second task: " + restingSand);
        }

        static void SimulateSand()
        {
            int sandStartH = 500;
            int sandStartV = 0;

            int sandPositionH;
            int sandPositionV;

            restingSand = 0;

            bool isOverflowing = false;

            while(!isOverflowing)
            {
                sandPositionH = sandStartH;
                sandPositionV = sandStartV;

                while(1 == 1)
                {
                    if(sandPositionV > vMax || sandPositionH > hMax || sandPositionH < hMin)
                    {
                        isOverflowing= true;
                        break;
                    }

                    if(!scanMap.Keys.Contains(new Tuple<int, int>(sandPositionH, sandPositionV + 1))) {
                        sandPositionV++;
                    } else if (!scanMap.Keys.Contains(new Tuple<int, int>(sandPositionH - 1, sandPositionV + 1)))
                    {
                        sandPositionV++;
                        sandPositionH--;
                    }
                    else if (!scanMap.Keys.Contains(new Tuple<int, int>(sandPositionH + 1, sandPositionV + 1)))
                    {
                        sandPositionV++;
                        sandPositionH++;
                    } else
                    {
                        scanMap.Add(new Tuple<int, int>(sandPositionH, sandPositionV), "o");
                        restingSand++;
                        if (sandPositionH == sandStartH && sandPositionV == sandStartV)
                        {
                            isOverflowing = true;
                        }
                        break;
                    }

                }
            }
        }

        static void ScanRocks()
        {
            hMax = 500;
            hMin = 500;
            vMax = 0;
            scanMap = new Dictionary<Tuple<int, int>, string>();

            foreach (string line in File.ReadLines(filePath))
            {
                string[] nodes = line.Split(" -> ");

                for(int nodeIterator = 0; nodeIterator < nodes.Length - 1; nodeIterator++)
                {
                    string[] node1 = nodes[nodeIterator].Split(",");
                    string[] node2 = nodes[nodeIterator + 1].Split(",");

                    int temp = Math.Min(int.Parse(node1[0]), int.Parse(node2[0]));
                    hMin = temp < hMin ? temp : hMin;
                    temp = Math.Max(int.Parse(node1[0]), int.Parse(node2[0]));
                    hMax = temp > hMax ? temp : hMax;
                    temp = Math.Max(int.Parse(node1[1]), int.Parse(node2[1]));
                    vMax = temp > vMax ? temp : vMax;

                    if (!node1[1].Equals(node2[1]))
                    {
                        int horizontalCoordinate = int.Parse(node1[0]);
                        for (int verticalCoordinate = Math.Min(int.Parse(node1[1]), int.Parse(node2[1])); verticalCoordinate <= Math.Max(int.Parse(node1[1]), int.Parse(node2[1])); verticalCoordinate++) {
                            if (!scanMap.Keys.Contains(new Tuple<int, int> (horizontalCoordinate, verticalCoordinate)))
                            {
                                scanMap.Add(new Tuple<int, int>(horizontalCoordinate, verticalCoordinate), "#");
                            }
                        }
                    } 
                    else if (!node1[0].Equals(node2[0])) 
                    {
                        int verticalCoordinate = int.Parse(node1[1]);
                        for (int horizontalCoordinate = Math.Min(int.Parse(node1[0]), int.Parse(node2[0])); horizontalCoordinate <= Math.Max(int.Parse(node1[0]), int.Parse(node2[0])); horizontalCoordinate++)
                        {
                            if (!scanMap.Keys.Contains(new Tuple<int, int>(horizontalCoordinate, verticalCoordinate)))
                            {
                                scanMap.Add(new Tuple<int, int>(horizontalCoordinate, verticalCoordinate), "#");
                            }
                        }
                    }
                    else
                    {
                        if (!scanMap.Keys.Contains(new Tuple<int, int>(int.Parse(node1[0]), int.Parse(node1[1]))))
                        {
                            scanMap.Add(new Tuple<int, int>(int.Parse(node1[0]), int.Parse(node1[1])), "#");
                        }
                    }
                }
            }
        }

        static void DrawScanMap()
        {
            for(int v = 0; v <= vMax; v++)
            {
                string line = "";
                for(int h = hMin - 1; h <= hMax; h++) 
                { 
                    if(scanMap.Keys.Contains(new Tuple<int, int> (h, v)))
                    {
                        line += scanMap[new Tuple<int, int>(h, v)];
                    } else
                    {
                        line += ".";
                    }
                }
                Console.WriteLine(line);
            }
        }
    }
}