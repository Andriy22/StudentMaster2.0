import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, User } from '@core/authentication';

@Component({
  selector: 'app-user-panel',
  template: `
    <div class="matero-user-panel">
      <img
        class="matero-user-panel-avatar"
        [src]="'https://localhost:7189/static/avatars/' + user.avatar"
        alt="avatar"
        width="64"
      />
      <h4 class="matero-user-panel-name">{{ user.name }}</h4>
      <h5 class="matero-user-panel-email">{{ user.email }}</h5>
      <div class="matero-user-panel-icons">
        <button
          mat-icon-button
          routerLink="/account/profile"
          matTooltip="{{ 'edit_profile' | translate }}"
        >
          <mat-icon class="icon-18">edit</mat-icon>
        </button>
        <button mat-icon-button (click)="logout()" matTooltip="{{ 'logout' | translate }}">
          <mat-icon class="icon-18">exit_to_app</mat-icon>
        </button>
      </div>
    </div>
  `,
  styleUrls: ['./user-panel.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserPanelComponent implements OnInit {
  user!: User;

  constructor(private router: Router, private auth: AuthService) {}

  ngOnInit(): void {
    this.auth.user().subscribe(user => (this.user = user));
  }

  logout() {
    this.auth.logout().subscribe(() => this.router.navigateByUrl('/auth/login'));
  }
}
