import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountDto, TransactionDto, TransferCommand } from '../models/account.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
  
})
export class BankingService {

  constructor(private http: HttpClient) { }
  private apiUrl = 'http://localhost:5223/api'; // Thay đổi URL thành endpoint API của bạn
  getAccount(accountNumber: string): Observable<AccountDto> {
    return this.http.get<AccountDto>(`${this.apiUrl}/Accounts/${accountNumber}`);
  }
  getHistory(accountNumber: string): Observable<TransactionDto[]> 
  {
    return this.http.get<TransactionDto[]>(`${this.apiUrl}/Transactions/history/${accountNumber}`);
  }

  transfer(command: TransferCommand): Observable<any> {
    return this.http.post(`${this.apiUrl}/Transactions/transfer`, command);
  }

}