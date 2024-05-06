import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { CrudEducationMaterial } from '@shared/models/education-material.models';
import { AdditionalMaterialsService } from '@core/services/additional-materials.service';
import { tap } from 'rxjs/operators';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { TeacherService } from '@core/services/teacher.service';
import { TeacherRegisterAddWorkComponent } from '../register/add-work/add-work.component';
import { MatDialog } from '@angular/material/dialog';
import { ToolsService } from '@shared/services/tools.service';
import { TeacherMaterialsAddEditMaterialComponent } from './add-edit-material/add-edit-material.component';

@Component({
  selector: 'app-teacher-materials',
  templateUrl: './materials.component.html',
  styleUrls: ['./materials.component.scss']
})
export class TeacherMaterialsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['title'];

  dataSource: MatTableDataSource<CrudEducationMaterial>;

  subjects: SubjectInfoModel[] = [];
  selectedSubjectId = -1;

  isLoading = false;

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;
  @ViewChild(MatSort) sort: MatSort | undefined;

  constructor(private _materialsService: AdditionalMaterialsService,
              private _teacherService: TeacherService,
              public dialog: MatDialog,
              private tools: ToolsService) {
    this.dataSource = new MatTableDataSource<CrudEducationMaterial>([]);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  ngAfterViewInit() {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
    }
  }

  onSubjectChange(event: any) {
    setTimeout(_ => {
      this.refreshData();
    }, 0);
  }

  refreshData = () => {
    this.isLoading = true;

    this._materialsService
      .getAdditionalMaterials(this.selectedSubjectId)
      .pipe(tap(groups => (this.dataSource.data = groups)))
      .subscribe(_ => this.isLoading = false, _ => this.isLoading = false);
  }

  addMaterial() {
    this._teacherService.getTeacherGroupsBySubject(this.selectedSubjectId).subscribe(groups => {
      const dialogRef = this.dialog
        .open(TeacherMaterialsAddEditMaterialComponent, {
          width: '90%',
          data: { subjectId: this.selectedSubjectId, groups },
        })
        .afterClosed()
        .subscribe((_: any) => this.refreshData());
    });
  }

  ngOnInit() {
    this._teacherService.getTeacherSubjects().subscribe(subjects => {
      this.subjects = subjects;

      if (subjects.length > 0) {
        this.selectedSubjectId = subjects[0].id;

        this.refreshData();
      }
    });
  }

}
