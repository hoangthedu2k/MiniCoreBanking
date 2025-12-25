using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Application.Features.Transactions.Commands;

// 1. COMMAND: Dữ liệu người dùng gửi lên
public record TransferMoneyCommand(
    string FromAccountNumber, 
    string ToAccountNumber, 
    decimal Amount, 
    string Message
) : IRequest<string>; // Trả về TransactionId

// 2. HANDLER: Xử lý logic
public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, string>
{
    private readonly IApplicationDbContext _context;

    public TransferMoneyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        // --- 1. KIỂM TRA DỮ LIỆU (VALIDATION) ---
        
        // Tìm tài khoản nguồn
        var fromAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccountNumber, cancellationToken);

        if (fromAccount == null)
            throw new Exception("Tài khoản nguồn không tồn tại.");

        // Tìm tài khoản đích
        var toAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountNumber, cancellationToken);

        if (toAccount == null)
            throw new Exception("Tài khoản nhận không tồn tại.");

        // --- 2. THỰC HIỆN NGHIỆP VỤ (DOMAIN LOGIC) ---
        // Gọi hàm trong Domain Entity để đảm bảo quy tắc nghiệp vụ (không bị âm tiền)
        // EF Core sẽ tự theo dõi sự thay đổi này
        fromAccount.Debit(request.Amount); 
        toAccount.Credit(request.Amount);

        // --- 3. LƯU LỊCH SỬ GIAO DỊCH (AUDIT) ---
        var transaction = new Transaction
        {
            FromAccount = fromAccount.AccountNumber,
            ToAccount = toAccount.AccountNumber,
            Amount = request.Amount,
            TransactionType = TransactionType.Transfer,
            Message = request.Message,
            Status = TransactionStatus.Success,
            TransactionDate = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);

        // --- 4. LƯU XUỐNG DB (COMMIT) ---
        // SaveChangesAsync trong EF Core mặc định là 1 Transaction Database.
        // Nếu dòng này lỗi -> Cả Debit, Credit, và Transaction đều bị Rollback (Hủy) hết.
        // Tiền sẽ an toàn 100%.
        await _context.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}