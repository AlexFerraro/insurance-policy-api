namespace insurance_policy_api_domain.Helpers;

public static class EnumerableExtensions
{
    public static bool ContainsAny(this IEnumerable<string> source, params string[] values)
    {
        if (source.Any() && values.Length > 0)
        {
            foreach (string value in values)
            {
                if (source.Contains(value))
                    return true;
            }
        }

        return false;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
    {
        if (source.Equals(null))
            throw new ArgumentException("A coleção não pode ser nula!");

        foreach (T element in source) act(element);

        return source;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) =>
        source is null || !source.Any();

}