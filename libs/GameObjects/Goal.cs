namespace libs;

public class Goal : GameObject {
    public Goal () : base() {
        this.Type = GameObjectType.Goal;
        this.CharRepresentation = '#';
        this.Color = ConsoleColor.Red;
    }
}