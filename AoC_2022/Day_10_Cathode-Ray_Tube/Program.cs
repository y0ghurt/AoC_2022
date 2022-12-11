using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;

namespace day_10
{
    class CathodeRayTube
    {
        static void Main(string[] args)
        {
            List<Instruction> instructions = new List<Instruction>();

            LoadInstructions(instructions);

            FirstTask(instructions);
            SecondTask(instructions);
        }

        static void FirstTask(List<Instruction> instructions)
        {
            int X = 1;
            int signalStrength = 0;
            int cycle = 0;
            foreach (Instruction instruction in instructions)
            {
                for(int cycles = instruction.command; cycles > 0; cycles--)
                {
                    cycle++;
                    if((cycle - 20) % 40 == 0)
                    {
                        signalStrength += (cycle * X);
                    }
                }
                if(instruction.command == instruction.ADDX)
                {
                    X += instruction.value;
                }
            }

            Console.WriteLine("First task: " + signalStrength);
        }

        static void SecondTask(List<Instruction> instructions)
        {
            int X = 1;
            int cycle = 0;
            int row = -1;

            List<string[]> pixels = new List<string[]>();
            foreach (Instruction instruction in instructions)
            {
                for (int cycles = instruction.command; cycles > 0; cycles--)
                {
                    if (cycle % 40 == 0)
                    {
                        row++;
                        pixels.Add(new string[] { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." });
                    }

                    if(cycle % 40 >= X - 1 && cycle % 40 <= X + 1)
                    {
                        pixels[row][cycle % 40] = "#";
                    }
                    cycle++;
                }
                if (instruction.command == instruction.ADDX)
                {
                    X += instruction.value;
                }
            }

            Console.WriteLine("Second task: " + "EHPZPJGL");
            foreach (string[] line in pixels)
            {
                Console.WriteLine(string.Join("", line));
            }
        }
        
        static void LoadInstructions(List<Instruction> instructions)
        {
            foreach (string line in File.ReadLines("resources\\day_10_first-input.txt"))
            {
                string[] instruction = line.Split(" ");
                if (instruction.Count() == 1)
                {
                    instructions.Add(new Instruction(instruction[0]));
                }
                else if (instruction.Count() == 2)
                {
                    instructions.Add(new Instruction(instruction[0], int.Parse(instruction[1])));
                }
            }
        }

        class Instruction
        {
            public readonly int NOOP = 1;
            public readonly int ADDX = 2;
            
            public readonly int command;
            public readonly int value;

            public Instruction(string commandString, int value)
            {
                if (commandString.Equals("noop"))
                {
                    command = NOOP;
                    this.value = 0;
                } else if (commandString.Equals("addx"))
                {
                    command = ADDX;
                    this.value = value;
                } else
                {
                    throw new ArgumentOutOfRangeException("Instructions need to be 'noop' or 'addx'.");
                }
            }

            public Instruction(string commandString)
            {
                if (commandString.Equals("noop"))
                {
                    command = NOOP;
                    this.value = 0;
                }
                else if (commandString.Equals("addx"))
                {
                    command = ADDX;
                    this.value = 0;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Instructions need to be 'noop' or 'addx'.");
                }
            }
        }


    }
}