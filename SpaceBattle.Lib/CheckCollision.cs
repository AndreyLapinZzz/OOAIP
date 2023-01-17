namespace SpaceBattle.Lib;
using Hwdtech;

public class CheckCollision : ICommand
{
    private IUObject obj1, obj2;
    public CheckCollision(IUObject obj1, IUObject obj2){
        this.obj1 = obj1;
        this.obj2 = obj2;
    }

    public void execute(){
        var diffs = IoC.Resolve<List<Vector>>("Game.getDifference", obj1, obj2);
        bool isCollision = IoC.Resolve<bool>("Game.checkCollision", diffs);
        if (isCollision) throw new Exception();
    }
}
