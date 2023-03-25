import { Component, Inject, OnInit } from '@angular/core';
import { ToolsService } from '@shared/services/tools.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';
import { GroupService } from '@core/services/group.service';
import { UserShortInfoModel } from '@shared/models/user-short-info.model';

@Component({
  selector: 'app-admin-group-change-group-teachers',
  templateUrl: './change-group-teachers.component.html',
  styleUrls: ['./change-group-teachers.component.scss'],
})
export class AdminGroupChangeGroupTeachersComponent implements OnInit {
  teachers: UserShortInfoModel[] = [];
  allTeachers: UserShortInfoModel[] = [];

  groups: { title: string; items: UserShortInfoModel[] }[] = [];

  selectedTeacherIds: string[] = [];

  isLoading = false;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminGroupChangeGroupTeachersComponent>,
    private adminService: AdminService,
    private groupService: GroupService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.update();
  }

  ngOnInit(): void {}

  update() {
    this.isLoading = true;
    this.adminService.getUsersByRole('Teacher').subscribe(data => {
      this.allTeachers = data;
      this.groups = [];
      this.groups.push({ title: 'Активні', items: data.filter(x => !x.isDeleted) });
      this.groups.push({ title: 'Деактивовані', items: data.filter(x => x.isDeleted) });

      this.groupService.getTeachersByGroupId(this.data.groupId).subscribe(teachers => {
        this.teachers = teachers;
        this.selectedTeacherIds = teachers.map(x => x.id);
        this.isLoading = false;
      });
    });
  }

  close() {
    this.dialogRef.close();
  }

  onItemSelected(id: string) {
    if (this.teachers.find(x => x.id === id)) {
      this.adminService.removeTeacherFromGroup(this.data.groupId, id).subscribe(() => {
        this.tools.showNotification('Предмет успішно видалено з групи!');
        this.update();
      });
      return;
    }

    this.adminService.addTeacherToGroup(this.data.groupId, id).subscribe(() => {
      this.tools.showNotification('Предмет успішно додано до групи!');
      this.update();
    });
  }
}
