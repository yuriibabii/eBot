namespace eBot.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceWordWithUnderscore(this string @string, string word)
            => @string.Replace($" {word} ", $" {"".PadLeft('_')} ");
    }
}