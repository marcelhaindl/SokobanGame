namespace libs;

public class GameObjectFactory : IGameObjectFactory
{
    public GameObject CreateGameObject(dynamic obj)
    {
        if (obj == null)
        {
            Console.WriteLine("Invalid game object data");
            return null;
        }

        GameObject newObj = new GameObject();
        int type = obj.Type;

        switch (type)
        {
            case (int)GameObjectType.Player:
                newObj = obj.ToObject<Player>();
                break;
            case (int)GameObjectType.Obstacle:
                newObj = obj.ToObject<Obstacle>();
                break;
            case (int)GameObjectType.Box:
                newObj = obj.ToObject<Box>();
                break;
            case (int)GameObjectType.Goal:
                newObj = obj.ToObject<Goal>();
                break;
            default:
                Console.WriteLine("Unknown GameObject type");
                return null;
        }

        return newObj;
    }
}