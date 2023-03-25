import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AdminService } from '@core/services/admin.service';
import { tap } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { AdminSubjectsAddSubjectComponent } from './add-subject/add-subject.component';

@Component({
  selector: 'app-admin-subjects',
  templateUrl: './subjects.component.html',
  styleUrls: ['./subjects.component.scss'],
})
export class AdminSubjectsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['name', 'isDeleted', 'actions'];

  dataSource: MatTableDataSource<SubjectInfoModel>;

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  constructor(private adminService: AdminService, public dialog: MatDialog) {
    this.dataSource = new MatTableDataSource<SubjectInfoModel>([]);
  }

  ngAfterViewInit() {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
    }
  }

  fetchSubjects() {
    this.adminService
      .getSubjects()
      .pipe(tap(subjects => (this.dataSource.data = subjects)))
      .subscribe();
  }

  ngOnInit() {
    this.fetchSubjects();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  addSubject() {
    const dialogRef = this.dialog.open(AdminSubjectsAddSubjectComponent, {
      width: '90%',
      data: {},
    });
    dialogRef.afterClosed().subscribe(() => {
      this.fetchSubjects();
    });
  }

  changeState(id: number) {
    this.adminService.changeSubjectStatus(id).subscribe(_ => {
      this.fetchSubjects();
    });
  }
}
