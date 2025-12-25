namespace CoreBanking.Application.Common.DTOs;

public class AccountDto
{
    public string AccountNumber { get; set; }
    public string FullName { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
}

public class TransactionDto
{
    public string Id { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // Deposit, Transfer...
    public string Status { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; }
}