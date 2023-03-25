import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { MatPaginator } from '@angular/material/paginator';
import { TeacherService } from '@core/services/teacher.service';
import { GroupService } from '@core/services/group.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import * as moment from 'moment';

@Component({
  selector: 'app-teacher-group-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.scss'],
})
export class TeacherGroupAttendanceComponent implements OnInit, AfterViewInit {
  dataSource: MatTableDataSource<any>;
  displayedColumns = ['fullName', 'isPresent'];

  isLoading = false;

  subjects: SubjectInfoModel[] = [];
  selectedSubjectId = -1;

  date = new Date().toISOString();

  @Input() groupId = -1;
  @ViewChild(MatPaginator)
  paginator: MatPaginator | undefined;

  constructor(private teacherService: TeacherService, private groupService: GroupService) {
    this.dataSource = new MatTableDataSource<any>([]);
  }

  loadAttendance() {
    this.teacherService
      .getStudentAttendance(
        this.groupId,
        this.selectedSubjectId,
        moment(this.date).format('YYYY-MM-DD')
      )
      .subscribe(data => {
        this.dataSource.data = data;
      });
  }

  ngOnInit() {
    this.groupService.getGroupSubjects(this.groupId).subscribe(subjects => {
      this.subjects = subjects;

      if (subjects.length > 0) {
        this.selectedSubjectId = subjects[0].id;
        this.loadAttendance();
      }
    });
  }

  ngAfterViewInit() {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  onSubjectChange(event: any) {
    setTimeout(() => this.loadAttendance(), 0);
  }

  onChange(event: MatCheckboxChange, id: string) {
    this.teacherService
      .setStudentAttendance(
        id,
        this.selectedSubjectId,
        moment(this.date).format('YYYY-MM-DD'),
        event.checked
      )
      .subscribe(
        _ => {
          this.loadAttendance();
        },
        _ => {
          this.loadAttendance();
        }
      );
  }

  onDataChange(event: MatDatepickerInputEvent<any>) {
    if (event.value) {
      this.date = event.value?.toISOString();
    }

    setTimeout(() => this.loadAttendance(), 0);
  }
}
