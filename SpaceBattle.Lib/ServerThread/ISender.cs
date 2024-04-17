using SpaceBattle.Lib;

namespace SpaceBattle.Lib;
public interface ISender{
    void Send(ICommand cmd);
}
