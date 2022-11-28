namespace SharedLibrary.Utils
{
    public static class Randomizer
    {
        private readonly static Random _random = new();

        /// <summary>
        /// Generates a random string of a specified length.
        /// </summary>
        /// <param name="length">Length of generated string</param>
        public static string GetRandomString(int length = 4)
        {
            const string charsSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(charsSet, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Returns a random value from Enum.
        /// </summary>
        /// <typeparam name="T">Specified Enum</typeparam>
        public static T GetRandomEnumValue<T>() where T : Enum
        {
            var enumValuesArray = Enum.GetValues(typeof(T));
            return (T)enumValuesArray.GetValue(_random.Next(enumValuesArray.Length));
        }
    }
}
