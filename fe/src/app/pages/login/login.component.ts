import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  accountNumber: string = '';
  constructor(private router: Router) {}
  onLogin() {
    // Perform login logic here (e.g., authentication)
    if (this.accountNumber) {
      // Lưu số tài khoản vào bộ nhớ trình duyệt
      localStorage.setItem('userAccount', this.accountNumber);
      // Chuyển hướng sang Dashboard
      this.router.navigate(['/']);
    }
  }
}
