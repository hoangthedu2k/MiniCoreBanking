import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
private hubConnection: signalR.HubConnection; 

public balanceSignal = signal<number | null>(null);
  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5223/notificationHub') // Thay đổi URL thành endpoint của bạn
      .withAutomaticReconnect()
      .build();
  }
public startConnection(accountNumber: string) {
    this.hubConnection
      .start()
      .then(() => { 
        console.log('SignalR Connected.');
        this.hubConnection.invoke('JoinRoom', accountNumber);
      })
      .catch(err => console.log('Error while starting connection: ' + err));
      this.hubConnection.on('ReceiveBalanceUpdate', (newBalance: number) => {
        console.log(`Balance updated: ${newBalance}`);
        // Xử lý cập nhật số dư ở đây, ví dụ: cập nhật UI hoặc thông báo người dùng
        this.balanceSignal.set(newBalance);
      });
  } 
}
