using System.Xml.Serialization;

namespace day_07
{
    class NoSpaceLeftOnDevice
    {
        static List<string> input = new List<string>();
        private static Dictionary<string, AoCFile> files = new Dictionary<string, AoCFile>();
        private static Dictionary<string, AoCDirectory> directories = new Dictionary<string, AoCDirectory>();
        static string separator = "\\";

        static void Main(string[] args)
        {
            ReadData();
            ParseFileTree();
            CalculateDirectorySizes();

            FirstTask();
            SecondTask();
        }

        static void FirstTask()
        {
            int sum = 0;
            foreach(AoCDirectory directory in directories.Values) 
            {
                if(directory.sizeRecursive <= 100000)
                {
                    sum += directory.sizeRecursive;
                }
                //Console.WriteLine(directory.path + "   " + directory.size);
            }
            Console.WriteLine("First task: " + sum);
        }

        static void SecondTask()
        {
            int neededSpace = 30000000;
            int totalSpace = 70000000;
            int usedSpace = 0;
            foreach(AoCDirectory directory in directories.Values)
            {
                usedSpace += directory.size;
            }

            List<int> validDirectorySizes = new List<int>();
            foreach(AoCDirectory directory in directories.Values)
            {
                if(directory.sizeRecursive >= neededSpace - (totalSpace - usedSpace))
                {
                    validDirectorySizes.Add(directory.sizeRecursive);
                }
            }

            Console.WriteLine("Second task: " + validDirectorySizes.Min());
        }

        static void CalculateDirectorySizes()
        {
            int maxDepth = 0;
            foreach(AoCDirectory directory in directories.Values)
            {
                if(directory.depth > maxDepth)
                {
                    maxDepth = directory.depth;
                }
            }

            for(int depth = maxDepth; depth >= 0; depth--)
            {
                foreach(AoCDirectory directory in directories.Values)
                {
                    if(directory.depth == depth)
                    {
                        CalculateDirectorySize(directory);
                    }
                }
            }
        }

        static void CalculateDirectorySize(AoCDirectory targetDirectory)
        {
            int size = 0;
            foreach(AoCFile file in files.Values)
            {
                if(file.parent == targetDirectory.path)
                {
                    size += file.size;
                }
            }
            targetDirectory.SetSizeNoDirs(size);

            foreach (AoCDirectory directory in directories.Values)
            {
                if (directory.parent == targetDirectory.path)
                {
                    size += directory.sizeRecursive;
                }
            }
            targetDirectory.SetSize(size);
        }

        static void ParseFileTree()
        {
            directories.Add("/", new AoCDirectory("/", "/", "", 0));

            AoCDirectory currentDir = directories["/"];

            for( int commandLineNumber = 0; commandLineNumber < input.Count(); commandLineNumber++)
            {
                ReadOnlySpan<char> command = input[commandLineNumber];

                if(command.Slice(2, 2).ToString().Equals("cd")) {
                    string path = command.ToString().Substring(5);
                    if(path.Equals(".."))
                    {
                        currentDir = directories[currentDir.parent];
                    } else if(path.Equals("/")) {
                        currentDir = directories["/"];
                    } else
                    {
                        currentDir = directories[currentDir.path + separator + path.ToString()];
                    }
                } else if(command.Slice(2, 2).ToString().Equals("ls"))
                {
                    commandLineNumber = GetDirectoryContent(commandLineNumber, currentDir);
                }
            }
        }

        static int GetDirectoryContent(int index, AoCDirectory currentDir)
        {
            while (index + 1 < input.Count && !input[index + 1].StartsWith("$"))
            {
                index++;
                ReadOnlySpan<char> fileOrDirectory = input[index];
                if (fileOrDirectory.Slice(0, 3).ToString().Equals("dir"))
                {
                    // I'm not proud of this, but it works...
                    directories.Add(currentDir.path + separator + fileOrDirectory.Slice(4).ToString(), new AoCDirectory(fileOrDirectory.Slice(4).ToString(), currentDir.path + separator + fileOrDirectory.Slice(4).ToString(), currentDir.path, currentDir.depth + 1));
                } else
                {
                    string[] file = fileOrDirectory.ToString().Split(" ");
                    files.Add(currentDir.path + separator + file[1], new AoCFile(file[1], currentDir.path + separator + file[1], file[0], currentDir.path));
                }
            }
            return index;
        }

        static void ReadData()
        {
            foreach (string line in File.ReadLines("resources\\day_07_first-input.txt"))
            {
                input.Add(line);
            }
        }

        class AoCDirectory
        {
            public readonly string name;
            public readonly string path;
            public readonly string parent;
            public readonly int depth;
            public int sizeRecursive;
            public int size;

            public AoCDirectory(string name, string path, string parent, int depth)
            {
                this.name = name;
                this.path = path;
                this.parent = parent;
                this.depth = depth;
            }

            public void SetSize(int size)
            {
                this.sizeRecursive = size;
            }

            public void SetSizeNoDirs(int sizeNoDirs)
            {
                this.size = sizeNoDirs;
            }
        }

        class AoCFile
        {
            public readonly string name;
            public readonly string path;
            public readonly int size;
            public readonly string parent;

            public AoCFile(string name, string path, string size, string parent)
            {
                this.name = name;
                this.path= path;
                this.size = int.Parse(size);
                this.parent = parent;
            }
            public AoCFile(string name, string path, int size, string parent)
            {
                this.name = name;
                this.path = path;
                this.size = size;
                this.parent = parent;
            }
        }
    }
}