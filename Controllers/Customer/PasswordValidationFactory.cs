using System;
public class PasswordValidationFactory
{
    public class PasswordValidationFactory : ValidationFactory
    {
        public (bool, string) Validate(string input)
        {
            return Validation.Password(input);
        }
    }
}