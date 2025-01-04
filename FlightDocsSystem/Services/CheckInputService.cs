using FlightDocsSystem.Interfaces;
using System.Text.RegularExpressions;

namespace FlightDocsSystem.Services
{
    public class CheckInputService : ICheckInput
    {
        public CheckInputService() { }

        public bool CheckEmail(string inputEmail)
        {
            string pattern = @"^\w+@vietjetair\.com$";
            return Regex.IsMatch(inputEmail, pattern);
        }

        public bool CheckPassword(string inputPassword)
        {
           
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(inputPassword, pattern);
        }

        public bool CheckPhoneNumber(string inputPhoneNumber)
        {
            
            string pattern = @"^\d{10,15}$";
            return Regex.IsMatch(inputPhoneNumber, pattern);
        }
    }
}
