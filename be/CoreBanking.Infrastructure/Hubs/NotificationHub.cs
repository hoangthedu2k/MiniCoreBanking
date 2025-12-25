using CoreBanking.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        // Kết nối vào Hub thì gán User vào Group theo Số Tài Khoản
     // Để khi gửi tin nhắn, ta chỉ gửi cho đúng người đó thôi
        public async Task JoinRoom(string accountNumber)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, accountNumber);
        }
    }
}
