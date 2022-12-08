namespace day_08
{
    class TreetopTreeHouse
    {
        static void Main(string[] args)
        {
            List<string> treeLines = getTreeLines();

            Dictionary<string, int> visibleTrees = GetVisibleTrees(treeLines);

            FirstTask(visibleTrees);
            SecondTask(visibleTrees);
        }

        static void FirstTask(Dictionary<string, int> visibleTrees)
        {
            Console.WriteLine("First task: " + visibleTrees.Count());
        }

        static void SecondTask(Dictionary<string, int> visibleTrees)
        {
            Console.WriteLine("Second task: " + visibleTrees.Values.Max());
        }

        static List<string> getTreeLines()
        {
            List<string> treeLines = new List<string>();

            foreach(string treeLine in File.ReadLines("resources\\day_08_first-input.txt"))
            {
                treeLines.Add(treeLine);
            }

            return treeLines;
        }

        static Dictionary<string, int> GetVisibleTrees(List<string> treeLines)
        {
            Dictionary<string, int> visibleTrees = new Dictionary<string, int>();

            int row;
            int column;
            int maxHeight = -1;

            // Left to right.
            for(row = 0; row < treeLines.Count(); row++)
            {
                maxHeight = -1;
                for(column = 0; column < treeLines[row].Length; column++)
                {
                    if (int.Parse(treeLines[row].Substring(column, 1)) > maxHeight)
                    {
                        maxHeight = int.Parse(treeLines[row].Substring(column, 1));
                        try
                        {
                            AddVisibleTreeWithScenicScore(treeLines, visibleTrees, row, column);
                        }
                        catch { }
                    }
                }
            }

            // Right to left.
            for (row = 0; row < treeLines.Count(); row++)
            {
                maxHeight = -1;
                for (column = treeLines[row].Length - 1; column >= 0; column--)
                {
                    if (int.Parse(treeLines[row].Substring(column, 1)) > maxHeight)
                    {
                        maxHeight = int.Parse(treeLines[row].Substring(column, 1));
                        try
                        {
                            AddVisibleTreeWithScenicScore(treeLines, visibleTrees, row, column);
                        }
                        catch { }
                    }
                }
            }

            // Top down.
            for (column = 0; column < treeLines[0].Length; column++)
            {
                maxHeight = -1;
                for (row = 0; row < treeLines.Count(); row++)
                {
                    if (int.Parse(treeLines[row].Substring(column, 1)) > maxHeight)
                    {
                        maxHeight = int.Parse(treeLines[row].Substring(column, 1));
                        try
                        {
                            AddVisibleTreeWithScenicScore(treeLines, visibleTrees, row, column);
                        }
                        catch { }
                    }
                }
            }

            // Bottom up.
            for (column = 0; column < treeLines[0].Length; column++)
            {
                maxHeight = -1;
                for (row = treeLines.Count() -1; row >= 0; row--)
                {
                    if (int.Parse(treeLines[row].Substring(column, 1)) > maxHeight)
                    {
                        maxHeight = int.Parse(treeLines[row].Substring(column, 1));
                        try
                        {
                            AddVisibleTreeWithScenicScore(treeLines, visibleTrees, row, column);
                        }
                        catch { }
                    }
                }
            }
            return visibleTrees;
        }

        static void AddVisibleTreeWithScenicScore(List<string> treeLines, Dictionary<string, int> visibleTrees, int row, int column)
        {
            int west = 0;
            int east = 0;
            int north = 0;
            int south = 0;

            int treeHeight = int.Parse(treeLines[row].Substring(column, 1));

            if(row == 0 || column == 0 || row == (treeLines.Count() - 1) || column == (treeLines[row].Length - 1))
            {
                visibleTrees.Add(row + "-" + column, 0);
                return;
            }

            // Look west.
            int lookRow = row;
            int lookColumn = column;
            while (lookColumn > 0)
            {
                lookColumn--;
                west++;
                if (!(int.Parse(treeLines[lookRow].Substring(lookColumn, 1)) < treeHeight)) {
                    break;
                }
            }

            // Look east.
            lookRow = row;
            lookColumn = column;
            while (lookColumn < treeLines[row].Length - 1)
            {
                lookColumn++;
                east++;
                if (!(int.Parse(treeLines[lookRow].Substring(lookColumn, 1)) < treeHeight))
                {
                    break;
                }
            }

            // Look south.
            lookRow = row;
            lookColumn = column;
            while (lookRow < treeLines.Count() - 1)
            {
                lookRow++;
                south++;
                if (!(int.Parse(treeLines[lookRow].Substring(lookColumn, 1)) < treeHeight))
                {
                    break;
                }
            }

            // Look north.
            lookRow = row;
            lookColumn = column;
            while (lookRow > 0)
            {
                lookRow--;
                north++;
                if (!(int.Parse(treeLines[lookRow].Substring(lookColumn, 1)) < treeHeight))
                {
                    break;
                }
            }

            visibleTrees.Add(row + "-" + column, (west * east * north * south));

        }
    }
}