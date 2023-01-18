namespace SpaceBattle.Lib;

public class RotateCommand : ICommand{

    private readonly IRotable rotable;

    public RotateCommand(IRotable rotable) => this.rotable = rotable;
    
    public void execute() {
        this.rotable.Angle += this.rotable.AngleVelocity;
    }
}
