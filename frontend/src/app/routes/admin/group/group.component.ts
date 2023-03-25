import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GroupService } from '@core/services/group.service';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { UserInfoModel } from '@shared/models/user-info.model';
import { MatDialog } from '@angular/material/dialog';
import { AdminGroupChangeGroupSubjectsComponent } from './change-group-subjects/change-group-subjects.component';
import { AdminGroupChangeGroupTeachersComponent } from './change-group-teachers/change-group-teachers.component';
import { AdminService } from '@core/services/admin.service';
import { AdminGroupScheduleEditorComponent } from './schedule-editor/schedule-editor.component';

@Component({
  selector: 'app-admin-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class AdminGroupComponent implements OnInit {
  displayedColumns: string[] = ['fullName', 'actions'];
  dataSource: UserInfoModel[] = [];
  group: GroupInfoModel | undefined;

  groupId = -1;

  constructor(
    route: ActivatedRoute,
    public dialog: MatDialog,
    private groupService: GroupService,
    private adminService: AdminService
  ) {
    route.params.subscribe(params => {
      this.groupId = params.id;
      this.groupService.getGroupInfoById(params.id).subscribe(group => {
        this.group = group;
      });

      this.groupService.getStudentsByGroupId(params.id).subscribe(students => {
        this.dataSource = students;
      });
    });
  }

  ngOnInit() {}

  changeGroupSubjects(id: number | undefined) {
    if (id) {
      const dialogRef = this.dialog.open(AdminGroupChangeGroupSubjectsComponent, {
        width: '90%',
        data: { groupId: id },
      });
      dialogRef.afterClosed();
    }
  }

  removeStudentFromGroup(id: string) {
    let groupId = -1;

    if (!this.group) {
      return;
    }
    groupId = this.group.id;

    this.adminService.removeStudentFromGroup(groupId, id).subscribe(_ => {
      this.groupService.getStudentsByGroupId(groupId).subscribe(students => {
        this.dataSource = students;
      });
    });
  }

  changeGroupTeachers(id: number | undefined) {
    if (id) {
      const dialogRef = this.dialog.open(AdminGroupChangeGroupTeachersComponent, {
        width: '90%',
        data: { groupId: id },
      });
      dialogRef.afterClosed();
    }
  }

  editSchedule(id: number | undefined) {
    if (id) {
      const dialogRef = this.dialog.open(AdminGroupScheduleEditorComponent, {
        width: '100%',
        data: { groupId: id },
      });
      dialogRef.afterClosed();
    }
  }
}
