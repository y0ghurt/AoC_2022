namespace day_17
{
    class PyroclasticFlow
    {
        static string commands = "";
        static string filePath = "resources\\day_17_first-input.txt";

        static void Main(string[] args)
        {
            foreach(string line in File.ReadLines(filePath)) {
                commands = line;
            }

            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            List<Shape> shapes = new List<Shape>();
            DefineFirstTaskShapes(shapes);

            long highestPoint = Simulate(shapes, commands, 2022);

            Console.WriteLine("First task: " + highestPoint);
        }
        static void SecondTask()
        {
            List<Shape> shapes = new List<Shape>();
            DefineFirstTaskShapes(shapes);

            long highestPoint = Simulate(shapes, commands, 1000000000000);
            //long highestPoint = Simulate(shapes, commands, 2550);

            Console.WriteLine("Second task: " + highestPoint);
        }

        static long Simulate(List<Shape> shapes, string commands, long numberOfRocksToSimulate)
        {
            List<Coordinate> landedRocks = new List<Coordinate>();

            long highestPoint = 0;
            int commandIterator = 0;
            Shape fallingRock;
            int initialLength = 2551;
            int initialHeight = 3987;
            int cycleLength = 1725;
            int cycleHeight = 2728;
            int iterationsNeeded = 0;
            int cyclesToCalculate = 0;
            //initialLength = 100000;
            long rockIterator = 0;


            if (numberOfRocksToSimulate < initialLength)
            {
                iterationsNeeded = (int)numberOfRocksToSimulate-1;
            } else
            {
                long intermediateThing = (numberOfRocksToSimulate - initialLength);
                iterationsNeeded = (int)(long)(intermediateThing % cycleLength);
                cyclesToCalculate = (int)((numberOfRocksToSimulate - initialLength) / cycleLength);
                commandIterator = 4858;
                highestPoint = (long)((long)initialHeight + ((long)cycleHeight * (long)cyclesToCalculate));
                rockIterator = (int)((long)((long)((long)cyclesToCalculate * (long)cycleLength) + (long)initialLength) % (long)shapes.Count);
            }

            for (int i = 0; i < 7; i++)
            {
                landedRocks.Add(new Coordinate(highestPoint, i));
            }

            for (; rockIterator <= iterationsNeeded; rockIterator++)
            {
                int rockNumber = (int)(rockIterator % shapes.Count);
                fallingRock = new Shape(new Coordinate(highestPoint + 4, 2), shapes[rockNumber].segments);
                bool isFalling = true;
                while (isFalling)
                {
                    commandIterator = commandIterator % (commands.Length);
                    char command = commands[commandIterator];


                    // Get affected by jet stream.
                    if (command == '<')
                    {
                        bool canMoveLeft = true;
                        if (fallingRock.anchorPoint.horizontalCoordinate > 0)
                        {
                            foreach (Coordinate coordinate in fallingRock.leftFacingSegments)
                            {
                                Coordinate nextCoordinate = new Coordinate(fallingRock.anchorPoint.verticalCoordinate + coordinate.verticalCoordinate, fallingRock.anchorPoint.horizontalCoordinate + coordinate.horizontalCoordinate - 1);
                                if (landedRocks.Contains(nextCoordinate))
                                {
                                    canMoveLeft = false;
                                }
                            }
                        }
                        else
                        {
                            canMoveLeft = false;
                        }
                        if (canMoveLeft)
                        {
                            fallingRock.anchorPoint.horizontalCoordinate--;
                        }
                    }
                    else if (command == '>')
                    {
                        bool canMoveRight = true;
                        if (fallingRock.anchorPoint.horizontalCoordinate + fallingRock.width < 6)
                        {
                            foreach (Coordinate coordinate in fallingRock.rightFacingSegments)
                            {
                                Coordinate nextCoordinate = new Coordinate(fallingRock.anchorPoint.verticalCoordinate + coordinate.verticalCoordinate, fallingRock.anchorPoint.horizontalCoordinate + coordinate.horizontalCoordinate + 1);
                                if (landedRocks.Contains(nextCoordinate))
                                {
                                    canMoveRight = false;
                                }
                            }
                        }
                        else
                        {
                            canMoveRight = false;
                        }
                        if (canMoveRight)
                        {
                            fallingRock.anchorPoint.horizontalCoordinate++;
                        }
                    }


                    // Fall.
                    if (fallingRock.anchorPoint.verticalCoordinate > 0)
                    {
                        foreach (Coordinate coordinate in fallingRock.downFacingSegments)
                        {
                            Coordinate nextCoordinate = new Coordinate(fallingRock.anchorPoint.verticalCoordinate + coordinate.verticalCoordinate - 1, fallingRock.anchorPoint.horizontalCoordinate + coordinate.horizontalCoordinate);
                            if (landedRocks.Contains(nextCoordinate))
                            {
                                isFalling = false;
                            }
                        }
                    }
                    else
                    {
                        isFalling = false;
                    }


                    if (isFalling)
                    {
                        fallingRock.anchorPoint.verticalCoordinate--;
                    }
                    else
                    {
                        foreach (Coordinate segment in fallingRock.segments)
                        {
                            landedRocks.Add(new Coordinate(fallingRock.anchorPoint.verticalCoordinate + segment.verticalCoordinate, fallingRock.anchorPoint.horizontalCoordinate + segment.horizontalCoordinate));
                        }
                        if (highestPoint < fallingRock.anchorPoint.verticalCoordinate + fallingRock.height)
                        {
                            highestPoint = fallingRock.anchorPoint.verticalCoordinate + fallingRock.height;
                        }
                    }
                    

                    // Debug print - gets spammy without breakpoints.
                    /*
                    Console.WriteLine(command);
                    for (long i = Math.Max(highestPoint, fallingRock.anchorPoint.verticalCoordinate + fallingRock.height); i >= Math.Max(0, highestPoint-4); i--)
                    {
                        string[] line = new string[] { ".", ".", ".", ".", ".", ".", "." };
                        foreach (Coordinate c in fallingRock.segments)
                        {
                            if (fallingRock.anchorPoint.verticalCoordinate + c.verticalCoordinate == i)
                            {
                                line[fallingRock.anchorPoint.horizontalCoordinate + c.horizontalCoordinate] = "@";
                            }
                        }

                        foreach (Coordinate c in landedRocks)
                        {
                            if (c.verticalCoordinate == i)
                            {
                                line[c.horizontalCoordinate] = "#";
                            }
                        }
                        Console.WriteLine(string.Join("", line));
                    }
                    Console.WriteLine("");
                    */


                    commandIterator++;
                }
                // Debug print - gets spammy without breakpoints.
                
                /*
                for (long i = Math.Max(highestPoint, fallingRock.anchorPoint.verticalCoordinate + fallingRock.height); i >= Math.Max(0, highestPoint-15); i--)
                {
                    string[] line = new string[] { ".", ".", ".", ".", ".", ".", "." };
                    foreach (Coordinate c in landedRocks)
                    {
                        if (c.verticalCoordinate == i)
                        {
                            line[c.horizontalCoordinate] = "#";
                        }
                    }
                    foreach (Coordinate c in fallingRock.segments)
                    {
                        if (fallingRock.anchorPoint.verticalCoordinate + c.verticalCoordinate == i)
                        {
                            line[fallingRock.anchorPoint.horizontalCoordinate + c.horizontalCoordinate] = "@";
                        }
                    }
                    Console.WriteLine(string.Join("", line));
                }
                Console.WriteLine("");
                */
                
                int layerCounter = 0;
                for (int i = 0; layerCounter < 7 && i < landedRocks.Count; i++)
                {
                    if (landedRocks.Count == 0) break;
                    if (landedRocks[i].verticalCoordinate == highestPoint)
                    {
                        layerCounter++;
                    }
                }

                if (layerCounter == 7)
                {
                    Console.WriteLine("rockIterator: " + rockIterator + "   highestPoint: " + highestPoint + "   commandIterator: " + commandIterator);
                    for(int i = 0; i < landedRocks.Count; i++)
                    {
                        if(landedRocks[i].verticalCoordinate < highestPoint)
                        {
                            landedRocks.Remove(landedRocks[i]);
                            i = -1;
                        }
                    }
                }

            }

            return highestPoint;
        }

        static void DefineFirstTaskShapes(List<Shape> shapes)
        {
            List<Coordinate> segments = new List<Coordinate>();

            // ####
            segments.Add(new Coordinate(0, 0));
            segments.Add(new Coordinate(0, 1));
            segments.Add(new Coordinate(0, 2));
            segments.Add(new Coordinate(0, 3));
            shapes.Add(new Shape(new Coordinate(0, 0), segments));
            segments = new List<Coordinate>();

            // .#.
            // ###
            // .#.
            segments.Add(new Coordinate(0, 1));
            segments.Add(new Coordinate(1, 0));
            segments.Add(new Coordinate(1, 1));
            segments.Add(new Coordinate(1, 2));
            segments.Add(new Coordinate(2, 1));
            shapes.Add(new Shape(new Coordinate(0, 0), segments));
            segments = new List<Coordinate>();

            // ..#
            // ..#
            // ###
            segments.Add(new Coordinate(0, 0));
            segments.Add(new Coordinate(0, 1));
            segments.Add(new Coordinate(0, 2));
            segments.Add(new Coordinate(1, 2));
            segments.Add(new Coordinate(2, 2));
            shapes.Add(new Shape(new Coordinate(0, 0), segments));
            segments = new List<Coordinate>();

            // #
            // #
            // #
            // #
            segments.Add(new Coordinate(0, 0));
            segments.Add(new Coordinate(1, 0));
            segments.Add(new Coordinate(2, 0));
            segments.Add(new Coordinate(3, 0));
            shapes.Add(new Shape(new Coordinate(0, 0), segments));
            segments = new List<Coordinate>();

            // ##
            // ##
            segments.Add(new Coordinate(0, 0));
            segments.Add(new Coordinate(0, 1));
            segments.Add(new Coordinate(1, 0));
            segments.Add(new Coordinate(1, 1));
            shapes.Add(new Shape(new Coordinate(0, 0), segments));
            segments = new List<Coordinate>();
        }

        class Shape
        {
            public Coordinate anchorPoint;
            // Segments are defined as relative to the anchorpoint.
            public readonly List<Coordinate> segments;
            public readonly List<Coordinate> leftFacingSegments;
            public readonly List<Coordinate> rightFacingSegments;
            public readonly List<Coordinate> downFacingSegments;
            public readonly long height;
            public readonly long width;

            public Shape(Coordinate anchorPoint, List<Coordinate> segments)
            {
                this.anchorPoint = anchorPoint;
                this.segments = segments;

                leftFacingSegments = new List<Coordinate>();
                foreach (Coordinate coordinate in segments)
                {
                    bool isLeftFacing = true;
                    foreach (Coordinate compCoordinate in segments)
                    {
                        if (coordinate == compCoordinate) continue;

                        if (compCoordinate.verticalCoordinate != coordinate.verticalCoordinate) continue;

                        if (compCoordinate.horizontalCoordinate < coordinate.horizontalCoordinate) isLeftFacing = false;
                    }
                    if (isLeftFacing) leftFacingSegments.Add(coordinate);
                }

                rightFacingSegments = new List<Coordinate>();
                foreach (Coordinate coordinate in segments)
                {
                    bool isRightFacing = true;
                    foreach (Coordinate compCoordinate in segments)
                    {
                        if (coordinate == compCoordinate) continue;

                        if (compCoordinate.verticalCoordinate != coordinate.verticalCoordinate) continue;

                        if (compCoordinate.horizontalCoordinate > coordinate.horizontalCoordinate) isRightFacing = false;
                    }
                    if (isRightFacing) rightFacingSegments.Add(coordinate);
                }

                downFacingSegments = new List<Coordinate>();
                foreach (Coordinate coordinate in segments)
                {
                    bool isDownFacing = true;
                    foreach (Coordinate compCoordinate in segments)
                    {
                        if (coordinate == compCoordinate) continue;

                        if (compCoordinate.horizontalCoordinate != coordinate.horizontalCoordinate) continue;

                        if (compCoordinate.verticalCoordinate < coordinate.verticalCoordinate) isDownFacing = false;
                    }
                    if (isDownFacing) downFacingSegments.Add(coordinate);
                }

                foreach(Coordinate coordinate in segments)
                {
                    if (coordinate.verticalCoordinate > height) height = coordinate.verticalCoordinate;
                }
                foreach (Coordinate coordinate in segments)
                {
                    if (coordinate.horizontalCoordinate > width) width = coordinate.horizontalCoordinate;
                }
            }
        }

        class Coordinate
        {
            public long verticalCoordinate;
            public long horizontalCoordinate;

            public Coordinate() { }

            public Coordinate(long verticalCoordinate, long horizontalCoordinate)
            {
                this.verticalCoordinate = verticalCoordinate;
                this.horizontalCoordinate = horizontalCoordinate;
            }
            public override string ToString()
            {
                return verticalCoordinate + " " + horizontalCoordinate;
            }

            public override bool Equals(object? obj)
            {
                if(obj.GetType() == this.GetType())
                {
                    var coordinate = obj as Coordinate;
                    return (coordinate.verticalCoordinate == this.verticalCoordinate && coordinate.horizontalCoordinate == this.horizontalCoordinate);
                } else
                {
                    return false;
                }
            }
        }
    }
}