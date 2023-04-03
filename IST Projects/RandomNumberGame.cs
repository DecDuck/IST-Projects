using System.Reflection;

namespace IST_Projects;

public class RandomNumberGame
{

    /*
     * Configuration
     * Automatically serializes and deserializes any options within the class using the 'FromString' and 'ToString' functions
     * Does a bunch of reflection magic
     */
    public class Configuration
    {
        public enum RangeEnum:uint
        {
            Easy,
            Medium,
            Hard,
        };

        public static List<int> Ranges = new()
        {
            100,
            1000,
            10000
        };

        public bool TwoPlayer = false;
        public bool ScoreSaving = false;
        public RangeEnum Range = RangeEnum.Easy;

        public static Configuration FromString(string data)
        {
            Configuration configuration = new Configuration();
            foreach (string line in data.Split("\n"))
            {
                string[] parts = line.Split(":");
                if(parts.Length != 2) continue;
                string key = parts[0];
                string value = parts[1];

                FieldInfo? info = configuration.GetType().GetField(key);
                if(info == null) continue;
                info.SetValue(configuration, Convert(value, info.FieldType));
            }

            return configuration;
        }

        public override string ToString()
        {
            string content = "";
            FieldInfo[] fields = GetType().GetFields(~BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                content += $"{field.Name}:{field.GetValue(this)}\n";
            }

            return content;
        }
    }

    public static Configuration GlobalConfiguration;

    public static string GlobalConfigurationPath =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.rng";

    public static Dictionary<Configuration.RangeEnum, int> GlobalHighscores;

    public static string GlobalHighscoresPath =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.rng_highscores";

    public static void Run()
    {
        // Load config
        if (File.Exists(GlobalConfigurationPath))
        {
            GlobalConfiguration = Configuration.FromString(File.ReadAllText(GlobalConfigurationPath));
        }
        else
        {
            GlobalConfiguration = new Configuration();
            File.WriteAllText(GlobalConfigurationPath, GlobalConfiguration.ToString());
        }

        // Load global highscores
        if (File.Exists(GlobalHighscoresPath))
        {
            LoadHighscores();
        }
        else
        {
            GlobalHighscores = new();
            SaveHighscores();
        }

        while (true)
        {
            // Menu Screen
                // Play
                // Options for 2 players, range, score saving
                // Credits
                // Exit
        
            // Options
                // 2 player mode (on/off)
                // Range (1-100, 1-1000, 1-10000)
                // Score saving (on/off)
        
            // Play
            switch (OptionSelector("Welcome to Random Number Game!\nMain Menu", new[] { "Play", "Options", "Credits", "Exit" }))
            {
                case 0:
                    Play();
                    break;
                case 1:
                    OptionsMenu();
                    break;
                case 2:
                    OptionSelector("Main Menu > Credits\nCreated by Declan Hofmeyr\nSubmitted as an IST project for Mr. Ng", new[] { "Back" });
                    break;
                case 3:
                    return;
            }
        }
    }

    public static void OptionsMenu()
    {
        while (true)
        {
            switch (OptionSelector("Main Menu > Options", new[]
                    {
                        $"2 Player Mode {(GlobalConfiguration.TwoPlayer ? "ON" : "OFF")}", 
                        $"Range {Configuration.Ranges[(int) GlobalConfiguration.Range]}",
                        $"Highscores {(GlobalConfiguration.ScoreSaving ? "ON" : "OFF")}",
                        "Back"
                    }))
            {
                case 0:
                    GlobalConfiguration.TwoPlayer =
                        OptionSelector("Main Menu > Options > 2 Player Mode", new[] { "ON", "OFF" }) == 0;
                    break;
                case 1:
                    GlobalConfiguration.Range = (Configuration.RangeEnum) OptionSelector("Main Menu > Options > Range",
                        Enum.GetNames(typeof(Configuration.RangeEnum)));
                    break;
                case 2:
                    GlobalConfiguration.ScoreSaving =
                        OptionSelector("Main Menu > Options > Highscores", new[] { "ON", "OFF" }) == 0;
                    break;
                case 3:
                    return;
            }
            SaveConfiguration();
        }
    }

