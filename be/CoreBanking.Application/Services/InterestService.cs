using CoreBanking.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Services
{
    public interface IInterestService
    {
        Task CalculateDailyInterest();
    }
    public class InterestService : IInterestService
    {
        private readonly IApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        public InterestService(IApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task CalculateDailyInterest()
        {
            var accounts = await _context.Accounts.Where(a => a.AccountType == Domain.Enums.AccountType.Payment && a.Status == "ACTIVE" && a.Balance > 0).ToListAsync(CancellationToken.None);
            foreach (var acc in accounts)
            {
                // Giả sử lãi suất hàng năm là 3%
                decimal annualInterestRate = 0.01m;
                decimal dailyInterest = acc.Balance * (annualInterestRate / 365);
                if (dailyInterest > 0)
                {
                    acc.Credit(dailyInterest);
                    // Gửi thông báo cho khách hàng
                    await _notificationService.SendBalanceUpdate(acc.AccountNumber, acc.Balance);




                }
            }
            await _context.SaveChangesAsync(CancellationToken.None);
            Console.WriteLine($"Đã trả lãi cho {accounts.Count} khách hàng lúc {DateTime.Now}");
        }
    }
}
