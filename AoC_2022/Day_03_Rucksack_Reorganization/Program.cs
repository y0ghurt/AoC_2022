using System.Text;

namespace day_03
{
    class RucksackReorganization
    {
        static List<string> lines = new List<string>();
        static List<List<string>> threeLines = new List<List<string>>();
        static List<char> matchesTotal = new List<char>();
        static List<char> badges = new List<char>();

        static void Main(string[] args)
        {

            FirstTask();
            SecondTask();
        }

        static void FirstTask()
        {
            foreach (string line in File.ReadLines(@"resources\day_03_first-input.txt"))
            {
                lines.Add(line);
            }

            foreach (string line in lines)
            {
                List<char> matchesInLine = new List<char>();
                string first = line.Substring(0, (line.Length / 2));
                string second = line.Substring((line.Length / 2));

                foreach (char item in first)
                {

                    if (second.Contains(item))
                    {
                        int matched = 0;
                        if (!matchesInLine.Contains(item))
                        {
                            matchesInLine.Add(item);
                            matchesTotal.Add(item);
                        }
                    }
                }
            }

            int missorterPriority = getPriorities(matchesTotal);

            Console.WriteLine("First task: " + missorterPriority);
        }

        static void SecondTask()
        {
            int lineCounter = 0;
            threeLines.Add(new List<string>());
            foreach (string line in File.ReadLines(@"resources\day_03_first-input.txt"))
            {
                if (threeLines[lineCounter].Count() == 3)
                {
                    lineCounter++;
                    threeLines.Add(new List<string>());
                }
                threeLines[lineCounter].Add(line);
            }

            foreach(List<string> group in threeLines)
            {
                foreach(char itemType in group[0])
                {
                    if (group[1].Contains(itemType) && group[2].Contains(itemType))
                    {
                        badges.Add(itemType);
                        break;
                    }
                }
            }

            int badgePriority = getPriorities(badges);

            Console.WriteLine("Second task: " + badgePriority);
        }

        static int getPriorities(List<char> input)
        {
            int output = 0;

            string matches = new string(input.ToArray());

            byte[] asciis = Encoding.ASCII.GetBytes(matches);
            foreach (byte ascii in asciis)
            {
                if (ascii > 96)
                {
                    output += (ascii - 96);
                }
                else
                {
                    output += (ascii - 38);
                }
            }
            return output;
        }
    }
}