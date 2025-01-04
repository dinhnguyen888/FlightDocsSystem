namespace FlightDocsSystem.Interfaces
{
    public interface ICheckInput
    {
        bool CheckEmail(string email);
        bool CheckPassword(string password);
        bool CheckPhoneNumber(string phoneNumber);
    }
}
