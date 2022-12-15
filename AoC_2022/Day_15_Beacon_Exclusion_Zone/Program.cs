namespace day_15
{
    class BeaconExclusionZone
    {
        static List<Sensor> sensors = new List<Sensor>();
        static int merged = 0;

        static string filePath = "resources\\day_15_first-input.txt";

        static void Main(string[] args)
        {
            foreach(string line in File.ReadLines(filePath))
            {
                sensors.Add(new Sensor(line));
            }

            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            int row = 2000000;

            List<Tuple<Coordinate, Coordinate>> coordinateLines = new List<Tuple<Coordinate, Coordinate>>();
            
            foreach(Sensor sensor in sensors)
            {
                if(sensor.GetRowCoverage(row) != null)
                {
                    coordinateLines.Add(sensor.GetRowCoverage(row));
                }
            }

            for(int i = 0; i < coordinateLines.Count(); i++)
            {
                Tuple<Coordinate, Coordinate> oneCoordinateLine = coordinateLines[i];
                for(int j = 0; j < coordinateLines.Count(); j++)
                {
                    if (i == j) continue;

                    Tuple<Coordinate, Coordinate> anotherCoordinateLine = coordinateLines[j];

                    if (oneCoordinateLine.Item1.hLoc <= anotherCoordinateLine.Item2.hLoc
                        && oneCoordinateLine.Item2.hLoc >= anotherCoordinateLine.Item1.hLoc)
                    {

                        coordinateLines.Add(MergeCoordinates(oneCoordinateLine, anotherCoordinateLine));
                        coordinateLines.Remove(oneCoordinateLine);
                        coordinateLines.Remove(anotherCoordinateLine);
                        i = -1;
                        break;
                    }
                }
            }

            int noBeacons = 0;
            foreach(Tuple<Coordinate, Coordinate> coordinateLine in coordinateLines)
            {
                noBeacons += coordinateLine.Item2.hLoc - coordinateLine.Item1.hLoc;
            }

            Console.WriteLine("First task: " + noBeacons);
        }
        static void SecondTask()
        {

            int row = 0;
            List<Tuple<Coordinate, Coordinate>> coordinateLines = new List<Tuple<Coordinate, Coordinate>>();
            int hLoc = 0;
            int vLoc = 0;

            while (row <= 4000000)
            {
                coordinateLines.Clear();
                foreach (Sensor sensor in sensors)
                {
                    if (sensor.GetRowCoverage(row) != null)
                    {
                        coordinateLines.Add(sensor.GetRowCoverage(row));
                    }
                }

                for (int i = 0; i < coordinateLines.Count(); i++)
                {
                    Tuple<Coordinate, Coordinate> oneCoordinateLine = coordinateLines[i];
                    for (int j = 0; j < coordinateLines.Count(); j++)
                    {
                        if (i == j) continue;

                        Tuple<Coordinate, Coordinate> anotherCoordinateLine = coordinateLines[j];

                        if (oneCoordinateLine.Item1.hLoc <= anotherCoordinateLine.Item2.hLoc
                            && oneCoordinateLine.Item2.hLoc >= anotherCoordinateLine.Item1.hLoc)
                        {

                            coordinateLines.Add(MergeCoordinates(oneCoordinateLine, anotherCoordinateLine));
                            coordinateLines.Remove(oneCoordinateLine);
                            coordinateLines.Remove(anotherCoordinateLine);
                            i = -1;
                            break;
                        }
                    }
                }
                if (coordinateLines.Count > 1)
                {
                    hLoc = (Math.Min(coordinateLines[0].Item2.hLoc, coordinateLines[1].Item2.hLoc) + 1);
                    vLoc = row;

                    if (hLoc >= 0 && hLoc <= 4000000)
                    {
                        break;
                    }
                }
                row++;
            }

            Console.WriteLine("Second task: " + (long)(((long)hLoc * 4000000) + vLoc));
        }

        static Tuple<Coordinate, Coordinate> MergeCoordinates(Tuple<Coordinate, Coordinate> c1, Tuple<Coordinate, Coordinate> c2)
        {
            int hLocLeft = Math.Min(c1.Item1.hLoc, c2.Item1.hLoc);
            int hLocRight = Math.Max(c1.Item2.hLoc, c2.Item2.hLoc);
            int vLoc = c1.Item1.vLoc;
            Tuple<Coordinate, Coordinate> output = new Tuple<Coordinate, Coordinate>(new Coordinate(hLocLeft, vLoc), new Coordinate(hLocRight, vLoc));
            merged++;
            return output;
        }
    }

    class Sensor
    {
        public readonly Coordinate coordinate;
        public readonly Coordinate nearestBeacon;
        public readonly int range;

        public Sensor(string input)
        {
            coordinate = new Coordinate();
            nearestBeacon = new Coordinate();

            string[] split = input.Split(':');
            string[] sensorCoordinates = split[0].Split(",");
            string[] beaconCoordinates = split[1].Split(",");

            string[] hLocStrings = sensorCoordinates[0].Split("=");
            string[] vLocStrings = sensorCoordinates[1].Split("=");
            coordinate.hLoc = int.Parse(hLocStrings[1]);
            coordinate.vLoc = int.Parse(vLocStrings[1]);

            string[] hBeaconStrings = beaconCoordinates[0].Split("=");
            string[] vBeaconStrings = beaconCoordinates[1].Split("=");
            nearestBeacon.hLoc = int.Parse(hBeaconStrings[1]);
            nearestBeacon.vLoc = int.Parse(vBeaconStrings[1]);

            int hDistance = Math.Max(nearestBeacon.hLoc, coordinate.hLoc) - Math.Min(nearestBeacon.hLoc, coordinate.hLoc);
            int vDistance = Math.Max(nearestBeacon.vLoc, coordinate.vLoc) - Math.Min(nearestBeacon.vLoc, coordinate.vLoc);

            range = hDistance + vDistance;
        }

        public Tuple<Coordinate, Coordinate> GetRowCoverage(int row)
        {
            int difference = Math.Max(row, coordinate.vLoc) - Math.Min(row, coordinate.vLoc);
            if (difference > range)
            {
                return null;
            } else
            {
                Coordinate leftCoordinate = new Coordinate(coordinate.hLoc - (range - difference), row);
                Coordinate rightCoordinate = new Coordinate(coordinate.hLoc + (range - difference), row);

                return new Tuple<Coordinate, Coordinate>(leftCoordinate, rightCoordinate);
            }
        }
    }

    class Coordinate
    {
        public int hLoc;
        public int vLoc;

        public Coordinate(int hLoc, int vLoc)
        {
            this.hLoc = hLoc;
            this.vLoc = vLoc;
        }

        public Coordinate()
        {

        }
    }
}