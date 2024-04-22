using System.Reflection.Metadata.Ecma335;

namespace libs;

using Newtonsoft.Json;

public static class FileHandler
{

    private static string filePath;
    private readonly static string envVar = "GAME_SETUP_PATH";
    private static bool isGameLoaded = false;
    static FileHandler()
    {
        Initialize();
    }

    private static void Initialize()
    {
        if (Environment.GetEnvironmentVariable(envVar) != null)
        {
            filePath = Environment.GetEnvironmentVariable(envVar);
        };
    }

    public static dynamic ReadJson()
    {


        if (isGameLoaded)
        {
            return LoadGame();
        }


        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    public static void SetLoadGameOnNextRead(bool value)
    {
        isGameLoaded = value;
    }




    public static void SaveGame()
    {
        GameEngine gameEngine = GameEngine.Instance;
        Map map = new Map();

        var GameObjectList = gameEngine.GetListOfObjects();


        string SaveFileName = "SokobanSaveGame.json";
        string ParentDirectoryPath = Convert.ToString(System.IO.Directory.GetParent(filePath));
       
        var currentGameState = GameObjectList;
        var MapSize = gameEngine.GetMap();
        dynamic SaveFilePath = Path.Combine(ParentDirectoryPath, SaveFileName);

        //dynamic JsonContent = MapSize.Add(GameObjectList);

        string jsonContent = JsonConvert.SerializeObject(currentGameState, Formatting.Indented);
        File.WriteAllText(SaveFilePath, jsonContent);
        
        }
    


    public static dynamic LoadGame()
    {
        string saveFileName = "SokobanSaveGame.json";
        string parentDirectoryPath = Convert.ToString(System.IO.Directory.GetParent(filePath));
        string saveFilePath = Path.Combine(parentDirectoryPath, saveFileName);

        
            string jsonContent = File.ReadAllText(saveFilePath);
            dynamic savedGameState = JsonConvert.DeserializeObject(jsonContent);
            filePath = savedGameState;
            return filePath;
    }

}
