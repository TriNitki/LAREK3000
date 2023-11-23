namespace OrderAPI.Utility
{
    public class SD
    {
        public static string AuthAPIBase { get; set; }
        public static string CatalogAPIBase { get; set; }
        public static string DeliveryAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
