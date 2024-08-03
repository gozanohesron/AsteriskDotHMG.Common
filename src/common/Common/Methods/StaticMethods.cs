namespace AsteriskDotHMG.Common.Methods;

public static partial class StaticMethods
{
    public static string CreateOperationMessage(int recordsInserted, int recordsUpdated)
    {
        string result = "No records affected";

        if (recordsInserted > 0 || recordsUpdated > 0)
        {
            if (recordsInserted > 0 && recordsUpdated > 0)
            {
                result = $"{recordsInserted} {(recordsInserted > 1 ? "rows" : "row")} added and {recordsUpdated} {(recordsUpdated > 1 ? "rows" : "row")} updated";
            }
            else if (recordsInserted > 0 && recordsUpdated == 0)
            {
                result = $"{recordsInserted} {(recordsInserted > 1 ? "rows" : "row")} added";
            }
            else if (recordsInserted == 0 && recordsUpdated > 0)
            {
                result = $"{recordsUpdated} {(recordsUpdated > 1 ? "rows" : "row")} updated";
            }
        }

        return result;
    }

    public static string GeneratePassword(int length = 12, bool includeUppercase = true, bool includeDigits = true, bool includeSpecialChars = true)
    {
        if (length == 0)
        {
            length = 12;
        };

        string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "0123456789";
        string specialCharacters = "!@#$%^&*()";

        StringBuilder passwordBuilder = new();

        // Create a pool of characters based on the selected options
        string characters = lowerCase;
        if (includeUppercase)
            characters += upperCase;
        if (includeDigits)
            characters += numbers;
        if (includeSpecialChars)
            characters += specialCharacters;

        Random random = new();

        // Generate random password
        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(characters.Length);
            passwordBuilder.Append(characters[randomIndex]);
        }

        return passwordBuilder.ToString();
    }

    public static string GenerateOTP(int length = 6)
    {
        if (length == 0)
        {
            length = 6;
        };

        string numbers = "0123456789";
        StringBuilder passwordBuilder = new();
        Random random = new();
        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(numbers.Length);
            passwordBuilder.Append(numbers[randomIndex]);
        }

        return passwordBuilder.ToString();
    }
}
