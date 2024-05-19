import { Component, OnInit, ViewChild } from '@angular/core';
import { Homework, HomeworkStudent } from '@shared/models/homeworks.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';
import { HomeworkService } from '@core/services/homework.service';
import { tap } from 'rxjs/operators';
import { HOMEWORK_STATUS_LABELS, HOMEWORK_TYPE_LABELS } from '@shared/constants/homework.constants';
import { MatDialog } from '@angular/material/dialog';
import { TeacherHomeworkItemReviewHomeworkComponent } from './review-homework/review-homework.component';

@Component({
  selector: 'app-teacher-homework-item',
  templateUrl: './homework-item.component.html',
  styleUrls: ['./homework-item.component.scss'],
})
export class TeacherHomeworkItemComponent implements OnInit {
  homeworkId = -1;

  homework: Homework | undefined = undefined;
  displayedColumns: string[] = ['ownerName', 'status', 'filePath', 'comment', 'mark', 'actions'];
  dataSource: MatTableDataSource<any>;
  isLoading = false;
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  protected readonly HOMEWORK_TYPE_LABELS = HOMEWORK_TYPE_LABELS;
  protected readonly HOMEWORK_STATUS_LABELS = HOMEWORK_STATUS_LABELS;

  constructor(
    private route: ActivatedRoute,
    private homeworkService: HomeworkService,
    public dialog: MatDialog
  ) {
    this.dataSource = new MatTableDataSource();
    route.params.subscribe(params => {
      this.homeworkId = params.id;

      this.homeworkService.getHomework(this.homeworkId).subscribe(homework => {
        this.homework = homework;
        this.refreshData();
      });
    });
  }

  refreshData = () => {
    this.isLoading = true;

    this.homeworkService
      .getHomeworksForReview(this.homeworkId)
      .pipe(
        tap(homeworks => {
          this.dataSource.data = homeworks;
        })
      )
      .subscribe();
  };

  applyFilter(event: Event) {
    const filterValue = ((event as any).target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  edit(row: HomeworkStudent) {
    const dialogRef = this.dialog
      .open(TeacherHomeworkItemReviewHomeworkComponent, {
        width: '90%',
        data: { item: row, homework: this.homework },
      })
      .afterClosed()
      .subscribe((_: any) => this.refreshData());
  }

  ngOnInit() {}
}
