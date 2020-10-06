namespace JobSity.ChatApp.Core.Utils
{
    public static class MessageHelper
    {
        public static (bool isStockMessage, string StockCode) ExtractStock(this string message)
        {
            const string STOCK_KEY = "/stock=";

            bool isStockMessage = false; 
            
            string StockCode = string.Empty;

            if(message.ToLower().Contains(STOCK_KEY))
            {
                isStockMessage = true;
                StockCode = message.Substring(
                    STOCK_KEY.Length,
                    message.Length - STOCK_KEY.Length
                );

                return(isStockMessage, StockCode);
            }

            return (false, string.Empty);
        }
    }
}