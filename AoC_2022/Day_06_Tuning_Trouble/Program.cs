namespace day_06
{
    class TuningTrouble
    {
        static void Main(string[] args)
        {
            string dataStream = ParseData();

            FirstTask(dataStream);
            SecondTask(dataStream);
        }

        static void FirstTask(string dataStream)
        {
            Console.WriteLine("First task: " + FindUniqueCharacterSequence(dataStream, 4));
        }

        static void SecondTask(string dataStream)
        {
            Console.WriteLine("Second task: " + FindUniqueCharacterSequence(dataStream, 14));
        }

        static int FindUniqueCharacterSequence(string dataStream, int length)
        {
            int characterCount = 0;
            for (int startingPoint = 0; 1 == 1; startingPoint++)
            {
                string dataPacket = dataStream.Substring(startingPoint, length);
                bool isStartSequence = true;
                foreach (char c in dataPacket)
                {
                    if (dataPacket.Count(x => x == c) > 1)
                    {
                        isStartSequence = false;
                    }
                }
                if (isStartSequence)
                {
                    characterCount = startingPoint + length;
                    break;
                }
            }
            return characterCount;
        }

        static string ParseData()
        {
            foreach (string line in File.ReadLines("resources\\day_06_first-input.txt"))
            {
                return line;
            }
            return "";
        }
    }
}