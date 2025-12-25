using CoreBanking.Application.Common.DTOs;
using CoreBanking.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Features.Accounts.Queries
{
    //1. Định nghĩa Query
    public record GetAccountByNumberQuery(string AccountNumber) : IRequest<AccountDto>;
    //2. Định nghĩa Handler
    public class GetAccountByNumberQueryHandler : IRequestHandler<GetAccountByNumberQuery, AccountDto>
    {
        private readonly IApplicationDbContext _context;
        public GetAccountByNumberQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<AccountDto> Handle(GetAccountByNumberQuery request, CancellationToken cancellationToken)
        {
           var account = await _context.Accounts.Include(x => x.Customer).AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber, cancellationToken);
            if (account == null)
                throw new Exception("Tài khoản không tồn tại.");
            // Map entity to DTO
            var accountDto = new AccountDto
            {
                AccountNumber = account.AccountNumber,
               
                Balance = account.Balance,
                Status = account.Status,
                Currency = account.Currency,
                FullName = account.Customer?.FullName??"Unknown",

            };
            return accountDto;
        }
    }
}
