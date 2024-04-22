namespace libs;

public class Player : GameObject
{
    private static Player instance = null;
    private static readonly object lockObject = new object();

    // Private constructor ensures that an object is not instantiated from outside the class
    private Player() : base()
    {
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;
    }

    // Public static method to get the instance of the class
    public static Player Instance
    {
        get
        {
            // Double-check locking to ensure thread safety
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Player();
                    }
                }
            }
            return instance;
        }
    }

    public override void Move(int dx, int dy)
    {
        prevPosX = GetPrevPosX();
        prevPosY = GetPrevPosY();
        
        posX = PosX();
        posY = PosY();

        prevPosX = posX;
        prevPosY = posY;
        posX += dx;
        posY += dy;
    }
}