    public static void Play()
    {
        // Stores player ID to list of guesses
        Dictionary<int, List<int>> playerGuesses = new();
        playerGuesses.Add(0, new ());
        if (GlobalConfiguration.TwoPlayer)
        {
            // Add second place
            playerGuesses.Add(1, new ());
        }
        
        Console.Clear();
        
        // Generate number
        int targetNumber = new Random().Next(0, Configuration.Ranges[(int) GlobalConfiguration.Range]);
        int latestGuess;
        int currentPlayer = 0;
        do
        {
            // Cheating protection
            if (playerGuesses.Count > 1)
            {
                Console.Write("Press any key to start your turn...");
                Console.ReadKey(true);
            }
            Console.WriteLine("\nPlayer {0} | Previously guessed: {1} | Debug: {2}", currentPlayer + 1, string.Join(", ", playerGuesses[currentPlayer]), targetNumber);
            Console.Write("Make your guess: ");
            if (!int.TryParse(Console.ReadLine(), out latestGuess)) continue;
            playerGuesses[currentPlayer].Add(latestGuess);
            currentPlayer = (currentPlayer + 1) % playerGuesses.Count;
            if (latestGuess > targetNumber)
            {
                Console.WriteLine("Unfortunately, too high.");
            }
            else if (latestGuess < targetNumber)
            {
                Console.WriteLine("Unfortunately, too low.");
            }
            else
            {
                continue;
            }

            Console.Write("Press any key to proceed...");
            Console.ReadKey(true);
            Console.Clear();
        } while (latestGuess != targetNumber);
        // If we exit, the previous player guessed correctly
        int winningPlayer = currentPlayer - 1;
        if (winningPlayer < 0) winningPlayer = playerGuesses.Count - 1;

        Console.WriteLine("Congratulations player {0}!", winningPlayer + 1);
        
        // Highscore checking
        if (GlobalConfiguration.ScoreSaving)
        {
            if(!GlobalHighscores.ContainsKey(GlobalConfiguration.Range)) GlobalHighscores.Add(GlobalConfiguration.Range, playerGuesses[winningPlayer].Count + 1);
            if (playerGuesses[winningPlayer].Count < GlobalHighscores[GlobalConfiguration.Range])
            {
                Console.WriteLine("That's a new highscore, player {0}. Congratulations!", winningPlayer + 1);
                GlobalHighscores[GlobalConfiguration.Range] = playerGuesses[winningPlayer].Count;
                SaveHighscores();
            }
        }
        
        Console.Write("Press any key to return to the main menu...");
        Console.ReadKey(true);
    }

    public static void SaveHighscores()
    {
        string file = "";
        foreach (KeyValuePair<Configuration.RangeEnum, int> highscore in GlobalHighscores)
        {
            file += $"{highscore.Key}=${highscore.Value}\n";
        }
        File.WriteAllText(GlobalHighscoresPath, file);
    }

    public static void LoadHighscores()
    {
        GlobalHighscores = new();
        string file = File.ReadAllText(GlobalHighscoresPath);
        foreach (string line in file.Split("\n"))
        {
            if(line == "") continue;
            string[] parts = line.Split("=");
            if(parts.Length != 2) continue;
            GlobalHighscores.Add(Enum.Parse<Configuration.RangeEnum>(parts[0]), int.Parse(parts[1]));
        }
    }

    public static void SaveConfiguration()
    {
        File.WriteAllText(GlobalConfigurationPath, GlobalConfiguration.ToString());
    }

    public static int OptionSelector(string pretext, string[] options)
    {
        int option = 0;
        ConsoleKey key;
        do
        {
            // Render
            Console.Clear();
            Console.WriteLine(pretext);
            for(int i = 0; i < options.Length; i++)
            {
                if (i == option)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.Write(" >{0}", options[i]);
                Console.ResetColor();
                Console.WriteLine();
            }
            
            // Read
            key = Console.ReadKey(true).Key;
            
            // Move logic
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.W) option--;
            if (key == ConsoleKey.DownArrow || key == ConsoleKey.S) option++;
            
            // Wrap logic
            if (option < 0) option = options.Length - 1;
            option %= options.Length;
        } while (key != ConsoleKey.Enter);

        return option;
    }
    
    public static object Convert(string value, Type type)
    {
        if (type.IsEnum)
            return Enum.Parse(type, value);

        return System.Convert.ChangeType(value, type);
    }
}