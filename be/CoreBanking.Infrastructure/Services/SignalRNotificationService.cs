using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Infrastructure.Services
{
    public class SignalRNotificationService : INotificationService
    {

        private readonly IHubContext<NotificationHub> _hubContext;
        public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendBalanceUpdate(string accountNumber, decimal newBalance)
        {
            await _hubContext.Clients.Group(accountNumber)
                .SendAsync("ReceiveBalanceUpdate", newBalance); 
        }
    }
}
