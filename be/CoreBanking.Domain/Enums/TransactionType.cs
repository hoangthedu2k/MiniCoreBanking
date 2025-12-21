using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Domain.Enums
{
    public enum TransactionType
    {
        Deposit,    // Nạp tiền
        Withdraw,   // Rút tiền
        Transfer,   // Chuyển khoản
        Interest    // Trả lãi
    }

}
