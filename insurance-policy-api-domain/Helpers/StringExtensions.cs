namespace insurance_policy_api_domain.Helpers;

public static class StringExtensions
{
    public static bool ContainsAnyEquals(this string strs, params string[] values)
    {
        if (!string.IsNullOrEmpty(strs) && values.Length > 0)
        {
            foreach (string value in values)
            {
                if (strs.Equals(value))
                    return true;
            }
        }

        return false;
    }

    public static bool ContainsAny(this string str, string values)
    {
        if (!string.IsNullOrEmpty(str) && values.Length > 0)
        {
            foreach (char value in values)
            {
                if (str.Contains(value))
                    return true;
            }
        }

        return false;
    }
}
