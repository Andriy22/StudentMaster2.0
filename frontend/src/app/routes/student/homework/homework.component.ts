import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Homework } from '@shared/models/homeworks.model';
import { HOMEWORK_STATUS_LABELS, HOMEWORK_TYPE_LABELS } from '@shared/constants/homework.constants';
import { HomeworkService } from '@core/services/homework.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-student-homework',
  templateUrl: './homework.component.html',
  styleUrls: ['./homework.component.scss'],
})
export class StudentHomeworkComponent implements OnInit {
  homeworkId = -1;

  canSendHomework = true;
  homework: Homework | undefined = undefined;
  displayedColumns: string[] = ['reviewerName', 'status', 'filePath', 'comment', 'mark', 'actions'];
  dataSource: MatTableDataSource<any>;
  isLoading = false;
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;
  protected readonly HOMEWORK_TYPE_LABELS = HOMEWORK_TYPE_LABELS;
  protected readonly HOMEWORK_STATUS_LABELS = HOMEWORK_STATUS_LABELS;

  constructor(private route: ActivatedRoute, private homeworkService: HomeworkService) {
    this.dataSource = new MatTableDataSource();
    route.params.subscribe(params => {
      this.homeworkId = params.id;

      this.homeworkService.getHomework(this.homeworkId).subscribe(homework => {
        this.homework = homework;
        this.refreshData();
      });
    });
  }

  submitHomework(event: Event): void {
    const target = (event as any).target as HTMLInputElement;
    const files = target.files as FileList;

    const formData = new FormData();
    formData.append('HomeworkId', this.homeworkId.toString());
    formData.append('File', files[0] as Blob);

    this.homeworkService.submitHomework(formData).subscribe(_ => {
      this.refreshData();
    });
  }

  refreshData = () => {
    this.isLoading = true;

    this.homeworkService
      .getHomeworkStudent(this.homeworkId)
      .pipe(
        tap(homeworks => {
          this.dataSource.data = homeworks;
          this.canSendHomework =
            homeworks.every(h => h.status === 1 || h.status == 3) || homeworks.length === 0;
        })
      )
      .subscribe();
  };

  delete(id: number) {
    this.homeworkService.cancelHomeworkToReview(id).subscribe(_ => {
      this.refreshData();
    });
  }

  applyFilter(event: Event) {
    const filterValue = ((event as any).target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  ngOnInit() {}
}
