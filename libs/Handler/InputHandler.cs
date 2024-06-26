namespace libs;

public sealed class InputHandler
{

    private static InputHandler? _instance;
    private GameEngine engine;

    public static InputHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler()
    {
        //INIT PROPS HERE IF NEEDED
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo)
    {
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null)
        {
            // Handle keyboard input to move the player
            switch (keyInfo.Key)
            {
                case ConsoleKey.S:
                    focusedObject.SaveState();
                    Console.WriteLine("Game state saved!");
                    break;
                case ConsoleKey.L:
                    if (focusedObject.LoadState())
                        Console.WriteLine("Game state loaded!");
                    else
                        Console.WriteLine("No saved game to load.");
                    break;
                case ConsoleKey.UpArrow:
                    focusedObject.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    focusedObject.Move(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    focusedObject.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    focusedObject.Move(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    GameEngine.Instance.LoadPreviousState();
                    break;
                default:
                    break;
            }
        }

    }

}