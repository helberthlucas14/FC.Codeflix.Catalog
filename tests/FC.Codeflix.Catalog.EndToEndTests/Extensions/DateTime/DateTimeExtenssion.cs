namespace FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime
{
    internal static class DateTimeExtenssion
    {
        public static System.DateTime TrimMillisseconds(this System.DateTime dateTime)
        {
            return new System.DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Kind);
        }
    }
}
