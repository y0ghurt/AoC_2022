namespace day_05
{
    class SupplyStacks
    {
        static void Main(string[] args)
        {
            List<string> stackStrings = new List<string>();
            List<string> instructionStrings = new List<string>();

            ParseData(stackStrings, instructionStrings);

            List<Instruction> instructions = GetInstructions(instructionStrings);

            FirstTask(GetStacks(stackStrings), instructions);
            SecondTask(GetStacks(stackStrings), instructions);
        }

        static void FirstTask(Dictionary<int, List<string>> stacks, List<Instruction> instructions)
        {
            Console.WriteLine("First task: " + MoveCrates(stacks, instructions, CraneType.NineThousand));
        }

        static void SecondTask(Dictionary<int, List<string>> stacks, List<Instruction> instructions)
        {
            Console.WriteLine("Second task: " + MoveCrates(stacks, instructions, CraneType.NineThousandAndOne));
        }

        public struct Instruction
        {
            public readonly int cratesToMove;
            public readonly int moveFromStack;
            public readonly int moveToStack;

            public Instruction(int cratesToMove, int moveFromStack, int moveToStack)
            {
                this.cratesToMove = cratesToMove;
                this.moveFromStack = moveFromStack;
                this.moveToStack = moveToStack;
            }
        }

        static void ParseData(List<string> stackStrings, List<string> instructionStrings)
        {
            bool allStacksParsed = false;
            foreach (string line in File.ReadLines("resources\\day_05_first-input.txt"))
            {
                if (line.Length == 0)
                {
                    allStacksParsed = true;
                }
                if (allStacksParsed)
                {
                    if (line.Length > 0)
                    {
                        instructionStrings.Add(line);
                    }
                }
                else
                {
                    stackStrings.Add(line);
                }
            }

        }

        static Dictionary<int, List<string>> GetStacks(List<string> stackStrings)
        {
            Dictionary<int, List<string>> stacks = new Dictionary<int, List<string>>();
            Dictionary<int, int> stackMapping = new Dictionary<int, int>();

            for (int stackStringsCounter = stackStrings.Count - 1; stackStringsCounter >= 0; stackStringsCounter--)
            {
                string stackString = stackStrings[stackStringsCounter];
                for (int positionCounter = 1; positionCounter < stackString.Length; positionCounter += 4)
                {
                    if (stackStringsCounter == stackStrings.Count - 1)
                    {
                        stacks.Add(int.Parse(stackString.Substring(positionCounter, 1)), new List<string>());
                        stackMapping.Add(positionCounter, int.Parse(stackString.Substring(positionCounter, 1)));
                    }
                    else
                    {
                        if (!stackString.Substring(positionCounter, 1).Equals(" "))
                        {
                            stacks[stackMapping[positionCounter]].Add(stackString.Substring(positionCounter, 1));
                        }
                    }
                }
            }
            return stacks;
        }

        static List<Instruction> GetInstructions(List<string> instructionStrings)
        {
            List<Instruction> instructions = new List<Instruction>();
            foreach(string instructionString in instructionStrings)
            {
                string[] splitInstructionString = instructionString.Split(' ');
                Instruction instruction = new Instruction(int.Parse(splitInstructionString[1]), int.Parse(splitInstructionString[3]), int.Parse(splitInstructionString[5]));
                instructions.Add(instruction);
            }
            return instructions;
        }

        enum CraneType
        {
            NineThousand,
            NineThousandAndOne
        }

        static string MoveCrates(Dictionary<int, List<string>> stacks, List<Instruction> instructions, CraneType craneType)
        {

            foreach (Instruction instruction in instructions)
            {
                for (int cratesMoved = 0; cratesMoved < instruction.cratesToMove; cratesMoved++)
                {
                    if (craneType == CraneType.NineThousand)
                    {
                        stacks[instruction.moveToStack].Add(stacks[instruction.moveFromStack][stacks[instruction.moveFromStack].Count - 1]);
                        stacks[instruction.moveFromStack].RemoveAt(stacks[instruction.moveFromStack].Count - 1);
                    }
                    else if (craneType == CraneType.NineThousandAndOne)
                    {
                        stacks[instruction.moveToStack].Add(stacks[instruction.moveFromStack][stacks[instruction.moveFromStack].Count - (instruction.cratesToMove - cratesMoved)]);
                        stacks[instruction.moveFromStack].RemoveAt(stacks[instruction.moveFromStack].Count - (instruction.cratesToMove - cratesMoved));

                    }

                }
            }

            string topCrates = "";

            foreach (KeyValuePair<int, List<string>> kvp in stacks)
            {
                topCrates += kvp.Value[kvp.Value.Count - 1];
            }

            return topCrates;
        }
    }
}