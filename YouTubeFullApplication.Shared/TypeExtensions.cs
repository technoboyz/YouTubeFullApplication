namespace YouTubeFullApplication.Shared
{
    public static class TypeExtensions
    {
        public static bool IsDefault<T>(this T value)
        {
            switch (value)
            {
                case bool boolean:
                    return !boolean;
                case string text:
                    return text == string.Empty;
                default:
                    return (Equals(value, default(T)));
            }
        }
    }
}
