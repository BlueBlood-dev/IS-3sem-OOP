using Banks.Exceptions;

namespace Banks.Models.Passport;

public class RussianPassport : IPassport
{
    private const int SerialNumberLength = 4;
    private const int PassportNumberLength = 6;

    public RussianPassport(string serialNumber, string number, string registeredPlaceOfLiving, string issuePlace)
    {
        CheckIfSerialNumberValid(serialNumber);
        CheckIfPassportNumberIsValid(number);
        if (string.IsNullOrEmpty(registeredPlaceOfLiving))
            throw new ArgumentException("place of living is invalid");
        if (string.IsNullOrWhiteSpace(issuePlace))
            throw new ArgumentException("issue place is invalid");
        SerialNumber = serialNumber;
        PassportNumber = number;
        RegisteredPlaceOfLiving = registeredPlaceOfLiving;
        IssuePlace = issuePlace;
    }

    public string PassportNumber { get; }
    public string SerialNumber { get; }
    public string IssuePlace { get; }
    public string RegisteredPlaceOfLiving { get; }

    private void CheckIfSerialNumberValid(string serialNumber)
    {
        if (!int.TryParse(serialNumber, out _))
            throw PassportException.WrongSerialNumberException($"serial number {serialNumber} is wrong");
        if (serialNumber.Length != SerialNumberLength)
        {
            throw PassportException.WrongSerialNumberException(
                $"serial number length is wrong, required {SerialNumberLength}, entered {serialNumber.Length}");
        }
    }

    private void CheckIfPassportNumberIsValid(string passportNumber)
    {
        if (!int.TryParse(passportNumber, out _))
            throw PassportException.WrongPassportNumberException($"passport number {passportNumber} is wrong");
        if (passportNumber.Length != PassportNumberLength)
        {
            throw PassportException.WrongPassportNumberException(
                $"serial number length is wrong, required {PassportNumberLength}, entered {passportNumber.Length}");
        }
    }
}