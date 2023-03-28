import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { ToolsService } from '@shared/services/tools.service';
import { StudentAttendanceComponent } from '@shared/components/student-attendance/student-attendance.component';
import { StudentService } from '@core/services/student.service';
import { SubjectShortInfoModel } from '@shared/models/subject-short-info.model';
import { StudentRegisterDataModel } from '@shared/models/student-register-data.model';

@Component({
  selector: 'app-student-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class StudentRegisterComponent implements OnInit {
  data: StudentRegisterDataModel[][] = [];

  displayedColumns: string[] = [];
  columnsSchema: { key: string; title: string }[] = [];
  dataSource: MatTableDataSource<any>;
  groups: { key?: string; header: string; colSpan: number }[] = [];
  groupedColumns: string[] = [];

  filteredColumns: string[] = [];

  allWorks: string[] = [];
  allColumns: string[] = [];

  isExtended = false;

  isLoading = false;

  subjects: SubjectShortInfoModel[] = [];
  selectedSubjectId = -1;

  @Input() groupId = -1;

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;

  constructor(
    private studentService: StudentService,
    public dialog: MatDialog,
    private tools: ToolsService
  ) {
    this.dataSource = new MatTableDataSource();
  }

  render(data: StudentRegisterDataModel[][]) {
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
    this.studentService.getRegisterData(this.selectedSubjectId, this.isExtended).subscribe(
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
    this.studentService.getSubjects().subscribe(subjects => {
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
