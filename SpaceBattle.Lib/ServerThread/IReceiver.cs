using Hwdtech;
using System.Collections.Concurrent;
namespace SpaceBattle.Lib;

public interface IReceiver
{
    ICommand Receive();
    bool isEmpty();
    void Push(ICommand cmd);
}