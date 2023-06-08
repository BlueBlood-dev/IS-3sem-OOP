namespace Banks.Models;

public class Address
{
    public Address(string livingPlace)
    {
        if (string.IsNullOrEmpty(livingPlace))
            throw new ArgumentException("invalid client's living place");
        LivingPlace = livingPlace;
    }

    public string LivingPlace { get; }
}