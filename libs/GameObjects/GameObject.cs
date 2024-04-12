namespace libs;

public class GameObject : IGameObject, IMovement
{
    private char charRepresentation = '#';
    private ConsoleColor color;

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

    public virtual void Move(int dx, int dy)
    {
        prevPosX = posX;
        prevPosY = posY;
        posX += dx;
        posY += dy;
    }
}
