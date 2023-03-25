import { Component, OnInit } from '@angular/core';
import { ToolsService } from '@shared/services/tools.service';
import { AuthService, TokenService } from '@core';
import { ChatMessageModel } from '@shared/models/chat-message.model';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '@env/environment';

@Component({
  selector: 'app-admin-console',
  templateUrl: './console.component.html',
  styleUrls: ['./console.component.scss'],
})
export class AdminConsoleComponent implements OnInit {
  messages: ChatMessageModel[] = [];
  message = '/help';
  isLoading = false;

  jwtToken = '';

  private _hubConnection: HubConnection | undefined;

  constructor(
    private tools: ToolsService,
    private authService: AuthService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.connect();
  }

  connect() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.apiUrl + '/live/console', {
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: () => {
          return this.tokenService.getBearerToken();
        },
      })
      .build();

    this._hubConnection
      .start()
      .then(() => {
        this.Send();
        this.tools.showNotification('Connected to @console');
      })
      .catch(err => {
        this.tools.showNotification('Error while starting connection');
        console.log('Error while starting connection: ' + err);
      });
    this._hubConnection?.on('receiveMessage', (message: ChatMessageModel) => {
      this.messages.push(message);
      this.goDown();
    });
  }

  Send() {
    this._hubConnection?.invoke('Execute', this.message).catch(e => this.Handler(e));
    this.message = '';
  }

  goDown() {
    const chatElement = document.querySelector('.chat');
    setTimeout(() => {
      if (chatElement) {
        chatElement.scrollTop = chatElement.scrollHeight;
      }
    }, 0);
  }

  saveCommand(cmd: string) {
    this.message = cmd.split(' ')[0];
  }

  Handler(err: any) {
    const error = err.message.toString();

    console.log(error);

    if (error.indexOf('because user is unauthorized') !== -1) {
      this._hubConnection?.stop();
      this.authService.refresh().subscribe(() => {
        this.connect();
      });
    }
  }

  Clear() {
    this.messages = [];
  }

  Help() {
    this._hubConnection?.invoke('Execute', '/help').catch(e => this.Handler(e));
    this.isLoading = true;
    setTimeout(() => {
      this.isLoading = false;
    }, 5000);
  }
}
