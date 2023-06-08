namespace Banks.Observer;

public interface IObserver
{
    void Update(string message);
    Guid ReturnObserverId();
}