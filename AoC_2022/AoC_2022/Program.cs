using System;

namespace day_01
{
    class CalorieCounting
    {
        static void Main(string[] args)
        {
            FirstTask();
            SecondTask();
        }

        static void FirstTask()
        {
            int mostCarbs = 0;
            foreach(int carbs in CheckCarbs())
            {
                if(carbs > mostCarbs) mostCarbs = carbs;
            }
            Console.WriteLine ("First Task: " + mostCarbs);
        }

        static void SecondTask()
        {
            int topThreeCarbs = 0;
            foreach(int carbs in CheckCarbs())
            {
                topThreeCarbs += carbs;
            }
            Console.WriteLine("Second Task: " + topThreeCarbs);
        }

        static List<int> CheckCarbs()
        {
            int mostCarriedCarbs = 0;
            int secondMostCarriedCarbs = 0;
            int thirdMostCarriedCarbs = 0;
            int currentCarriedCarbs = 0;
            foreach (string line in File.ReadLines(@"resources\day_01_first-input.txt"))
            {
                if (line.Length > 0)
                {
                    currentCarriedCarbs += int.Parse(line);
                }
                else
                {
                    if (currentCarriedCarbs >= mostCarriedCarbs)
                    {
                        thirdMostCarriedCarbs = secondMostCarriedCarbs;
                        secondMostCarriedCarbs = mostCarriedCarbs;
                        mostCarriedCarbs = currentCarriedCarbs;
                    } else if (currentCarriedCarbs >= secondMostCarriedCarbs)
                    {
                        thirdMostCarriedCarbs = secondMostCarriedCarbs;
                        secondMostCarriedCarbs = currentCarriedCarbs;
                    } else if (currentCarriedCarbs >= thirdMostCarriedCarbs)
                    {
                        thirdMostCarriedCarbs = currentCarriedCarbs;
                    }
                    currentCarriedCarbs = 0;
                }
            }
            List<int> ints = new List<int>();
            ints.Add(mostCarriedCarbs);
            ints.Add(secondMostCarriedCarbs);
            ints.Add(thirdMostCarriedCarbs);
            return ints;
        }
    }
}