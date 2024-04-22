using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace libs;

public sealed class GameEngine
{
    private static GameEngine? _instance;
    private IGameObjectFactory gameObjectFactory;

    public static GameEngine Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    private GameEngine()
    {
        //INIT PROPS HERE IF NEEDED
        gameObjectFactory = new GameObjectFactory();
    }

    private GameObject? _focusedObject;

    private Map map = new Map();

    private List<List<GameObject>> savedObjectsStack = new List<List<GameObject>>();

    private List<GameObject> gameObjects = new List<GameObject>();

    public Map GetMap()
    {
        return map;
    }

    public void clearGameobjects()
    {
        gameObjects.Clear();
    }

    public GameObject GetFocusedObject()
    {
        return _focusedObject;
    }

    public void Setup()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Setting up game...");

        try
        {
            // Correct usage of static class method
            dynamic gameData = FileHandler.ReadJson();

            if (gameData == null)
            {
                Console.WriteLine("Error: Game data could not be loaded.");
                return;
            }

            // Process game data
            map.MapWidth = gameData.map.width;
            map.MapHeight = gameData.map.height;
            foreach (var gameObject in gameData.gameObjects)
            {
                if (gameObject == null)
                {
                    Console.WriteLine("Warning: Skipping a null gameObject in setup.");
                    continue;
                }
                AddGameObject(CreateGameObject(gameObject));
            }

            savedObjectsStack.Add(gameObjects.Select(x => (GameObject)x.Clone()).ToList());
            _focusedObject = gameObjects.OfType<Player>().FirstOrDefault();

            if (_focusedObject == null)
            {
                Console.WriteLine("Error: No player object found in game data.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during setup: {ex.Message}");
        }

        // foreach (var gameObject in gameData.gameObjects)
        // {
        //     AddGameObject(CreateGameObject(gameObject));
        // }

        // _focusedObject = gameObjects.OfType<Player>().First();
    }

    public void Render()
    {

        //Clean the map
        Console.Clear();

        map.Initialize();

        PlaceGameObjects();

        //Render the map
        for (int i = 0; i < map.MapHeight; i++)
        {
            for (int j = 0; j < map.MapWidth; j++)
            {
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }
    }


    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj)
    {
        return gameObjectFactory.CreateGameObject(obj);
    }

    //Function to get all goal objects
    public List<GameObject> GetBoxes()
    {
        return gameObjects.Where(x => x.Type == GameObjectType.Box).ToList();
    }

    public void AddGameObject(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
    }

    public void SaveCurrentState()
    {
        savedObjectsStack.Add(gameObjects.Select(x => (GameObject)x.Clone()).ToList());
    }

    public void LoadPreviousState()
    {
        if (savedObjectsStack.Count > 1)
        {
            savedObjectsStack.RemoveAt(savedObjectsStack.Count - 1);
        }
        gameObjects = savedObjectsStack.Last();
        Render();
    }


    private void PlaceGameObjects()
    {

        gameObjects.ForEach(delegate (GameObject obj)
        {
            map.Set(obj);
        });
        _focusedObject = gameObjects.FirstOrDefault(x => x.Type == GameObjectType.Player);
        map.Set(_focusedObject);
    }

    private void DrawObject(GameObject gameObject)
    {

        Console.ResetColor();

        if (gameObject != null)
        {
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.CharRepresentation);
        }

        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(' ');
        }
    }
}