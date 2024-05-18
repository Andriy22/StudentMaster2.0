import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { TeacherService } from '@core/services/teacher.service';
import { MatDialog } from '@angular/material/dialog';
import { ToolsService } from '@shared/services/tools.service';
import { MatTableDataSource } from '@angular/material/table';
import { TeacherHomeworkAddEditHomeworkComponent } from './add-edit-homework/add-edit-homework.component';
import { HOMEWORK_TYPE_LABELS } from '@shared/constants/homework.constants';
import { Homework } from '@shared/models/homeworks.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { tap } from 'rxjs/operators';
import { HomeworkService } from '@core/services/homework.service';

@Component({
  selector: 'app-teacher-homework',
  templateUrl: './homework.component.html',
  styleUrls: ['./homework.component.scss'],
})
export class TeacherHomeworkComponent implements OnInit, AfterViewInit {
  @Input() groupId = -1;
  displayedColumns: string[] = ['title', 'type', 'url', 'actions'];

  subjects: SubjectInfoModel[] = [];
  selectedSubjectId = -1;

  dataSource: MatTableDataSource<any>;

  isLoading = false;

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  protected readonly HOMEWORK_TYPE_LABELS = HOMEWORK_TYPE_LABELS;

  constructor(
    private teacherService: TeacherService,
    private homeworkService: HomeworkService,
    public dialog: MatDialog,
    private tools: ToolsService
  ) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit() {
    this.teacherService.getTeacherSubjectsInGroup(this.groupId).subscribe(
      subjects => {
        this.isLoading = true;
        this.subjects = subjects;

        if (subjects.length > 0) {
          this.selectedSubjectId = subjects[0].id;
          this.isLoading = false;

          this.refreshData();
        }
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  ngAfterViewInit() {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
    }
  }

  addHomework() {
    const dialogRef = this.dialog
      .open(TeacherHomeworkAddEditHomeworkComponent, {
        width: '90%',
        data: { subjectId: this.selectedSubjectId, groupId: this.groupId },
      })
      .afterClosed()
      .subscribe((_: any) => {
        this.refreshData();
      });
  }

  onSubjectChange(event: any) {}

  edit(row: Homework) {
    const dialogRef = this.dialog
      .open(TeacherHomeworkAddEditHomeworkComponent, {
        width: '90%',
        data: { subjectId: this.selectedSubjectId, groupId: this.groupId, homework: row },
      })
      .afterClosed()
      .subscribe((_: any) => this.refreshData());
  }

  delete(id: number) {
    this.homeworkService.deleteHomework(id).subscribe(_ => {
      this.refreshData();
    });
  }

  refreshData = () => {
    this.isLoading = true;

    this.homeworkService
      .getHomeworks(this.groupId, this.selectedSubjectId)
      .pipe(tap(homeworks => (this.dataSource.data = homeworks)))
      .subscribe(
        _ => (this.isLoading = false),
        _ => (this.isLoading = false)
      );
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
