using System.IO;
using System.Text.Json;

namespace libs;

public class GameObject : IGameObject, IMovement
{
    private char _charRepresentation = '#';

    private ConsoleColor _color;

    private int _posX;
    private int _posY;

    private int _prevPosX;
    private int _prevPosY;

    public GameObjectType Type;

    public GameObject()
    {
        this._posX = 5;
        this._posY = 5;
        this._color = ConsoleColor.Gray;
    }

    public GameObject(int posX, int posY)
    {
        this._posX = posX;
        this._posY = posY;
    }

    public GameObject(int posX, int posY, ConsoleColor color)
    {
        this._posX = posX;
        this._posY = posY;
        this._color = color;
    }

    public char CharRepresentation
    {
        get { return _charRepresentation; }
        set { _charRepresentation = value; }
    }

    public ConsoleColor Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public int PosX
    {
        get { return _posX; }
        set { _posX = value; }
    }

    public int PosY
    {
        get { return _posY; }
        set { _posY = value; }
    }

    public int GetPrevPosY()
    {
        return _prevPosY;
    }

    public int GetPrevPosX()
    {
        return _prevPosX;
    }

    public bool Move(int dx, int dy)
    {
        bool move = true;
        if (GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Type == GameObjectType.Obstacle)
        {
            return false;
        }
        if (GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Type == GameObjectType.Box)
        {
            move = GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Move(dx, dy);
        }
        if (GameEngine.Instance.GetMap().Get(_posY, _posX).Type == GameObjectType.Box &&
            GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Type == GameObjectType.Goal)
        {
            GameEngine.Instance.GetMap().Get(_posY, _posX).Color = ConsoleColor.Green;
        }
        else if (GameEngine.Instance.GetMap().Get(_posY, _posX).Type == GameObjectType.Box)
        {
            GameEngine.Instance.GetMap().Get(_posY, _posX).Color = ConsoleColor.Red;
        }
        if (move)
        {
            _prevPosX = _posX;
            _prevPosY = _posY;
            _posX += dx;
            _posY += dy;
            CheckWin();
        }
        return move;
    }

    public void SaveState()
    {
        var saveData = new
        {
            Level = GetCurrentLevel(),
            PosX = this._posX,
            PosY = this._posY,
            Color = this._color.ToString(),
            Type = this.Type.ToString()
        };

        string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        string filePath = $"../Save.json";
        //Environment.SetEnvironmentVariable("GAME_SETUP_PATH", filePath);
        File.WriteAllText(filePath, json);
    }

    public bool LoadState()
    {
        string filePath = $"../Save.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var loadData = JsonSerializer.Deserialize<JsonElement>(json);

            if (!loadData.GetProperty("PosX").TryGetInt32(out int posX) ||
             !loadData.GetProperty("PosY").TryGetInt32(out int posY) ||
             string.IsNullOrEmpty(loadData.GetProperty("Color").GetString()) ||
             string.IsNullOrEmpty(loadData.GetProperty("Type").GetString()) ||
             !loadData.GetProperty("Level").TryGetInt32(out int level))  // Checking all required properties
            {
                Console.WriteLine("Failed to load game state correctly.");
                return false;
            }

            this._posX = posX;
            this._posY = posY;
            this._color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), loadData.GetProperty("Color").GetString());
            this.Type = (GameObjectType)Enum.Parse(typeof(GameObjectType), loadData.GetProperty("Type").GetString());
            Environment.SetEnvironmentVariable("CURRENT_LEVEL", level.ToString());  // Set the loaded level into environment variable

            return true;
        }
        Console.WriteLine("No saved game state to load.");
        return false;
    }



    public int GetCurrentLevel()
    {
        string currentLevelStr = Environment.GetEnvironmentVariable("CURRENT_LEVEL");
        if (!string.IsNullOrEmpty(currentLevelStr) && int.TryParse(currentLevelStr, out int currentLevel))
        {
            return currentLevel;
        }
        // Default to level 1 if the environment variable is not set or invalid
        return 1;
    }

    public void CheckWin()
    {
        if (GameEngine.Instance.GetBoxes().All(x => x.Color == ConsoleColor.Green))
        {
            int currentLevel = GetCurrentLevel();
            GameEngine.Instance.clearGameobjects();

            // Increment the level after winning
            currentLevel++;

            // Determine the next level setup JSON file path
            string nextLevelPath = $"../Setup_Level{currentLevel}.json";

            // Check if the next level setup file exists
            if (File.Exists(nextLevelPath))
            {
                Console.Clear();
                Console.WriteLine("You win!");

                // Set the environment variable to the path of the next level JSON file
                Environment.SetEnvironmentVariable("GAME_SETUP_PATH", nextLevelPath);

                Console.WriteLine("Press any key to continue to the next level...");
                Console.ReadKey();

                // Update the current level in the environment variable
                Environment.SetEnvironmentVariable("CURRENT_LEVEL", currentLevel.ToString());

                SaveState();

                // Restart the game with the new setup
                GameEngine.Instance.Setup();
            }
            else
            {
                // Handle the case when there are no more levels available
                Console.WriteLine("Congratulations! You have completed all levels.");
                // Optionally, exit the game or perform other actions
                Environment.Exit(0);
            }
        }
    }
}
