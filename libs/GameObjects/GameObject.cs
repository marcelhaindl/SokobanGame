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
    public int GetPosY()
    {
        return _posY;
    }
    public int GetPosX()
    {
        return _posX;
    }
    public int GetPrevPosY()
    {
        return _prevPosY;
    }

    public int GetPrevPosX()
    {
        return _prevPosX;
    }

    public void Move(int dx, int dy)
    {
        _prevPosX = _posX;
        _prevPosY = _posY;
        _posX += dx;
        _posY += dy;
    }


    public void CheckCollision()
{
    GameEngine gameEngine = GameEngine.Instance;
    Map map = gameEngine.GetMap();
    GameObject focusedObject = gameEngine.GetFocusedObject();
    // GameObject? gameObjectBox = gameEngine.GetGameObject(typeof(Box));
    // GameObject? gameObjectObstacle = gameEngine.GetGameObject(typeof(Obstacle));

    // Check if the focusedObject is at the edge of the map
    if (map.Get(focusedObject.PosY , focusedObject.PosX).Type == GameObjectType.Obstacle)
    {
       
        focusedObject.PosX = focusedObject.GetPrevPosX();
        focusedObject.PosY = focusedObject.GetPrevPosY();
        return;
    }

        // BOX MOVING 
        else if (map.Get(focusedObject.GetPosY(), focusedObject.GetPosX()) is Box && focusedObject.GetPrevPosX() == focusedObject.GetPosX() - 1)
        {
            if(map.Get(focusedObject.PosY, focusedObject.PosX).Type == GameObjectType.Box) {
                map.Get(focusedObject.PosY, focusedObject.PosX).Move(1, 0);
            }
            return;
        }
        else if (map.Get(focusedObject.GetPosY(), focusedObject.GetPosX()) is Box && focusedObject.GetPrevPosX() == focusedObject.GetPosX() + 1)
        {
            
            if(map.Get(focusedObject.PosY, focusedObject.PosX).Type == GameObjectType.Box) {
             map.Get(focusedObject.PosY, focusedObject.PosX).Move(-1, 0);
            }
            return;
        }
        else if (map.Get(focusedObject.GetPosY(), focusedObject.GetPosX()) is Box && focusedObject.GetPrevPosY() == focusedObject.GetPosY() - 1)
        {
            if(map.Get(focusedObject.PosY, focusedObject.PosX).Type == GameObjectType.Box) {
             map.Get(focusedObject.PosY, focusedObject.PosX).Move(0, 1);
            }
            return;
        }
        else if (map.Get(focusedObject.GetPosY(), focusedObject.GetPosX()) is Box && focusedObject.GetPrevPosY() == focusedObject.GetPosY() + 1)
        {
            if(map.Get(focusedObject.PosY, focusedObject.PosX).Type == GameObjectType.Box) {
             map.Get(focusedObject.PosY, focusedObject.PosX).Move(0, -1);
            }
            return;
        }
    }
}
