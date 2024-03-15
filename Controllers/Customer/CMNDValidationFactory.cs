using System;

public class CMNDValidationFactory
{
    public class CMNDValidationFactory : ValidationFactory
    {
        public (bool, string) Validate(string input)
        {
            return Validation.CMND(input);
        };
    }
}
