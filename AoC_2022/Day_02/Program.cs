using System;
using System.Reflection.Metadata;

namespace day_02
{
    class RockPaperScissors
    {
        static Dictionary<Move, int> results = new Dictionary<Move, int>();

        static void Main(string[] args)
        {
            foreach (string line in File.ReadLines(@"resources\day_02_first-input.txt"))
            {
                ParseInput(line);
            }
            FirstTask();
            SecondTask();
        }

        static void FirstTask()
        {
            int totalScore = 0;

            foreach(KeyValuePair<Move, int> kvp in results)
            {
                int score = CalculateAssumedScore(kvp.Key);
                totalScore += (score * kvp.Value);

                // Debug print, feel free to remove.
                // Console.WriteLine(kvp.Key.ToString() + "   " + kvp.Value);
            }


            Console.WriteLine("First Task: " + totalScore);
        }

        static void SecondTask()
        {
            int totalScore = 0;

            foreach (KeyValuePair<Move, int> kvp in results)
            {
                int score = CalculateActualScore(kvp.Key);
                totalScore += (score * kvp.Value);

                // Debug print, feel free to remove.
                // Console.WriteLine(kvp.Key.ToString() + "   " + kvp.Value);
            }


            Console.WriteLine("Second Task: " + totalScore);
        }

        static int CalculateAssumedScore(Move move)
        {
            int score = 0;
            switch (move.myMove)
            {
                case "X":
                    score = 1;
                    if(move.opponentMove.Equals("A"))
                    {
                        score += 3;
                    } else if(move.opponentMove.Equals("C"))
                    {
                        score += 6;
                    }
                    break;
                case "Y":
                    score = 2;
                    if (move.opponentMove.Equals("B"))
                    {
                        score += 3;
                    }
                    else if (move.opponentMove.Equals("A"))
                    {
                        score += 6;
                    }
                    break;
                case "Z":
                    score = 3;
                    if (move.opponentMove.Equals("C"))
                    {
                        score += 3;
                    }
                    else if (move.opponentMove.Equals("B"))
                    {
                        score += 6;
                    }
                    break;
            }
            return score;
        }
        static int CalculateActualScore(Move move)
        {
            int score = 0;
            int moveScore;
            if(move.opponentMove.Equals("A")) {
                moveScore = 1;
            } else if(move.opponentMove.Equals("B"))
            {
                moveScore = 2;
            } else if(move.opponentMove.Equals("C"))
            {
                moveScore = 3;
            } else
            {
                return score;
            }

            if(move.myMove.Equals("X"))
            {
                moveScore--;
                if(moveScore < 1)
                {
                    moveScore = 3;
                }
                score += moveScore;
            } else if(move.myMove.Equals("Y"))
            {
                score += (3 + moveScore);
            } else if(move.myMove.Equals("Z"))
            {
                moveScore++;
                if(moveScore > 3)
                {
                    moveScore = 1;
                }
                score += (6 + moveScore);
            }
            return score;
        }

        static void ParseInput(string input)
        {
            string[] split = input.Split(" ");
            Move move = new Move(split[0], split[1]);
            if(!results.ContainsKey(move))
            {
                results.Add(move, 1);
            } else
            {
                results[move]++;
            }
            List<int> ints = new List<int>();
        }

        struct Move
        {
            public readonly string opponentMove;
            public readonly string myMove;
            public Move(string p1, string p2)
            {
                opponentMove = p1;
                myMove = p2;
            }

            public override string ToString()
            {
                return opponentMove + " " + myMove;
            }
        }
    }
}