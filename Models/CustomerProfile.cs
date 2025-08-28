using Azure.Data.Tables;
using System;

namespace azuresolution1.Models
{
    public class CustomerProfile : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public Azure.ETag ETag { get; set; }

        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}