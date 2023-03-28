import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { TeacherService } from '@core/services/teacher.service';
import { TeacherRegisterDataModel } from '@shared/models/teacher-register-data.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { TeacherRegisterAddWorkComponent } from './add-work/add-work.component';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { ToolsService } from '@shared/services/tools.service';
import { StudentAttendanceComponent } from '@shared/components/student-attendance/student-attendance.component';

@Component({
  selector: 'app-teacher-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class TeacherRegisterComponent implements OnInit {
  data: TeacherRegisterDataModel[][] = [];

  displayedColumns: string[] = [];
  columnsSchema: { key: string; title: string }[] = [];
  dataSource: MatTableDataSource<any>;
  groups: { key?: string; header: string; colSpan: number }[] = [];
  groupedColumns: string[] = [];

  filteredColumns: string[] = [];

  allWorks: string[] = [];
  allColumns: string[] = [];

  isExtended = false;
  isEdit = false;

  isLoading = false;

  subjects: SubjectInfoModel[] = [];
  selectedSubjectId = -1;

  @Input() groupId = -1;

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;

  constructor(
    private teacherService: TeacherService,
    public dialog: MatDialog,
    private tools: ToolsService
  ) {
    this.dataSource = new MatTableDataSource();
  }

  render(data: TeacherRegisterDataModel[][]) {
    data.forEach(row => {
      const obj: { [key: string]: string } = {};
      row.forEach(el => {
        if (!this.filteredColumns.includes(el.header)) {
          return;
        }
        el.items.forEach(item => {
          if (!this.columnsSchema.find(x => x.key === item.name)) {
            this.columnsSchema.push({
              key: item.name,
              title: item.title,
            });
          }
          obj[item.name] = item.value;

          obj[`${item.name}_editable`] = String(item.editable);
          if (item?.id != null) {
            obj[`${item.name}_id`] = item.id;
          }
          obj[`${item.name}_limit`] = item.limit;
        });

        if (!this.groups.find(x => x.header === el.header)) {
          this.groups.push({ key: el.name, header: el.header, colSpan: el.items.length });
        }
      });
      this.dataSource.data.push(obj);
    });

    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }

    this.allColumns = this.columnsSchema.map(col => col.key);
    this.rerender();
  }

  loadRegisterData() {
    this.isLoading = true;
    this.teacherService
      .getRegisterData(this.groupId, this.selectedSubjectId, this.isExtended)
      .subscribe(
        data => {
          this.data = data;
          if (this.filteredColumns.length == 0) {
            this.allWorks = [];
            data.forEach(el => {
              el.forEach(item => {
                this.filteredColumns.push(item.header);

                if (!this.allWorks.includes(item.header)) this.allWorks.push(item.header);
              });
            });
          }

          this.render(data);

          this.isLoading = false;
        },
        _ => (this.isLoading = false)
      );
  }

  ngOnInit() {
    this.teacherService.getTeacherSubjectsInGroup(this.groupId).subscribe(subjects => {
      this.subjects = subjects;

      if (subjects.length > 0) {
        this.selectedSubjectId = subjects[0].id;

        this.loadRegisterData();
      }
    });
  }

  onExtendChange(event: MatSlideToggleChange) {
    this.reset();
    this.loadRegisterData();
  }

  onSubjectChange(event: any) {
    this.reset();

    this.filteredColumns = [];

    setTimeout(_ => {
      this.loadRegisterData();
    }, 0);
  }

  applyFilter(event: any[]) {
    this.reset();

    setTimeout(() => {
      this.render(this.data);
    }, 0);
  }

  onMarkChange(mark: string, colId: string, uId: string) {
    this.teacherService.addMark(colId, uId, mark).subscribe(
      _ => {
        this.tools.showNotification('Оцінку успішно додано!');
      },
      _ => {
        this.reset();
        this.loadRegisterData();
      }
    );
  }

  addWork() {
    this.teacherService.getPresets(this.groupId, this.selectedSubjectId).subscribe(presets => {
      const dialogRef = this.dialog
        .open(TeacherRegisterAddWorkComponent, {
          width: '90%',
          data: { subjectId: this.selectedSubjectId, groupId: this.groupId, presets },
        })
        .afterClosed()
        .subscribe(_ => {
          this.reset();

          this.filteredColumns = [];

          this.loadRegisterData();
        });
    });
  }

  showVisiting($event: MouseEvent, key: string, id: string) {
    const dialogRef = this.dialog
      .open(StudentAttendanceComponent, {
        width: '50%',
        data: { subjectId: this.selectedSubjectId, studentId: id },
      })
      .afterClosed();
  }

  private reset() {
    this.displayedColumns = [];
    this.columnsSchema = [];
    this.dataSource.data = [];
    this.groups = [];
    this.groupedColumns = [];
  }

  private rerender() {
    this.displayedColumns = this.columnsSchema.map(col => col.key);
    this.groupedColumns = this.groups.map(col => col.header);
  }
}
