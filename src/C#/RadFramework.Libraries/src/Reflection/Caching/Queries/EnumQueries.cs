namespace RadFramework.Libraries.Reflection.Caching.Queries
{
    public static class EnumQueries
    {
        public static string[] GetEnumValues(Type @enum)
        {
            List<string> stringVals = new();

            foreach (object value in @enum.GetEnumValues())
            {
                stringVals.Add(value.ToString());
            }

            return stringVals.ToArray();;
        }
    }
}