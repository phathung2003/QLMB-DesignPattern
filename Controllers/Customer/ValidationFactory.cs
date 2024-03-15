using System;

public class ValidationFactory
{
    public interface ValidationFactory
    {
        (bool, string) Validate(string input);
    }
}
