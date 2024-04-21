namespace libs;

public class GameObject : IGameObject, IMovement
{
    private char _charRepresentation = '#';

    private ConsoleColor _color;

    public int posX;
    public int posY;

    public int prevPosX;
    public int prevPosY;

    public GameObjectType Type;

    public GameObject()
    {
        this.posX = 5;
        this.posY = 5;
        this.color = ConsoleColor.Gray;
    }

    public GameObject(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;
    }

    public GameObject(int posX, int posY, ConsoleColor color)
    {
        this.posX = posX;
        this.posY = posY;
        this.color = color;
    }

    public char CharRepresentation
    {
        get { return charRepresentation; }
        set { charRepresentation = value; }
    }

    public ConsoleColor Color
    {
        get { return color; }
        set { color = value; }
    }

    public int PosX
    {
        get { return posX; }
        set { posX = value; }
    }

    public int PosY
    {
        get { return posY; }
        set { posY = value; }
    }

    public int GetPrevPosY()
    {
        return prevPosY;
    }

    public int GetPrevPosX()
    {
        return prevPosX;
    }

    public bool Move(int dx, int dy) {
        bool move = true;
        if(GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Type == GameObjectType.Obstacle) {
            return false;
        }
        if(GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Type == GameObjectType.Box) {
            move = GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Move(dx, dy);
        }
        if(GameEngine.Instance.GetMap().Get(_posY, _posX).Type == GameObjectType.Box &&
            GameEngine.Instance.GetMap().Get(_posY + dy, _posX + dx).Type == GameObjectType.Goal) {
            GameEngine.Instance.GetMap().Get(_posY, _posX).Color = ConsoleColor.Green;
        } else if(GameEngine.Instance.GetMap().Get(_posY, _posX).Type == GameObjectType.Box){
            GameEngine.Instance.GetMap().Get(_posY, _posX).Color = ConsoleColor.Red;
        }
        if(move) {
            _prevPosX = _posX;
            _prevPosY = _posY;
            _posX += dx;
            _posY += dy;
            CheckWin();
        }
        return move;
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

    public void CheckWin() {
        if(GameEngine.Instance.GetBoxes().All(x => x.Color == ConsoleColor.Green)) {
            int currentLevel = GetCurrentLevel();
            GameEngine.Instance.clearGameobjects();

            // Increment the level after winning
            currentLevel++;

            // Determine the next level setup JSON file path
            string nextLevelPath = $"../Setup_Level{currentLevel}.json";

            // Check if the next level setup file exists
            if (File.Exists(nextLevelPath)) {
                Console.Clear();
                Console.WriteLine("You win!");

                // Set the environment variable to the path of the next level JSON file
                Environment.SetEnvironmentVariable("GAME_SETUP_PATH", nextLevelPath);

                Console.WriteLine("Press any key to continue to the next level...");
                Console.ReadKey();

                // Update the current level in the environment variable
                Environment.SetEnvironmentVariable("CURRENT_LEVEL", currentLevel.ToString());

                // Restart the game with the new setup
                GameEngine.Instance.Setup();
            } else {
                // Handle the case when there are no more levels available
                Console.WriteLine("Congratulations! You have completed all levels.");
                // Optionally, exit the game or perform other actions
                Environment.Exit(0);
            }
        }
    }
}
