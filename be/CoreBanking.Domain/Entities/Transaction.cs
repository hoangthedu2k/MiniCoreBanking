using CoreBanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Domain.Entities
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Tự sinh ID

        public string? FromAccount { get; set; }
        public string? ToAccount { get; set; }

        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Message { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    }

}
