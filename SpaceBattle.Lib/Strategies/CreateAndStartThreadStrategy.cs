namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        string thread_id = (string)args[0];
        Action? acmd = null;
        if (args.Length > 1)
        {
            acmd = (Action)args[1];
        }
        return new ActionCommand((arg) =>
        {
            try{
            var cmd = new CreateAndStartThreadCommand(thread_id);
            cmd.execute();}
            finally{
            if (acmd != null)
            {
                acmd();
            }}
        });
    }
}
