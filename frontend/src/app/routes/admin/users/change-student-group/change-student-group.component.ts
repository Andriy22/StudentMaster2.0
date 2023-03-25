import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { ToolsService } from '@shared/services/tools.service';
import { AdminService } from '@core/services/admin.service';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-admin-users-change-student-group',
  templateUrl: './change-student-group.component.html',
  styleUrls: ['./change-student-group.component.scss'],
})
export class AdminUsersChangeStudentGroupComponent implements OnInit {
  groups: GroupInfoModel[] = [];
  selectedGroup = -1;
  isLoading = false;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminUsersChangeStudentGroupComponent>,
    private adminService: AdminService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.fetchData();
  }

  ngOnInit(): void {}

  cancel() {
    this.dialogRef.close();
  }

  onChange(event: MatSelectChange) {
    this.isLoading = true;
    this.adminService.changeStudentGroup(event.value, this.data.studentId).subscribe(
      () => {
        this.isLoading = false;
        this.tools.showNotification('Групу успішно змінено!');
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  fetchData() {
    this.isLoading = true;
    this.adminService.getGroups().subscribe(
      groups => {
        this.groups = groups;

        this.adminService.getGroupByStudentId(this.data.studentId).subscribe(
          group => {
            this.selectedGroup = group.id;
            this.isLoading = false;
          },
          () => {
            this.isLoading = false;
          }
        );
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  close() {
    this.dialogRef.close();
  }
}
