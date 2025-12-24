using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Enums;
using MediatR;

namespace CoreBanking.Application.Features.Accounts.Commands;

// 1. COMMAND: Dữ liệu API gửi lên (giống DTO)
// Trả về string (là AccountNumber vừa tạo)
public record CreateAccountCommand(string FullName, string Email, string Phone) : IRequest<string>;

// 2. HANDLER: Logic xử lý
public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // Bước 1: Tạo Customer (Khách hàng)
        var customer = new Customer
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = "hashed_password_demo" // Thực tế phải hash password
        };

        // Bước 2: Tạo Account (Tài khoản thanh toán mặc định)
        var account = new Account
        {
            AccountNumber = GenerateAccountNumber(), // Hàm tự chế ở dưới
            Customer = customer, // EF Core tự hiểu là tạo cả Customer lẫn Account
            AccountType = AccountType.Payment,
            Status = "ACTIVE"
        };

        // Bước 3: Lưu vào DB
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return account.AccountNumber;
    }

    // Hàm sinh số tài khoản ngẫu nhiên 10 số
    private string GenerateAccountNumber()
    {
        var random = new Random();
        return "10" + random.Next(10000000, 99999999).ToString();
    }
}