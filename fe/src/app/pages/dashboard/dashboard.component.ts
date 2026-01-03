import { CommonModule } from '@angular/common';
import { Component, effect } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BankingService } from '../../services/banking.service';
import { Router } from '@angular/router';
import { SignalrService } from '../../services/signalr.service';
import {
  AccountDto,
  TransactionDto,
  TransferCommand,
} from '../../models/account.model';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  constructor(
    private bankingService: BankingService,
    private router: Router,
    private signalRService: SignalrService
  ) {
    effect(() => {
      // REAL-TIME MAGIC ✨
      // Dùng effect để tự động cập nhật UI khi SignalR báo có tiền mới
      const newBalance = this.signalRService.balanceSignal();
      if (this.accountInfo && newBalance !== null) {
        this.accountInfo.balance = newBalance;
        // Load lại lịch sử để thấy giao dịch vừa nhận/gửi
        this.loadHistory();
      }
    });
  }

  currentUser: string | null = null;
  accountInfo: AccountDto | null = null;
  history: TransactionDto[] = [];

  transferData: TransferCommand = {
    fromAccountNumber: '',
    toAccountNumber: '',
    amount: 0,
    message: 'Chuyen tien',
  };
  ngOnInit() {
    //Lấy số tài khoản từ bộ nhớ trình duyệt
    this.currentUser = localStorage.getItem('userAccount');
    if (!this.currentUser) {
      // Nếu chưa đăng nhập, chuyển hướng về trang login
      this.router.navigate(['/login']);
      return;
    }
    //2. Set số tài khoản gửi trong dữ liệu chuyển khoản
    this.transferData.fromAccountNumber = this.currentUser;
    //3. Load du lieu ban dau
    this.loadAccountInfo();
    this.loadHistory();
    //4. Bắt đầu kết nối SignalR
    this.signalRService.startConnection(this.currentUser);
  }

  loadAccountInfo() {
    if (this.currentUser) {
      this.bankingService.getAccount(this.currentUser).subscribe({
        next: (data) => {
          this.accountInfo = data;
        },
        error: (err) => {
          alert('Khong tim thay tai khoan! Hay kiem tra lai so tai khoan.');
          console.log(err);
        },
      });
    }
    
  }
  loadHistory() {
    if (this.currentUser) {
      this.bankingService.getHistory(this.currentUser).subscribe({
        next: (data) => {
          this.history = data;
        },
      });
    }
  
  }
  onTransfer() {
    if(confirm(`Ban co chac chan muon chuyen ${this.transferData.amount} cho ${this.transferData.toAccountNumber} khong?`)) {
      this.bankingService.transfer(this.transferData).subscribe({
        next: (data) => {
          alert('Chuyen tien thanh cong!');
          //reset form
          this.transferData.toAccountNumber = '';
          this.transferData.amount = 0;
        },
        error: (err) => {
          alert('Chuyen tien that bai! Hay kiem tra lai thong tin.');
          console.log(err);
        }
      });
    }
  }
  onLogout() {
    // Xóa số tài khoản khỏi bộ nhớ trình duyệt
    localStorage.removeItem('userAccount');
    // Chuyển hướng về trang đăng nhập
    this.router.navigate(['/login']);
  }
}