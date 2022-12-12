using System.Linq;
using System.Text;

namespace day_12
{
    class HillClimbingAlgorithm
    {
        static int horizontalStartPosition = 0;
        static int verticalStartPosition = 0;
        static int horizontalEndPosition = 0;
        static int verticalEndPosition = 0;
        static Dictionary<int, Dictionary<int, int>> heightMap = new Dictionary<int, Dictionary<int, int>>();
        static Dictionary<int, Dictionary<int, int>> pathLengthMap = new Dictionary<int, Dictionary<int, int>>();
        static List<Probe> probes = new List<Probe>();

        static void Main(string[] args)
        {
            ParseHeightMap();

            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            Probe probe = new Probe(verticalStartPosition, horizontalStartPosition);
            probes.Add(probe);
            probe.Explore();

            Dictionary<Dictionary<Tuple<int, int>, string>, int> enders = new Dictionary<Dictionary<Tuple<int, int>, string>, int>();
            foreach(Probe ender in probes)
            {
                enders.Add(ender.GetVisitedLocations(), ender.GetVisitedLocations().Count());
            }

            Console.WriteLine("First task: " + (enders.Values.Min() - 1));
        }

        static void SecondTask()
        {
            probes.Clear();
            for(int v = 0; v < heightMap.Count(); v++)
            {
                for(int h = 0; h < heightMap[v].Count(); h++)
                {
                    if (heightMap[v][h] == Encoding.ASCII.GetBytes(new char[] { 'a' })[0])
                    {
                        Probe probe = new Probe(v, h);
                        probes.Add(probe);
                        probe.Explore();
                    }
                }
            }

            Dictionary<Dictionary<Tuple<int, int>, string>, int> enders = new Dictionary<Dictionary<Tuple<int, int>, string>, int>();
            foreach (Probe ender in probes)
            {
                enders.Add(ender.GetVisitedLocations(), ender.GetVisitedLocations().Count());
            }

            Console.WriteLine("Second task: " + (enders.Values.Min() - 1));
        }

        static void ParseHeightMap()
        {
            int verticalPosition = 0;
            foreach (string line in File.ReadLines("resources\\day_12_first-input.txt"))
            {
                int horizontalPosition = 0;
                heightMap.Add(verticalPosition, new Dictionary<int, int>());
                pathLengthMap.Add(verticalPosition, new Dictionary<int, int>());
                foreach(byte b in Encoding.ASCII.GetBytes(line.ToCharArray()))
                {
                    if (line[horizontalPosition] != 'E' && line[horizontalPosition] != 'S')
                    {
                        heightMap[verticalPosition].Add(horizontalPosition, b);
                    } else if(line[horizontalPosition] == 'E') {
                        horizontalEndPosition = horizontalPosition;
                        verticalEndPosition = verticalPosition;
                        heightMap[verticalPosition].Add(horizontalPosition, Encoding.ASCII.GetBytes(new char[] { 'z' })[0]);
                    }
                    else if (line[horizontalPosition] == 'S')
                    {
                        horizontalStartPosition = horizontalPosition;
                        verticalStartPosition = verticalPosition;
                        heightMap[verticalPosition].Add(horizontalPosition, Encoding.ASCII.GetBytes(new char[] { 'a' })[0]);
                    }
                    pathLengthMap[verticalPosition].Add(horizontalPosition, int.MaxValue);
                    horizontalPosition++;
                }
                verticalPosition++;
            }
        }

        class Probe
        {
            private int verticalPosition;
            private int horizontalPosition;
            private bool foundEnd = false;

            Dictionary<Tuple<int, int>, string> visitedLocations;

            public Probe(int verticalPosition, int horizontalPosition)
            {
                this.horizontalPosition = horizontalPosition;
                this.verticalPosition = verticalPosition;
                this.visitedLocations = new Dictionary<Tuple<int, int>, string>();
                visitedLocations.Add(new Tuple<int, int>(verticalPosition, horizontalPosition), StringifyPosition(verticalPosition, horizontalPosition));
                if (verticalPosition == verticalEndPosition && horizontalPosition == horizontalEndPosition)
                {
                    foundEnd = true;
                }
            }

