namespace DalilAcademy.Core.Validators
{
    public static class PasswordValidator
    {
        public static bool BeAValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            return hasUpper && hasLower && hasDigit;
        }
    }
}
