namespace Banks.Models.Passport;

public interface IPassport
{
     string PassportNumber { get; }
     string SerialNumber { get; }
     string IssuePlace { get; }
     string RegisteredPlaceOfLiving { get; }
}