            public Probe(int verticalPosition, int horizontalPosition, Dictionary<Tuple<int, int>, string> visitedLocations)
            {
                this.horizontalPosition = horizontalPosition;
                this.verticalPosition = verticalPosition;
                this.visitedLocations = new Dictionary<Tuple<int, int>, string>();
                this.visitedLocations = new Dictionary<Tuple<int, int>, string>(visitedLocations);
                this.visitedLocations.Add(new Tuple<int, int>(verticalPosition, horizontalPosition), StringifyPosition(verticalPosition, horizontalPosition));
                if(verticalPosition == verticalEndPosition && horizontalPosition == horizontalEndPosition)
                {
                    foundEnd = true;
                }
            }

            public Dictionary<Tuple<int, int>, string> GetVisitedLocations()
            {
                return visitedLocations;
            }

            public void Explore()
            {
                if(foundEnd) return;

                // Travel west.
                if (horizontalPosition > 0)
                {
                    if (!visitedLocations.Keys.Contains(new Tuple<int, int>(verticalPosition, horizontalPosition - 1)))
                    {
                        if(visitedLocations.Count() < pathLengthMap[verticalPosition][horizontalPosition - 1])
                        {
                            if (heightMap[verticalPosition][horizontalPosition - 1] <= (heightMap[verticalPosition][horizontalPosition] + 1))
                            {
                                pathLengthMap[verticalPosition][horizontalPosition - 1] = visitedLocations.Count();
                                Probe probe = new Probe(verticalPosition, horizontalPosition - 1, visitedLocations);
                                probes.Add(probe);
                                probe.Explore();
                            }
                        }
                    }
                }
                // Travel east.
                if (horizontalPosition < heightMap[verticalPosition].Count() - 1)
                {
                    if (!visitedLocations.Keys.Contains(new Tuple<int, int>(verticalPosition, horizontalPosition + 1)))
                    {
                        if (visitedLocations.Count() < pathLengthMap[verticalPosition][horizontalPosition + 1])
                        {
                            if (heightMap[verticalPosition][horizontalPosition + 1] <= (heightMap[verticalPosition][horizontalPosition] + 1))
                            {
                                pathLengthMap[verticalPosition][horizontalPosition + 1] = visitedLocations.Count();
                                Probe probe = new Probe(verticalPosition, horizontalPosition + 1, visitedLocations);
                                probes.Add(probe);
                                probe.Explore();
                            }
                        }
                    }
                }
                // Travel north.
                if (verticalPosition > 0) {
                    if (!visitedLocations.Keys.Contains(new Tuple<int, int>(verticalPosition - 1, horizontalPosition)))
                    {
                        if (visitedLocations.Count() < pathLengthMap[verticalPosition - 1][horizontalPosition])
                        {
                            if (heightMap[verticalPosition - 1][horizontalPosition] <= (heightMap[verticalPosition][horizontalPosition] + 1))
                            {
                                pathLengthMap[verticalPosition - 1][horizontalPosition] = visitedLocations.Count();
                                Probe probe = new Probe(verticalPosition - 1, horizontalPosition, visitedLocations);
                                probes.Add(probe);
                                probe.Explore();
                            }
                        }
                    }
                }
                // Travel south.
                if (verticalPosition < heightMap.Count() - 1) {
                    if (!visitedLocations.Keys.Contains(new Tuple<int, int>(verticalPosition + 1, horizontalPosition)))
                    {
                        if (visitedLocations.Count() < pathLengthMap[verticalPosition + 1][horizontalPosition])
                        {
                            if (heightMap[verticalPosition + 1][horizontalPosition] <= (heightMap[verticalPosition][horizontalPosition] + 1))
                            {
                                pathLengthMap[verticalPosition + 1][horizontalPosition] = visitedLocations.Count();
                                Probe probe = new Probe(verticalPosition + 1, horizontalPosition, visitedLocations);
                                probes.Add(probe);
                                probe.Explore();
                            }
                        }
                    }
                }
                probes.Remove(this);
            }

            public string StringifyPosition(int verticalPosition, int horizontalPosition)
            {
                return verticalPosition + "-" + horizontalPosition;
            }
        }
    }
}