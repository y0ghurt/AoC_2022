namespace day_16
{
    class ProboscideaVolcanium
    {
        static List<Path> paths = new List<Path>();
        static string filePath = "resources\\day_16_first-input.txt";
        static Dictionary<string, Valve> valves = new Dictionary<string, Valve>();
        static int highestPossibleReleasedPressure = 0;
        static int startTime = 30;
        static int startElephantTime = 0;

        static void Main(string[] args)
        {
            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            ParseInput();
            foreach (Valve valve in valves.Values)
            {
                valve.MapValves();
            }

            Path firstPath = new Path();
            firstPath.remainingTime = startTime;
            paths.Add(firstPath);
            for (bool done = false; !done;)
            {
                FindPaths();

                done = true;

                foreach (Path path in paths)
                {
                    if(path.remainingTime > 0)
                    {
                        done = false;
                    }
                }
            }

            Console.WriteLine("First task: " + highestPossibleReleasedPressure);
        }

        static void SecondTask()
        {
            valves.Clear();
            ParseInput();
            foreach (Valve valve in valves.Values)
            {
                valve.MapValves();
            }

            highestPossibleReleasedPressure = 0;
            paths.Clear();

            Path firstPath = new Path();
            startTime = 26;
            startElephantTime = 26;
            firstPath.remainingTime = startTime;
            firstPath.remainingElephantTime = startElephantTime;
            paths.Add(firstPath);
            for (bool done = false; !done;)
            {
                FindPaths();

                done = true;

                foreach (Path path in paths)
                {
                    if (path.remainingTime > 0 || path.remainingElephantTime > 0)
                    {
                        done = false;
                    }
                }
            }

            Console.WriteLine("Second task: " + highestPossibleReleasedPressure);
        }
        
        static void ParseInput()
        {
            string name;
            int flow;
            List<string> tunnels = new List<string>();
            
            foreach(string line in File.ReadLines(filePath)) 
            {
                name = line.Split(";")[0].Split(" ")[1];
                flow = int.Parse(line.Split(";")[0].Split("=")[1]);
                string temp = line.Split(";")[1];
                if (temp.Contains("valves"))
                {
                    foreach (string tunnel in temp.Split("valves")[1].Split(','))
                    {
                        tunnels.Add(tunnel.Trim());
                    }
                } else
                {
                    tunnels.Add(temp.Split("valve")[1].Trim());
                }
                valves.Add(name, new Valve(name, flow, new List<string>(tunnels)));
                tunnels.Clear();
            }
        }

        static void FindPaths()
        {
            for (int i = paths.Count() - 1; i >= 0; i--)
            {
                if (paths[i].Pathfind())
                {
                    i = paths.Count();
                }
            }
        }

        class Valve
        {
            public Dictionary<string, ValveReference> valveMap = new Dictionary<string, ValveReference>();
            public string name;
            public int flowRate;
            private List<string> connections;

            public Valve(string name, int flowRate, List<string> connections)
            {
                this.name = name;
                this.flowRate = flowRate;
                this.connections = connections;
            }

            public void MapValves()
            {
                int distance = 1;
                bool addedConnection = true;
                foreach (string connection in connections)
                {
                    if (!valveMap.ContainsKey(connection))
                    {
                        valveMap.Add(connection, new ValveReference(connection, valves[connection].flowRate, distance));
                    }
                    else if (distance < valveMap[connection].distance)
                    {
                        valveMap.Add(connection, new ValveReference(connection, valves[connection].flowRate, distance));
                    }
                }

                while (addedConnection)
                {
                    distance++;
                    addedConnection = false;
                    List<ValveReference> tempValveMap = new List<ValveReference>();

                    foreach (ValveReference vr in valveMap.Values)
                    {
                        foreach (string connection in valves[vr.name].connections)
                        {
                            if (connection.Equals(name)) continue;
                            if (!valveMap.ContainsKey(connection))
                            {
                                tempValveMap.Add(new ValveReference(connection, valves[connection].flowRate, distance));
                                addedConnection = true;
                            }
                            else if (distance < valveMap[connection].distance)
                            {
                                tempValveMap.Add(new ValveReference(connection, valves[connection].flowRate, distance));
                                addedConnection = true;
                            }
                        }
                    }
                    foreach(ValveReference tvr in tempValveMap)
                    {
                        if(!valveMap.ContainsKey(tvr.name))
                        {
                            valveMap.Add(tvr.name, tvr);
                        } else
                        {
                            valveMap[tvr.name] = tvr;
                        }
                    }
                }
            }

            public List<Tuple<string, int>> FindViableValves(Dictionary<string, ValveMetaData> openValves, int pointInTime)
            {
                List<Tuple<string, int>> viableValves = new List<Tuple<string, int>>();

                foreach(ValveReference vr in valveMap.Values)
                {
                    if (vr.flowRate > 0 && !openValves.Keys.Contains(vr.name) && vr.distance < (pointInTime - 1))
                    {
                        viableValves.Add(new Tuple<string, int>(vr.name, (vr.flowRate * (pointInTime - (vr.distance + 1)))));
                    }
                }
                return viableValves;
            }
        }
        
        struct ValveMetaData
        {
            public string valveResponsible;
            public int timeStamp;
        }

        class Path
        {
            public Dictionary<string, ValveMetaData> openValves = new Dictionary<string, ValveMetaData>();
            public string currentValve = "AA";
            public string currentElephantValve = "AA";
            public int remainingTime = 30;
            public int remainingElephantTime = 0;
            public int releasedPressure = 0;

            public Path()
            {

            }

            public Path(string currentValve, int releasedPressure, Dictionary<string, ValveMetaData> openValves)
            {
                this.currentValve = currentValve;
                this.releasedPressure = releasedPressure;
                this.openValves = openValves;
            }

            public bool Pathfind()
            {
                List<Tuple<string, int>> viablePaths;
                viablePaths = valves[currentValve].FindViableValves(openValves, remainingTime);
                if (viablePaths.Count() == 0)
                {
                    remainingTime = 0;
                }

                List<Tuple<string, int>> viableElephantPaths;
                viableElephantPaths = valves[currentElephantValve].FindViableValves(openValves, remainingElephantTime);
                if (viableElephantPaths.Count() == 0)
                {
                    remainingElephantTime = 0;
                }

                int imaginaryMaxReleasedPressure = releasedPressure;
                if (viableElephantPaths.Count() > 0 && viablePaths.Count() > 0)
                {
                    foreach (Tuple<string, int> vP in viablePaths)
                    {
                        Tuple<string, int> vEP;
                        for (int i = 0; i < viableElephantPaths.Count(); i++)
                        {
                            vEP = viableElephantPaths[i];
                            if (vP.Item1.Equals(vEP.Item1))
                            {
                                imaginaryMaxReleasedPressure += Math.Max(vP.Item2, vEP.Item2);
                            }
                        }
                    }
                }
                else if (viablePaths.Count() > 0)
                {
                    foreach (Tuple<string, int> vP in viablePaths)
                    {
                        imaginaryMaxReleasedPressure += vP.Item2;
                    }
                } else if (viableElephantPaths.Count() > 0)
                {
                    foreach (Tuple<string, int> vEP in viableElephantPaths)
                    {
                        imaginaryMaxReleasedPressure += vEP.Item2;
                    }
                }

                // Had to do this to shorten execution time - careful to not set the multiplier too low or it can potentially clear out the best run (I had it happen before).
                if (imaginaryMaxReleasedPressure * 1.2 <= highestPossibleReleasedPressure)
                {
                    paths.Remove(this);
                    return false;
                }

                if (remainingTime == 0 && remainingElephantTime == 0)
                {
                    if (releasedPressure > highestPossibleReleasedPressure)
                    {
                        if(releasedPressure == 1737 || releasedPressure == 1651 || releasedPressure == 1707)
                        {
                            foreach(KeyValuePair<string, ValveMetaData> kvp in openValves)
                            {
                                Console.WriteLine(kvp.Key + " " + kvp.Value.timeStamp + " " + kvp.Value.valveResponsible);
                            }
                        }
                        highestPossibleReleasedPressure = releasedPressure;
                    }
                    paths.Remove(this);
                    return false;
                }

                Path newPath;
                Path newElephantPath;
                if (remainingTime >= remainingElephantTime)
                {
                    foreach (Tuple<string, int> viablePath in viablePaths)
                    {
                        newPath = new Path(currentValve, releasedPressure + viablePath.Item2, new Dictionary<string, ValveMetaData> (openValves));
                        newPath.currentValve = viablePath.Item1;
                        newPath.remainingTime = remainingTime - (valves[currentValve].valveMap[viablePath.Item1].distance + 1);
                        newPath.remainingElephantTime = remainingElephantTime;
                        newPath.currentElephantValve = currentElephantValve;
                        newPath.openValves.Add(viablePath.Item1, new ValveMetaData { valveResponsible = "ME", timeStamp = startTime - newPath.remainingTime});

                        paths.Add(newPath);
                    }
                }
                else
                {
                    foreach (Tuple<string, int> viableElephantPath in viableElephantPaths)
                    {
                        newElephantPath = new Path(currentValve, releasedPressure + viableElephantPath.Item2, new Dictionary<string, ValveMetaData>(openValves));
                        newElephantPath.currentElephantValve = viableElephantPath.Item1;
                        newElephantPath.remainingTime = remainingTime;
                        newElephantPath.remainingElephantTime = remainingElephantTime - (valves[currentElephantValve].valveMap[viableElephantPath.Item1].distance + 1);
                        newElephantPath.openValves.Add(viableElephantPath.Item1, new ValveMetaData { valveResponsible = "ELEPHANT", timeStamp = startElephantTime - newElephantPath.remainingElephantTime});

                        paths.Add(newElephantPath);
                    }
                }


                paths.Remove(this);
                return true;
            }
        }
        
        class ValveReference
        {
            public string name;
            public int flowRate;
            public int distance;

            public ValveReference(string name, int flowRate, int distance)
            {
                this.name = name;
                this.flowRate = flowRate;
                this.distance = distance;
            }
        }
    }
}