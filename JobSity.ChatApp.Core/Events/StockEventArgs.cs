using System;

namespace JobSity.ChatApp.Core.Events
{
    public class StockEventArgs : EventArgs
    {
        public string StockInfo { get; set; }
    }
}