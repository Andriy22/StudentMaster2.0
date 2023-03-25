import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService, User } from '@core';
import { Router } from '@angular/router';
import { debounceTime, switchMap, tap } from 'rxjs/operators';
import { AccountService } from '@core/services/account.service';
import { ToolsService } from '@shared/services/tools.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-account-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class AccountProfileComponent implements OnInit, OnDestroy {
  user!: User;

  userSub: Subscription | undefined;
  updateUserSub: Subscription | undefined;

  constructor(
    private router: Router,
    private auth: AuthService,
    private cdr: ChangeDetectorRef,
    private account: AccountService,
    private tools: ToolsService
  ) {}

  ngOnDestroy(): void {
    this.updateUserSub?.unsubscribe();
    this.userSub?.unsubscribe();
  }

  ngOnInit(): void {
    this.userSub = this.auth
      .user()
      .pipe(
        tap(user => (this.user = user)),
        debounceTime(10)
      )
      .subscribe(() => this.cdr.detectChanges());
  }

  changePhoto() {
    const selector = document.getElementById('photoSelector') as HTMLInputElement;
    selector.click();

    selector.onchange = event => {
      if (event == null) {
        return;
      }

      const target = event.target as HTMLInputElement;
      if (target == null) {
        return;
      }

      const files = target.files;
      if (files === null) {
        return;
      }

      if (files[0]) {
        const data = new FormData();
        data.append('file', files[0]);

        this.updateUserSub = this.account
          .changeAvatar(data)
          .pipe(
            switchMap(_ => {
              this.tools.showNotification('Ваш аватар змінено!');
              return this.auth.updateUser();
            })
          )
          .subscribe();
      }
    };
  }
}
