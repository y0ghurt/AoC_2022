namespace day_09
{
    class RopeBridge
    {
        static void Main(string[] args)
        {
            List<string> instructions = new List<string>();
            LoadInstructions(instructions);

            FirstTask(instructions);
            SecondTask(instructions);
        }

        static void FirstTask(List<string> instructions)
        {
            Rope rope = new Rope(2);

            foreach (string instruction in instructions)
            {
                rope.ExecuteInstruction(instruction);
            }

            Console.WriteLine("First task: " + rope.rearVisitedCoordinates.Count);
        }

        static void SecondTask(List<string> instructions)
        {

            Rope rope = new Rope(10);
            foreach (string instruction in instructions)
            {
                rope.ExecuteInstruction(instruction);
            }

            Console.WriteLine("Second task: " + rope.rearVisitedCoordinates.Count);
        }

        static void LoadInstructions(List<string> instructions)
        {
            foreach(string line in File.ReadLines("resources\\day_09_first-input.txt"))
            {
                instructions.Add(line);
            }
        }

        class Rope
        {
            public static readonly string UP = "U";
            public static readonly string DOWN = "D";
            public static readonly string LEFT = "L";
            public static readonly string RIGHT = "R";

            public Dictionary<string, int> rearVisitedCoordinates = new Dictionary<string, int>();

            public List<RopeNode> nodes = new List<RopeNode>();

            // First node ([0]) is considered Head.
            public Rope(int numberOfNodes)
            {
                if(numberOfNodes < 2)
                {
                    throw new ArgumentOutOfRangeException("A rope needs at least 2 nodes to be a rope.");
                }

                for(int i = 0; i < numberOfNodes; i++)
                {
                    nodes.Add(new RopeNode());
                }

                UpdateTailCoordinates(nodes[0].horizontalPosition + " " + nodes[0].verticalPosition);
            }

            public void ExecuteInstruction(string instruction)
            {
                string direction = instruction.Substring(0, 1);
                int stepCount = int.Parse(instruction.Substring(2));
                for (int stepsTaken = 0; stepsTaken < stepCount; stepsTaken++)
                {
                    if (direction.Equals(UP))
                    {
                        nodes[0].verticalPosition++;
                    }
                    else if (direction.Equals(DOWN))
                    {
                        nodes[0].verticalPosition--;
                    }
                    else if (direction.Equals(RIGHT))
                    {
                        nodes[0].horizontalPosition++;
                    }
                    else if (direction.Equals(LEFT))
                    {
                        nodes[0].horizontalPosition--;
                    }
                    MoveTail();
                }
            }

            public void MoveTail()
            {
                for (int nodeNumber = 1; nodeNumber < nodes.Count; nodeNumber++)
                {
                    // If previous node is within reach, no further movement happens.
                    if (!(nodes[nodeNumber - 1].horizontalPosition > nodes[nodeNumber].horizontalPosition + 1)
                        && !(nodes[nodeNumber - 1].horizontalPosition < nodes[nodeNumber].horizontalPosition - 1)
                        && !(nodes[nodeNumber - 1].verticalPosition > nodes[nodeNumber].verticalPosition + 1)
                        && !(nodes[nodeNumber - 1].verticalPosition < nodes[nodeNumber].verticalPosition - 1))
                    {
                        return;
                    }

                    // Check for horizontal movement.
                    if (nodes[nodeNumber - 1].horizontalPosition > nodes[nodeNumber].horizontalPosition + 1
                        || nodes[nodeNumber - 1].horizontalPosition < nodes[nodeNumber].horizontalPosition - 1)
                    {
                        if (nodes[nodeNumber - 1].horizontalPosition > nodes[nodeNumber].horizontalPosition)
                        {
                            nodes[nodeNumber].horizontalPosition++;
                        }
                        else
                        {
                            nodes[nodeNumber].horizontalPosition--;
                        }

                        // Diagonal?
                        if (nodes[nodeNumber - 1].verticalPosition > nodes[nodeNumber].verticalPosition)
                        {
                            nodes[nodeNumber].verticalPosition++;
                        }
                        else if (nodes[nodeNumber - 1].verticalPosition < nodes[nodeNumber].verticalPosition)
                        {
                            nodes[nodeNumber].verticalPosition--;
                        }
                    }

                    // Check for vertical movement.
                    if (nodes[nodeNumber - 1].verticalPosition > nodes[nodeNumber].verticalPosition + 1
                            || nodes[nodeNumber - 1].verticalPosition < nodes[nodeNumber].verticalPosition - 1)
                    {
                        if (nodes[nodeNumber - 1].verticalPosition > nodes[nodeNumber].verticalPosition)
                        {
                            nodes[nodeNumber].verticalPosition++;
                        }
                        else
                        {
                            nodes[nodeNumber].verticalPosition--;
                        }

                        // Diagonal?
                        if (nodes[nodeNumber - 1].horizontalPosition > nodes[nodeNumber].horizontalPosition)
                        {
                            nodes[nodeNumber].horizontalPosition++;
                        }
                        else if (nodes[nodeNumber - 1].horizontalPosition < nodes[nodeNumber].horizontalPosition)
                        {
                            nodes[nodeNumber].horizontalPosition--;
                        }
                    }

                    // Update list of visited coordinates.
                    if (nodeNumber == nodes.Count() - 1)
                    {
                        string tailCoordinates = nodes[nodeNumber].horizontalPosition + " " + nodes[nodeNumber].verticalPosition;

                        UpdateTailCoordinates(tailCoordinates);
                    }
                }
            }

            public class RopeNode
            {
                public int horizontalPosition;
                public int verticalPosition;

                public RopeNode(int horizontalPosition, int verticalPosition)
                {
                    this.horizontalPosition = horizontalPosition;
                    this.verticalPosition = verticalPosition;
                }

                public RopeNode()
                {
                    horizontalPosition = 0;
                    verticalPosition = 0;
                }
            }

            void UpdateTailCoordinates(string coordinate)
            {
                if(rearVisitedCoordinates.ContainsKey(coordinate)) {
                    rearVisitedCoordinates[coordinate]++;
                } else
                {
                    rearVisitedCoordinates.Add(coordinate, 1);
                }
            }
        }
    }
}