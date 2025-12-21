using CoreBanking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Domain.Entities
{
    public class Account
    {
        public string AccountNumber { get; set; } = string.Empty; // Khóa chính
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } // Navigation Property

        // private set: Chỉ có thể sửa Balance từ bên trong class này
        public decimal Balance { get; private set; }
        public string Currency { get; set; } = "VND";
        public AccountType AccountType { get; set; } = AccountType.Payment;
        public string Status { get; set; } = "ACTIVE";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Logic nghiệp vụ: Nạp tiền (Credit)
        public void Credit(decimal amount)
        {
            if (amount <= 0) throw new Exception("Số tiền nạp phải lớn hơn 0");
            Balance += amount;
        }

        // Logic nghiệp vụ: Trừ tiền (Debit)
        public void Debit(decimal amount)
        {
            if (amount <= 0) throw new Exception("Số tiền trừ phải lớn hơn 0");
            if (Balance < amount) throw new Exception("Số dư không đủ để thực hiện giao dịch"); // Quy tắc nghiệp vụ cốt lõi
            Balance -= amount;
        }
    }

}
