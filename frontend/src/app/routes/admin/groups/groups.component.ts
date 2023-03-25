import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { AdminService } from '@core/services/admin.service';
import { MatDialog } from '@angular/material/dialog';
import { tap } from 'rxjs/operators';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { AdminGroupsAddGroupComponent } from './add-group/add-group.component';
import { AdminGroupsEditGroupComponent } from './edit-group/edit-group.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss'],
})
export class AdminGroupsComponent implements OnInit {
  displayedColumns: string[] = ['name', 'isDeleted', 'actions'];

  dataSource: MatTableDataSource<GroupInfoModel>;

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  constructor(
    private adminService: AdminService,
    public dialog: MatDialog,
    private router: Router
  ) {
    this.dataSource = new MatTableDataSource<GroupInfoModel>([]);
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
      .getGroups()
      .pipe(tap(groups => (this.dataSource.data = groups)))
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
    const dialogRef = this.dialog.open(AdminGroupsAddGroupComponent, {
      width: '90%',
      data: {},
    });
    dialogRef.afterClosed().subscribe(() => {
      this.fetchSubjects();
    });
  }

  changeState(id: number) {
    this.adminService.changeGroupStatus(id).subscribe(_ => {
      this.fetchSubjects();
    });
  }

  changeGroupName(id: number) {
    if (!this.dataSource) {
      return;
    }

    if (!this.dataSource.data) {
      return;
    }

    const dialogRef = this.dialog.open(AdminGroupsEditGroupComponent, {
      width: '90%',
      data: {
        groupName: this.dataSource.data.find(x => x.id === id)?.name,
        groupId: id,
      },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.fetchSubjects();
    });
  }

  navigateToGroup(id: number) {
    this.router.navigate([`/admin/group/${id}`]).then(r => {});
  }
}
