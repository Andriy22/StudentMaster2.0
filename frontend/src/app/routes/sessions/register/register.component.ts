import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '@core/services/account.service';
import { Router } from '@angular/router';
import { ToolsService } from '@shared/services/tools.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm = this.fb.group(
    {
      confirmationCode: ['', [Validators.required]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
    },
    {
      validators: [this.matchValidator('password', 'confirmPassword')],
    }
  );

  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private tools: ToolsService
  ) {}

  submit() {
    this.isLoading = true;
    this.accountService
      .confirmAccount(
        Number.parseInt(<string>this.registerForm.get('confirmationCode')?.value),
        <string>this.registerForm.get('password')?.value,
        <string>this.registerForm.get('confirmPassword')?.value
      )
      .subscribe(
        () => {
          (this.isLoading = false), this.router.navigate(['/']);
          this.tools.showNotification('Ваш аккаунт підтверджено!');
        },
        () => {
          this.isLoading = false;
        }
      );
  }

  matchValidator(source: string, target: string) {
    return (control: AbstractControl) => {
      const sourceControl = control.get(source)!;
      const targetControl = control.get(target)!;
      if (targetControl.errors && !targetControl.errors.mismatch) {
        return null;
      }
      if (sourceControl.value !== targetControl.value) {
        targetControl.setErrors({ mismatch: true });
        return { mismatch: true };
      } else {
        targetControl.setErrors(null);
        return null;
      }
    };
  }
}
