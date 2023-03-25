import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';
import { ToolsService } from '@shared/services/tools.service';
import { FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss'],
})
export class CreateUserComponent {
  isLoading = false;

  creatingUserSub: Subscription | undefined;

  createUserForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    name: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    role: ['', [Validators.required]],
  });

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<CreateUserComponent>,
    private adminService: AdminService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  get firstName() {
    return this.createUserForm.get('firstName')!;
  }

  get name() {
    return this.createUserForm.get('name')!;
  }

  get lastName() {
    return this.createUserForm.get('lastName')!;
  }

  get email() {
    return this.createUserForm.get('email')!;
  }

  get role() {
    return this.createUserForm.get('role')!;
  }

  ngOnInit(): void {}

  cancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.isLoading = true;
    console.log(this.firstName.value);
    this.creatingUserSub = this.adminService
      .createUser({
        firstName: this.firstName.value,
        name: this.name.value,
        lastName: this.lastName.value,
        email: this.email.value,
        role: this.role.value,
      })
      .subscribe(
        () => {
          this.tools.showNotification('Користувача успішно створено!');
          this.isLoading = false;
          this.dialogRef.close();
        },
        () => {
          this.isLoading = false;
        }
      );
  }
}
