import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { TeacherRegisterDataModel } from '@shared/models/teacher-register-data.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { TeacherService } from '@core/services/teacher.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { GroupService } from '@core/services/group.service';
import { SubjectInfoModel } from '@shared/models/subject-info.model';

@Component({
  selector: 'app-admin-group-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class AdminGroupRegisterComponent implements OnInit {
  data: TeacherRegisterDataModel[][] = [];

  displayedColumns: string[] = [];
  columnsSchema: { key: string; title: string }[] = [];
  dataSource: MatTableDataSource<any>;
  groups: { key?: string; header: string; colSpan: number }[] = [];
  groupedColumns: string[] = [];

  allWorks: string[] = [];
  allColumns: string[] = [];

  isExtended = false;
  isEdit = false;

  isLoading = false;

  subjects: SubjectInfoModel[] = [];
  selectedSubjectId = -1;

  @Input() groupId = -1;
  @ViewChild(MatPaginator)
  paginator: MatPaginator | undefined;

  constructor(
    private teacherService: TeacherService,
    public dialog: MatDialog,
    private groupService: GroupService
  ) {
    this.dataSource = new MatTableDataSource();
  }

  loadRegisterData() {
    this.isLoading = true;
    this.teacherService
      .getRegisterData(this.groupId, this.selectedSubjectId, this.isExtended)
      .subscribe(
        data => {
          this.data = data;

          data.forEach(row => {
            const obj: { [key: string]: string } = {};
            row.forEach(el => {
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
          this.allWorks = this.groups.map(col => col.header);

          this.isLoading = false;
        },
        _ => (this.isLoading = false)
      );
  }

  ngOnInit() {
    this.groupService.getGroupSubjects(this.groupId).subscribe(subjects => {
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

  onHiddenChange(event: any[]) {
    this.isLoading = true;
    this.reset();
    const intersection = this.allWorks.filter(x => event.includes(x));

    this.data.forEach(row => {
      const obj: { [key: string]: string } = {};
      row.forEach(el => {
        if (intersection.includes(el.header)) {
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
        }
      });
      this.dataSource.data.push(obj);
    });

    this.rerender();
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
    this.isLoading = false;
  }

  onSubjectChange(event: any) {
    this.reset();

    setTimeout(_ => {
      this.loadRegisterData();
    }, 0);
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
