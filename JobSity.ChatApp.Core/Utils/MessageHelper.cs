namespace JobSity.ChatApp.Core.Utils
{
    public static class MessageHelper
    {
        public static (bool isStockMessage, string StockCode) ExtractStock(this string message)
        {
            const string StockKey = "/stock=";

            bool isStockMessage = false; 
            
            string StockCode = string.Empty;

            if(message.ToLower().Contains(StockKey))
            {
                isStockMessage = true;
                StockCode = message.Substring(
                    StockKey.Length,
                    message.Length - StockKey.Length
                );

                return(isStockMessage, StockCode);
            }

            return (false, string.Empty);
        }
    }
}