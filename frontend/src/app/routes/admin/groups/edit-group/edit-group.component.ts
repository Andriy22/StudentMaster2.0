import { Component, Inject, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ToolsService } from '@shared/services/tools.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';

@Component({
  selector: 'app-admin-groups-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.scss'],
})
export class AdminGroupsEditGroupComponent implements OnDestroy {
  isLoading = false;

  name = '';

  $subscription: Subscription | undefined;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminGroupsEditGroupComponent>,
    private adminService: AdminService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.name = data.groupName;
  }

  cancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.isLoading = true;

    this.$subscription = this.adminService.changeGroupName(this.data.groupId, this.name).subscribe(
      () => {
        this.dialogRef.close();
        this.tools.showNotification('Назву групи успішно змінено!');
        this.isLoading = false;
      },
      () => (this.isLoading = false)
    );
  }

  ngOnDestroy(): void {
    this.$subscription?.unsubscribe();
  }
}
