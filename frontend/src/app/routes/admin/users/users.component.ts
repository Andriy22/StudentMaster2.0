import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UserInfoModel } from '@shared/models/user-info.model';
import { AdminService } from '@core/services/admin.service';
import { tap } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { CreateUserComponent } from './modals/create-user/create-user.component';
import { AdminUsersChangeStudentGroupComponent } from './change-student-group/change-student-group.component';
import { AdminUsersChangeTeacherSubjectsComponent } from './change-teacher-subjects/change-teacher-subjects.component';

@Component({
  selector: 'app-admin-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class AdminUsersComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'fullName',
    'email',
    'roles',
    'isDeleted',
    'lastOnlineDate',
    'actions',
  ];

  dataSource: MatTableDataSource<UserInfoModel>;
  roles: string[] = [];

  selectedRole = 'Admin';

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  constructor(private adminService: AdminService, public dialog: MatDialog) {
    this.dataSource = new MatTableDataSource<UserInfoModel>([]);
  }

  ngAfterViewInit() {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
    }
  }

  fetchRoles() {
    this.adminService
      .getRoles()
      .pipe(tap(roles => (this.roles = roles)))
      .subscribe();
  }

  fetchUsers(role: string) {
    this.adminService
      .getUsersByRole(role)
      .pipe(tap(users => (this.dataSource.data = users)))
      .subscribe();
  }

  ngOnInit() {
    this.fetchUsers('Admin');
    this.fetchRoles();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  onRoleFilterChange(event: MatButtonToggleChange) {
    this.fetchUsers(event.value);
    this.selectedRole = event.value;
  }

  addUser() {
    const dialogRef = this.dialog.open(CreateUserComponent, {
      width: '90%',
      data: { roles: this.roles },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.fetchUsers(this.selectedRole);
    });
  }

  changeStudentGroup(id: string) {
    const dialogRef = this.dialog.open(AdminUsersChangeStudentGroupComponent, {
      width: '90%',
      data: { studentId: id },
    });
  }

  changeTeacherSubjects(id: string) {
    const dialogRef = this.dialog.open(AdminUsersChangeTeacherSubjectsComponent, {
      width: '90%',
      data: { teacherId: id },
    });
  }
}
