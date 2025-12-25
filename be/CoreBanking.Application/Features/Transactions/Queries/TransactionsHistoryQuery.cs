using CoreBanking.Application.Common.DTOs;
using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Features.Transactions.Queries
{
    public record GetTransactionsHistoryQuery(string AccountNumber) : IRequest<List<TransactionDto>>;
    public class GetTransactionsHistoryQueryHandler : IRequestHandler<GetTransactionsHistoryQuery, List<TransactionDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionsHistoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TransactionDto>> Handle(GetTransactionsHistoryQuery request, CancellationToken cancellationToken)
        {

           var history = await _context.Transactions
                .Where(t => t.FromAccount == request.AccountNumber || t.ToAccount ==request.AccountNumber)
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    FromAccount = t.FromAccount,
                    ToAccount = t.ToAccount,
                    Amount = t.Amount,
                    Type = t.TransactionType.ToString(),
                    Status = t.Status.ToString(),
                    Date = t.TransactionDate,
                    Message = t.Message
                   
                }).ToListAsync();
            return history;
        }
    }
}
