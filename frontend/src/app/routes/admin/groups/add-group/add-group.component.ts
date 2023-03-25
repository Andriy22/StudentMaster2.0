import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ToolsService } from '@shared/services/tools.service';
import { MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';

@Component({
  selector: 'app-admin-groups-add-group',
  templateUrl: './add-group.component.html',
  styleUrls: ['./add-group.component.scss'],
})
export class AdminGroupsAddGroupComponent implements OnDestroy {
  isLoading = false;

  name = '';

  $subscription: Subscription | undefined;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminGroupsAddGroupComponent>,
    private adminService: AdminService
  ) {}

  cancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.isLoading = true;

    this.$subscription = this.adminService.createGroup(this.name).subscribe(
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
