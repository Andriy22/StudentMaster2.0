import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { filter } from 'rxjs/operators';
import { AuthService } from '@core/authentication';
import { NgxPermissionsService } from 'ngx-permissions';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy {
  isSubmitting = false;

  loginSub: Subscription | undefined;

  loginForm = this.fb.nonNullable.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private permissionsService: NgxPermissionsService
  ) {}

  get username() {
    return this.loginForm.get('username')!;
  }

  get password() {
    return this.loginForm.get('password')!;
  }

  login() {
    this.isSubmitting = true;
    this.auth
      .login(this.username.value, this.password.value, false)
      .pipe(filter(authenticated => authenticated))
      .subscribe(
        () => {
          this.router.navigateByUrl('/');
          this.permissionsService.flushPermissions();
          this.loginSub = this.auth.user().subscribe(user => {
            const roles: string[] = [];

            user?.roles?.forEach((role: string) => {
              roles.push(role);
            });

            this.permissionsService.loadPermissions(roles);
          });
        },
        (errorRes: HttpErrorResponse) => {
          if (errorRes.status === 422) {
            const form = this.loginForm;
            const errors = errorRes.error.errors;
            Object.keys(errors).forEach(key => {
              form.get(key === 'email' ? 'username' : key)?.setErrors({
                remote: errors[key][0],
              });
            });
          }
          this.isSubmitting = false;
        }
      );
  }

  ngOnDestroy(): void {
    this.loginSub?.unsubscribe();
  }
}
