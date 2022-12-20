using static day_18.BoilingBoulders;

namespace day_18
{
    class BoilingBoulders
    {
        static string filePath = "resources\\day_18_first-input.txt";
        static List<Cube> cubeList = new List<Cube>();

        static void Main(string[] args)
        {
            ParseInput();

            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {
            Console.WriteLine("First task: " + GetFaces());
        }
        static void SecondTask()
        {

            Console.WriteLine("Second task: " + GetOutwardfacingFaces());
        }

        static int GetFaces()
        {
            foreach(Cube cube in cubeList)
            {
                foreach(Cube cube2 in cubeList)
                {
                    if(cube == cube2)
                    {
                        continue;
                    }

                    if(cube.x == cube2.x && cube.y == cube2.y)
                    {
                        if(cube.z == cube2.z+1 || cube.z == cube2.z-1)
                        {
                            cube.faces--;
                        }
                    }
                    else if (cube.x == cube2.x && cube.z == cube2.z)
                    {
                        if (cube.y == cube2.y + 1 || cube.y == cube2.y - 1)
                        {
                            cube.faces--;
                        }
                    }
                    else if (cube.z == cube2.z && cube.y == cube2.y)
                    {
                        if (cube.x == cube2.x + 1 || cube.x == cube2.x - 1)
                        {
                            cube.faces--;
                        }
                    }
                }
            }

            int faces = cubeList.Sum(x => x.faces);

            return faces;
        }

        static int GetOutwardfacingFaces()
        {
            var xMax = cubeList.Max(x => x.x);
            var xMin = cubeList.Min(x => x.x);
            var yMax = cubeList.Max(x => x.y);
            var yMin = cubeList.Min(x => x.y);
            var zMax = cubeList.Max(x => x.z);
            var zMin = cubeList.Min(x => x.z);
            List<Cube> matrix = new List<Cube>();

            for(int x = xMin-1; x <= xMax; x++)
            {
                for(int y = yMin-1; y <= yMax; y++)
                {
                    for(int z = zMin-1; z <= zMax; z++)
                    {
                        matrix.Add(new Cube(x, y, z));
                    }
                }
            }

            foreach(Cube cube in cubeList)
            {
                Cube lavaCube = matrix.Find(x => x.Equals(cube));
                lavaCube.material = Material.Lava;
            }

            Cube patientZero = matrix.Find(x => x.Equals(new Cube(0, 0, 0)));
            patientZero.material = Material.Water;
            patientZero.Flow(matrix);

            List<Cube> airCubes = matrix.Where(cube => cube.material == Material.Air).ToList();

            foreach (Cube cube in cubeList)
            {
                foreach (Cube cube2 in airCubes)
                {
                    if (cube.x == cube2.x && cube.y == cube2.y)
                    {
                        if (cube.z == cube2.z + 1 || cube.z == cube2.z - 1)
                        {
                            cube.faces--;
                        }
                    }
                    else if (cube.x == cube2.x && cube.z == cube2.z)
                    {
                        if (cube.y == cube2.y + 1 || cube.y == cube2.y - 1)
                        {
                            cube.faces--;
                        }
                    }
                    else if (cube.z == cube2.z && cube.y == cube2.y)
                    {
                        if (cube.x == cube2.x + 1 || cube.x == cube2.x - 1)
                        {
                            cube.faces--;
                        }
                    }
                }
            }

            int faces = cubeList.Sum(x => x.faces);

            return faces;
        }

        static void ParseInput()
        {
            cubeList.Clear();
            foreach (string line in File.ReadLines(filePath))
            {
                string[] xyz = line.Split(",");
                cubeList.Add(new Cube(int.Parse(xyz[0]), int.Parse(xyz[1]), int.Parse(xyz[2])));
            }
        }

        public class Cube
        {
            public Material material = Material.Air;
            public int faces = 6;
            public int x;
            public int y;
            public int z;

            public Cube(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public override bool Equals(object? obj)
            {
                if (obj.GetType() == this.GetType())
                {
                    var cube = obj as Cube;
                    return (cube.x == this.x && cube.y == this.y && cube.z == this.z);
                }
                else
                {
                    return false;
                }
            }

            public void Flow(List<Cube> matrix)
            {
                List<Cube> tempCubes = new List<Cube>();
                tempCubes.Add(new Cube(x - 1, y, z));
                tempCubes.Add(new Cube(x + 1, y, z));
                tempCubes.Add(new Cube(x, y - 1, z));
                tempCubes.Add(new Cube(x, y + 1, z));
                tempCubes.Add(new Cube(x, y, z - 1));
                tempCubes.Add(new Cube(x, y, z + 1));

                var xMax = matrix.Max(x => x.x);
                var xMin = matrix.Min(x => x.x);
                var yMax = matrix.Max(x => x.y);
                var yMin = matrix.Min(x => x.y);
                var zMax = matrix.Max(x => x.z);
                var zMin = matrix.Min(x => x.z);

                List<Cube> flow = new List<Cube>();
                foreach (Cube tempCube in tempCubes)
                {
                    if(xMin <= tempCube.x && tempCube.x <= xMax
                        && yMin <= tempCube.y && tempCube.y <= yMax
                        && zMin <= tempCube.z && tempCube.z <= zMax)
                    {
                        Cube cube = matrix.Find(x => x.Equals(tempCube));
                        if(cube.material == Material.Air)
                        {
                            cube.material = Material.Water;
                            flow.Add(cube);
                        }
                    }
                }

                foreach (Cube flowCube in flow)
                {
                    flowCube.Flow(matrix);
                }

            }

        }

        public enum Material
        {
            Air = 0,
            Lava = 1,
            Water = 2
        }
    }
}