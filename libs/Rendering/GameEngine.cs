using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

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

    public GameObject GetFocusedObject()
    {
        return _focusedObject;
    }

    public void Setup()
    {

        //Added for proper display of game characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        dynamic gameData = FileHandler.ReadJson();

        map.MapWidth = gameData.map.width;
        map.MapHeight = gameData.map.height;

        foreach (var gameObject in gameData.gameObjects)
        {
            AddGameObject(CreateGameObject(gameObject));
        }
        savedObjectsStack.Add(gameObjects.Select(x => (GameObject)x.Clone()).ToList());

        _focusedObject = gameObjects.OfType<Player>().First();

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