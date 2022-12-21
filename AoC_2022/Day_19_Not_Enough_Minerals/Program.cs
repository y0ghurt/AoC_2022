using System.Numerics;
using System.Resources;
using System.Text.RegularExpressions;

namespace day_19
{
    class NotEnoughMinerals
    {
        static string filePath = "resources\\day_19_first-input.txt";

        static List<FactoryRun> factoryRuns = new List<FactoryRun>();
        static List<Blueprint> blueprints = new List<Blueprint>();
        static long runCounter = 0;
        static Dictionary<int, int> bestResult = new Dictionary<int, int>();

        public static int maxQualityScore = 0;

        static void Main(string[] args)
        {
            ParseInput();

            FirstTask();

            SecondTask();
        }

        static void FirstTask()
        {

            int result = 0;
            List<int> qualityLevels = new List<int>();
            foreach (Blueprint blueprint in blueprints)
            {
                FactoryRun factoryRun = new FactoryRun(blueprint, 24);
                factoryRuns.Add(factoryRun);
                for (int i = factoryRuns.Count; i > 0; i--)
                {
                    factoryRuns[i-1].FindMaxGeodes();
                    i = factoryRuns.Count+1;
                }
                Console.WriteLine(String.Format("Blueprint: {0}    Quality Score: {1}", blueprint.id, maxQualityScore));
                result += maxQualityScore;
                maxQualityScore = 0;
                bestResult.Clear();
                factoryRuns.Clear();
            }

            Console.WriteLine("First task: " + result);
        }
        static void SecondTask()
        {

            int result = 1;
            List<int> qualityLevels = new List<int>();
            for(int bpc = 0; bpc < 3; bpc++)
            {
                Blueprint blueprint = blueprints[bpc];
                FactoryRun factoryRun = new FactoryRun(blueprint, 32);
                factoryRuns.Add(factoryRun);
                for (int i = factoryRuns.Count; i > 0; i--)
                {
                    factoryRuns[i - 1].FindMaxGeodesSecondTask();
                    i = factoryRuns.Count + 1;
                }
                int geodes = maxQualityScore / blueprint.id;
                Console.WriteLine(String.Format("Blueprint: {0}    Geodes: {1}", blueprint.id, geodes));
                result = result * geodes;
                maxQualityScore = 0;
                bestResult.Clear();
                factoryRuns.Clear();
            }
            
            Console.WriteLine("Second task: " + result);
        }

        class Blueprint
        {
            public int id;
            public Robot oreRobot = new Robot(Resources.Ore);
            public Robot clayRobot = new Robot(Resources.Clay);
            public Robot obsidianRobot = new Robot(Resources.Obsidian);
            public Robot geodeRobot = new Robot(Resources.Geode);

            public Blueprint(int id)
            {
                this.id = id;
            }
        }

        class Robot
        {
            public Resources excavatedResource;

            public int oreCost = 0;
            public int clayCost = 0;
            public int obsidianCost = 0;

            public Robot(Resources excavatedResource)
            {
                this.excavatedResource = excavatedResource;
            }
        }

        class FactoryRun
        {
            int minute = 1;
            int minutesToGo;
            Blueprint blueprint;
            // Simplified representation of a robot manufacturing queue. It's enough to know the element.
            List<Resources> manufacturingQueue = new List<Resources>();
            Storage storage = new Storage();
            RoboticFleet roboticFleet = new RoboticFleet();

            public FactoryRun(Blueprint blueprint, int minutesToGo, int minute, List<Resources> manufacturingQueue, Storage storage, RoboticFleet roboticFleet)
            {
                this.minute = minute;
                this.minutesToGo = minutesToGo;
                this.blueprint = blueprint;
                this.manufacturingQueue = manufacturingQueue;
                this.storage = storage;
                this.roboticFleet = roboticFleet;
                runCounter++;
            }
            public FactoryRun(Blueprint blueprint, int minutesToGo, List<Resources> manufacturingQueue, Storage storage, RoboticFleet roboticFleet)
            {
                this.minutesToGo = this.minutesToGo;
                this.blueprint = blueprint;
                this.manufacturingQueue = manufacturingQueue;
                this.storage = storage;
                this.roboticFleet = roboticFleet;
                runCounter++;
            }

            public FactoryRun(Blueprint blueprint, int minutesRemaining)
            {
                this.blueprint = blueprint;
                this.minutesToGo = minutesRemaining;
                runCounter++;
            }


