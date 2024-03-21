namespace Banks.Observer;

public interface IObservable
{
    void Notify(string message);
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
}