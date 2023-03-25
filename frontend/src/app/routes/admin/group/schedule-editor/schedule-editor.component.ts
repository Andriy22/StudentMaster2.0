import { Component, Inject, OnInit } from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import { ScheduleService } from '@core/services/schedule.service';
import { ToolsService } from '@shared/services/tools.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';
import { GroupService } from '@core/services/group.service';
import {
  ScheduleDayViewModel,
  ScheduleItemTypeViewModel,
  ScheduleItemViewModel,
} from '@shared/models/schedule.models';
import { SubjectShortInfoModel } from '@shared/models/subject-short-info.model';
import { MatTableDataSource } from '@angular/material/table';
import { MtxDialog } from '@ng-matero/extensions/dialog';

@Component({
  selector: 'app-admin-group-schedule-editor',
  templateUrl: './schedule-editor.component.html',
  styleUrls: ['./schedule-editor.component.scss'],
})
export class AdminGroupScheduleEditorComponent implements OnInit {
  displayedColumns: string[] = ['subject', 'type', 'start', 'end', 'url', 'comment', 'action'];

  dataSource = new MatTableDataSource<ScheduleItemViewModel>();

  selectedDay = -1;

  days: ScheduleDayViewModel[] = [];
  subjects: SubjectShortInfoModel[] = [];
  types: ScheduleItemTypeViewModel[] = [];

  isLoading = false;

  timeRegex = /^(?:[01][0-9]|2[0-3]):[0-5][0-9](?::[0-5][0-9])?$/;

  constructor(
    public schedule: ScheduleService,
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminGroupScheduleEditorComponent>,
    private adminService: AdminService,
    private groupService: GroupService,
    private mtxDialog: MtxDialog,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  fetchData() {
    this.schedule.getScheduleDays().subscribe(days => {
      this.days = days;

      this.selectedDay = this.days[0].id;

      this.fetchRows();
    });

    this.groupService.getGroupSubjects(this.data.groupId).subscribe(subjects => {
      this.subjects = subjects;
    });

    this.schedule.getScheduleItemTypes().subscribe(types => {
      this.types = types;
    });
  }

  fetchRows() {
    this.isLoading = true;
    this.schedule.getScheduleItemsByGroupAndDay(this.data.groupId, this.selectedDay).subscribe(
      data => {
        this.dataSource.data = data;
        this.isLoading = false;
      },
      _ => (this.isLoading = false)
    );
  }

  ngOnInit() {
    this.fetchData();
  }

  onDayChange(event: MatSelectChange) {
    this.fetchRows();
  }

  saveItem(element: ScheduleItemViewModel) {
    this.schedule
      .updateScheduleItem(
        element.id,
        element.subjectShortInfo.id,
        element.scheduleItemType.id,
        element.url,
        element.start,
        element.end,
        element.comment
      )
      .subscribe(_ => {
        this.fetchRows();
      });
  }

  addNewRow() {
    this.schedule.addNewScheduleItem(this.data.groupId, this.selectedDay).subscribe(_ => {
      this.fetchRows();
    });
  }

  deleteRow(id: number) {
    this.mtxDialog.confirm('Підтвердження', 'Ви дійсно хочете видалити цей рядок?', () => {
      this.schedule.removeScheduleItem(id).subscribe(_ => {
        this.fetchRows();
      });
    });
  }

  saveItems() {
    this.dataSource.data.forEach(element => {
      this.schedule
        .updateScheduleItem(
          element.id,
          element.subjectShortInfo.id,
          element.scheduleItemType.id,
          element.url,
          element.start,
          element.end,
          element.comment
        )
        .subscribe(_ => {
          this.fetchRows();
        });
    });
  }
}
