namespace SpaceBattle.Lib;

public interface IStartable
{
    IUObject uobject { get; }
    IDictionary<string, object> properties { get; }
}
