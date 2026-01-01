export interface AccountDto {
  accountNumber: string;
  fullName: string;
  balance: number;
  currency: string;
  status: string;
}

export interface TransactionDto {
  id: string;
  fromAccount: string;
  toAccount: string;
  amount: number;
  type: string;
  status: string;
  date: string;
  message: string;
}

// Dùng cho form chuyển tiền
export interface TransferCommand {
  fromAccountNumber: string;
  toAccountNumber: string;
  amount: number;
  message: string;
}