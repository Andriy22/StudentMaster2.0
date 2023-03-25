import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { AccountService } from '@core/services/account.service';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class AccountSettingsComponent {
  isLinear = true;
  currentPassword: FormGroup;
  newPassword: FormGroup;
  error = '';
  done = '';

  constructor(private formBuilder: FormBuilder, private accountService: AccountService) {
    this.currentPassword = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
    this.newPassword = this.formBuilder.group(
      {
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirm: ['', [Validators.required, Validators.minLength(6)]],
      },
      { validator: this.checkPasswords }
    );
  }

  ngOnInit() {}

  checkPasswords(group: FormGroup) {
    const pass = group.controls.password.value;
    const confirmPass = group.controls.confirm.value;
    if (pass === confirmPass) {
      return null;
    }

    return group.controls.confirm.setErrors({ MatchPassword: true });
  }

  onSubmit(stepper: MatStepper) {
    const current = this.currentPassword.get('password')?.value;
    const newPassword = this.newPassword.get('password')?.value;
    const confirm = this.newPassword.get('confirm')?.value;

    this.accountService.changePassword(current, newPassword, confirm).subscribe(() => {
      stepper.next();
    });
  }
}
