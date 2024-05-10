import { Component, Input } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '@env/environment';
import { ToolsService } from '@shared/services/tools.service';
import { AuthService, TokenService } from '@core';
import { Router } from '@angular/router';
import { ChatMessage } from '@shared/models/ChatMessage.models';

@Component({
  selector: 'app-group-chat',
  templateUrl: './group-chat.component.html',
  styleUrls: ['./group-chat.component.scss'],
})
export class GroupChatComponent {
  messages: ChatMessage[] = [];
  message = '';
  isLoading = false;

  @Input('group') selectedGroup: number | undefined;
  private _hubConnection: HubConnection | undefined;

  constructor(
    private tools: ToolsService,
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.connect();
  }

  connect() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/live/chat`, {
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: () => {
          return this.tokenService.getAccessToken();
        },
      })
      .build();

    this._hubConnection
      ?.start()
      .then(() => {
        this._hubConnection
          ?.invoke(
            'SwitchGroup',
            parseInt(this.selectedGroup ? this.selectedGroup.toString() : '-1')
          )
          .then(() => {
            this._hubConnection
              ?.invoke(
                'SendAllMessages',
                parseInt(this.selectedGroup ? this.selectedGroup.toString() : '-1')
              )
              .catch(e => this.Handler(e));
          });

        this.tools.showNotification('Connected to the chat');
      })
      .catch(err => {
        this.tools.showNotification('Error while starting connection');
        console.log('Error while starting connection: ' + err);
      });
    this._hubConnection?.on('ReceiveAllMessages', (messages: ChatMessage[]) => {
      this.messages = messages;
      console.log(this.messages);
      this.goDown();
    });
    this._hubConnection?.on('ReceiveMessageGroup', (message: ChatMessage) => {
      this.messages.push(message);

      this.goDown();
    });
    this._hubConnection?.on('ReceiveMessage', (message: ChatMessage) => {
      this.messages.push(message);
      this.goDown();
    });
  }

  Send() {
    if (this.message.indexOf('/ban') !== -1) {
      const args = this.message.split(' ');

      this._hubConnection?.invoke('BanUser', args[1], args[2], args[3]).catch(e => this.Handler(e));

      setTimeout(() => {
        this._hubConnection?.invoke(
          'SendAllMessages',
          parseInt(this.selectedGroup ? this.selectedGroup.toString() : '-1')
        );
      }, 2000);
      this.message = '';
      return;
    }

    if (this.message.indexOf('/unban') !== -1) {
      const args = this.message.split(' ');

      this._hubConnection?.invoke('UnBanUser', args[1]).catch(e => this.Handler(e));

      setTimeout(() => {
        this._hubConnection?.invoke(
          'SendAllMessages',
          parseInt(this.selectedGroup ? this.selectedGroup.toString() : '-1')
        );
      }, 2000);

      this.message = '';
      return;
    }

    this._hubConnection
      ?.invoke(
        'SendMessage',
        this.message,
        parseInt(this.selectedGroup ? this.selectedGroup.toString() : '-1')
      )
      .catch(e => this.Handler(e));
    this.message = '';
  }

  goDown() {
    const chatElement = document.querySelector('.chat');
    setTimeout(() => {
      if (chatElement) chatElement.scrollTop = chatElement.scrollHeight;
    }, 0);
  }

  ban(uid: string) {
    this.message = '/ban ' + uid + ' minutes reason';
  }

  Handler(err: { message: { toString: () => any } }) {
    const error = err.message.toString();

    console.error(error);

    if (error.indexOf('because user is unauthorized') !== -1) {
      this._hubConnection?.stop();

      this.tools.showNotification('Будь-ласка перезайдіть...');
      this.router.navigate(['auth/login']);
    }
  }
}
