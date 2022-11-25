namespace MessagesGenerator.Services
{
    internal static class MessagesGenerator
    {
        private readonly static Random _random = new();

        public static string GetRandomString(int length = 6)
        {
            const string charsSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(charsSet, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static T GetRandomEnumValue<T>() where T : Enum
        {
            var enumValuesArray = Enum.GetValues(typeof(T));
            return (T)enumValuesArray.GetValue(_random.Next(enumValuesArray.Length));
        }
    }
}