            public void FindMaxGeodes()
            {
                FactoryRun factoryRun;
                if (!bestResult.ContainsKey(minute))
                {
                    bestResult.Add(minute, storage.geodeStorage);
                }
                else
                {
                    if (storage.geodeStorage < bestResult[minute])
                    {
                        factoryRuns.Remove(this);
                        return;
                    }
                }
                if (minute <= minutesToGo)
                {
                    if (HasEnoughResourcesFor(blueprint.geodeRobot))
                    {
                        factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), storage, roboticFleet);
                        factoryRun.BuildRobot(blueprint.geodeRobot);
                        factoryRun.Progress();
                        factoryRuns.Add(factoryRun);
                    }
                    else 
                    {
                        factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                        factoryRun.Progress();
                        factoryRuns.Add(factoryRun);
                        if (HasEnoughResourcesFor(blueprint.oreRobot) && roboticFleet.oreRobots < Math.Max(blueprint.geodeRobot.oreCost, Math.Max(blueprint.obsidianRobot.oreCost, blueprint.clayRobot.oreCost)))
                        {
                            factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                            factoryRun.BuildRobot(blueprint.oreRobot);
                            factoryRun.Progress();
                            factoryRuns.Add(factoryRun);
                        }
                        if (HasEnoughResourcesFor(blueprint.clayRobot) && roboticFleet.clayRobots < blueprint.obsidianRobot.clayCost)
                        {
                            factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                            factoryRun.BuildRobot(blueprint.clayRobot);
                            factoryRun.Progress();
                            factoryRuns.Add(factoryRun);
                        }
                        if (HasEnoughResourcesFor(blueprint.obsidianRobot) && roboticFleet.obsidianRobots < blueprint.geodeRobot.obsidianCost)
                        {
                            factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                            factoryRun.BuildRobot(blueprint.obsidianRobot);
                            factoryRun.Progress();
                            factoryRuns.Add(factoryRun);
                        }
                    }

                    factoryRuns.Remove(this);
                }
                else
                {
                    int qualityScore = storage.geodeStorage * blueprint.id;
                    if (qualityScore > maxQualityScore)
                    {
                        maxQualityScore = qualityScore;
                    }
                    factoryRuns.Remove(this);
                }
            }

            public void FindMaxGeodesSecondTask()
            {
                FactoryRun factoryRun;
                if (!bestResult.ContainsKey(minute))
                {
                    bestResult.Add(minute, storage.geodeStorage);
                }
                else
                {
                    if (storage.geodeStorage < bestResult[minute])
                    {
                        factoryRuns.Remove(this);
                        return;
                    }
                }
                if (minute <= minutesToGo)
                {
                    if (HasEnoughResourcesFor(blueprint.geodeRobot))
                    {
                        factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), storage, roboticFleet);
                        factoryRun.BuildRobot(blueprint.geodeRobot);
                        factoryRun.Progress();
                        factoryRuns.Add(factoryRun);
                    }
                    else if (HasEnoughResourcesFor(blueprint.obsidianRobot) && roboticFleet.obsidianRobots < blueprint.geodeRobot.obsidianCost)
                    {
                        factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                        factoryRun.BuildRobot(blueprint.obsidianRobot);
                        factoryRun.Progress();
                        factoryRuns.Add(factoryRun);
                    }
                    else

                    {
                        factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                        factoryRun.Progress();
                        factoryRuns.Add(factoryRun);
                        if (HasEnoughResourcesFor(blueprint.oreRobot) && roboticFleet.oreRobots < Math.Max(blueprint.geodeRobot.oreCost, Math.Max(blueprint.obsidianRobot.oreCost, blueprint.clayRobot.oreCost)))
                        {
                            factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                            factoryRun.BuildRobot(blueprint.oreRobot);
                            factoryRun.Progress();
                            factoryRuns.Add(factoryRun);
                        }
                        if (HasEnoughResourcesFor(blueprint.clayRobot) && roboticFleet.clayRobots < blueprint.obsidianRobot.clayCost)
                        {
                            factoryRun = new FactoryRun(blueprint, minutesToGo, minute, new List<Resources>(manufacturingQueue), new Storage(storage), new RoboticFleet(roboticFleet));
                            factoryRun.BuildRobot(blueprint.clayRobot);
                            factoryRun.Progress();
                            factoryRuns.Add(factoryRun);
                        }
                    }

