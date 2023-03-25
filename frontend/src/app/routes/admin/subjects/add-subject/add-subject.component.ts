import { Component, OnDestroy } from '@angular/core';
import { ToolsService } from '@shared/services/tools.service';
import { MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-admin-subjects-add-subject',
  templateUrl: './add-subject.component.html',
  styleUrls: ['./add-subject.component.scss'],
})
export class AdminSubjectsAddSubjectComponent implements OnDestroy {
  isLoading = false;

  name = '';

  $subscription: Subscription | undefined;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminSubjectsAddSubjectComponent>,
    private adminService: AdminService
  ) {}

  cancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.isLoading = true;

    this.$subscription = this.adminService.createSubject(this.name).subscribe(
      () => {
        this.dialogRef.close();
        this.isLoading = false;
      },
      () => (this.isLoading = false)
    );
  }

  ngOnDestroy(): void {
    this.$subscription?.unsubscribe();
  }
}
