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
                case ConsoleKey.W:
                    focusedObject.Move(0, -1);
                    focusedObject.CheckCollision();
                    break;
                case ConsoleKey.S:
                    focusedObject.Move(0, 1);
                    focusedObject.CheckCollision();
                    break;
                case ConsoleKey.A:
                    focusedObject.Move(-1, 0);
                    focusedObject.CheckCollision();
                    break;
                case ConsoleKey.D:
                    focusedObject.Move(1, 0);
                    focusedObject.CheckCollision();
                    break;
                case ConsoleKey.Z:
                    FileHandler.SaveGame();
                    break;
                case ConsoleKey.I:
                    FileHandler.SetLoadGameOnNextRead(true);
                    break;
                default:
                    break;
            }
        }

    }

}