                    factoryRuns.Remove(this);
                }
                else
                {
                    int qualityScore = storage.geodeStorage * blueprint.id;
                    if (qualityScore > maxQualityScore)
                    {
                        maxQualityScore = qualityScore;
                    }
                    factoryRuns.Remove(this);
                }
            }



            void Progress()
            {
                minute++;

                storage.oreStorage += roboticFleet.oreRobots;
                storage.clayStorage += roboticFleet.clayRobots;
                storage.obsidianStorage += roboticFleet.obsidianRobots;
                storage.geodeStorage += roboticFleet.geodeRobots;

                roboticFleet.oreRobots += manufacturingQueue.Count(x => x == Resources.Ore);
                roboticFleet.clayRobots += manufacturingQueue.Count(x => x == Resources.Clay);
                roboticFleet.obsidianRobots += manufacturingQueue.Count(x => x == Resources.Obsidian);
                roboticFleet.geodeRobots += manufacturingQueue.Count(x => x == Resources.Geode);

                manufacturingQueue.Clear();
            }

            void BuildRobot(Robot robot)
            {
                storage.oreStorage -= robot.oreCost;
                storage.clayStorage -= robot.clayCost;
                storage.obsidianStorage -= robot.obsidianCost;

                manufacturingQueue.Add(robot.excavatedResource);
            }

            bool HasEnoughResourcesFor(Robot robot)
            {
                if (storage.oreStorage < robot.oreCost) return false;
                if (storage.clayStorage < robot.clayCost) return false;
                if (storage.obsidianStorage < robot.obsidianCost) return false;
                return true;

            }
            bool IsItQuickerToWait(Robot desiredRobot, Robot supportingRobot)
            {
                List<int> turnsToBuildOnWait = new List<int>();
                List<int> turnsToBuildOnBuild = new List<int>();
                if (desiredRobot.oreCost > 0)
                {
                    if (roboticFleet.oreRobots == 0) return false;

                    turnsToBuildOnWait.Add((desiredRobot.oreCost - storage.oreStorage) / roboticFleet.oreRobots);
                    if (supportingRobot.excavatedResource == Resources.Ore)
                    {
                        turnsToBuildOnBuild.Add((desiredRobot.oreCost + supportingRobot.oreCost - storage.oreStorage + 1) / (roboticFleet.oreRobots + 1));
                    }
                    else
                    {
                        turnsToBuildOnBuild.Add((desiredRobot.oreCost + supportingRobot.oreCost - storage.oreStorage) / roboticFleet.oreRobots);
                    }
                }

                if (desiredRobot.clayCost > 0)
                {
                    if (roboticFleet.clayRobots == 0) return false;
                    turnsToBuildOnWait.Add((desiredRobot.clayCost - storage.clayStorage) / roboticFleet.clayRobots);
                    if (supportingRobot.excavatedResource == Resources.Clay)
                    {
                        turnsToBuildOnBuild.Add((desiredRobot.clayCost + supportingRobot.clayCost - storage.clayStorage + 1) / (roboticFleet.clayRobots + 1));
                    }
                    else
                    {
                        turnsToBuildOnBuild.Add((desiredRobot.clayCost + supportingRobot.clayCost - storage.clayStorage) / roboticFleet.clayRobots);
                    }
                }

                if (desiredRobot.obsidianCost > 0)
                {
                    if (roboticFleet.obsidianRobots == 0) return false;
                    turnsToBuildOnWait.Add((desiredRobot.obsidianCost - storage.obsidianStorage) / roboticFleet.obsidianRobots);
                    if (desiredRobot.excavatedResource == Resources.Obsidian)
                    {
                        turnsToBuildOnBuild.Add((desiredRobot.obsidianCost + supportingRobot.obsidianCost - storage.obsidianStorage + 1) / (roboticFleet.obsidianRobots + 1));
                    }
                    else
                    {
                        turnsToBuildOnBuild.Add((desiredRobot.obsidianCost + supportingRobot.obsidianCost - storage.obsidianStorage) / roboticFleet.obsidianRobots);
                    }
                }
                int wait = turnsToBuildOnWait.Max();
                int build = turnsToBuildOnBuild.Max();
                if (wait < build)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }


        struct RoboticFleet
        {
            public int oreRobots = 1;
            public int clayRobots = 0;
            public int obsidianRobots = 0;
            public int geodeRobots = 0;

            public RoboticFleet()
            {

            }

            public RoboticFleet(RoboticFleet roboticFleet)
            {
                this.oreRobots = roboticFleet.oreRobots;
                this.clayRobots = roboticFleet.clayRobots;
                this.obsidianRobots = roboticFleet.obsidianRobots;
                this.geodeRobots = roboticFleet.geodeRobots;
            }
        }

        struct Storage
        {
            public int oreStorage = 0;
            public int clayStorage = 0;
            public int obsidianStorage = 0;
            public int geodeStorage = 0;

            public Storage()
            {

            }

            public Storage(Storage storage)
            {
                this.oreStorage = storage.oreStorage;
                this.clayStorage = storage.clayStorage;
                this.obsidianStorage = storage.obsidianStorage;
                this.geodeStorage = storage.geodeStorage;
            }
        }

        enum Resources
        {
            Ore = 0,
            Clay = 1,
            Obsidian = 2,
            Geode = 3,
            Unknown = 4
        }


        static void ParseInput()
        {
            foreach (string line in File.ReadLines(filePath))
            {
                string[] colon = line.Split(':');
                int id = int.Parse(colon[0].Split(" ")[1]);
                Blueprint blueprint = new Blueprint(id);
                Resources robotType = Resources.Unknown;

                string[] costTexts = colon[1].Split(".");
                for (int costTextsCounter = 0; costTextsCounter < costTexts.Length; costTextsCounter++)
                {
                    int oreCost = 0;
                    int clayCost = 0;
                    int obsidianCost = 0;


                    if (costTexts[costTextsCounter].Length == 0) continue;

                    string[] robot = costTexts[costTextsCounter].Split("costs");
                    if (robot[0].Contains("ore")) robotType = Resources.Ore;
                    if (robot[0].Contains("clay")) robotType = Resources.Clay;
                    if (robot[0].Contains("obsidian")) robotType = Resources.Obsidian;
                    if (robot[0].Contains("geode")) robotType = Resources.Geode;

                    if (robot[1].Contains("and"))
                    {
                        string[] costs = robot[1].Split("and");
                        for (int costCounter = 0; costCounter < costs.Length; costCounter++)
                        {
                            int amount = int.Parse(Regex.Match(costs[costCounter], "\\d+").Captures[0].Value);
                            if (costs[costCounter].Contains("ore")) oreCost = amount;
                            if (costs[costCounter].Contains("clay")) clayCost = amount;
                            if (costs[costCounter].Contains("obsidian")) obsidianCost = amount;
                        }
                    }
                    else
                    {
                        int amount = int.Parse(Regex.Match(robot[1], "\\d+").Captures[0].Value);
                        if (robot[1].Contains("ore")) oreCost = amount;
                        if (robot[1].Contains("clay")) clayCost = amount;
                        if (robot[1].Contains("obsidian")) obsidianCost = amount;
                    }

                    if (robotType == Resources.Ore)
                    {
                        blueprint.oreRobot.excavatedResource = robotType;
                        blueprint.oreRobot.oreCost = oreCost;
                        blueprint.oreRobot.clayCost = clayCost;
                        blueprint.oreRobot.obsidianCost = obsidianCost;
                    }
                    else if (robotType == Resources.Clay)
                    {
                        blueprint.clayRobot.excavatedResource = robotType;
                        blueprint.clayRobot.oreCost = oreCost;
                        blueprint.clayRobot.clayCost = clayCost;
                        blueprint.clayRobot.obsidianCost = obsidianCost;
                    }
                    else if (robotType == Resources.Obsidian)
                    {
                        blueprint.obsidianRobot.excavatedResource = robotType;
                        blueprint.obsidianRobot.oreCost = oreCost;
                        blueprint.obsidianRobot.clayCost = clayCost;
                        blueprint.obsidianRobot.obsidianCost = obsidianCost;
                    }
                    else if (robotType == Resources.Geode)
                    {
                        blueprint.geodeRobot.excavatedResource = robotType;
                        blueprint.geodeRobot.oreCost = oreCost;
                        blueprint.geodeRobot.clayCost = clayCost;
                        blueprint.geodeRobot.obsidianCost = obsidianCost;

                    }
                }

                blueprints.Add(blueprint);
            }
        }
    }
}