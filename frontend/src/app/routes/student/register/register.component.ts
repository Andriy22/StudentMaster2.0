import { Component, OnInit } from '@angular/core';
import { StudentService } from '@core/services/student.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { StudentRegisterDataModel } from '@shared/models/student-register-data.model';

@Component({
  selector: 'app-student-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class StudentRegisterComponent implements OnInit {
  data: StudentRegisterDataModel[] = [];

  displayedColumns: string[] = [];
  columnsSchema: { key: string; title: string }[] = [];
  dataSource: MatTableDataSource<any>;
  groups: { header: string; colSpan: number }[] = [];
  groupedColumns: string[] = [];

  allWorks: string[] = [];
  allColumns: string[] = [];

  isExtended = false;

  isLoading = false;

  constructor(private studentService: StudentService) {
    this.dataSource = new MatTableDataSource([{}]);
  }

  loadRegisterData() {
    this.isLoading = true;
    this.studentService.getRegisterData(123, this.isExtended).subscribe(
      data => {
        this.data = data;

        const obj: { [key: string]: string } = {};

        data.forEach(el => {
          el.items.forEach(item => {
            this.columnsSchema.push({
              key: item.name,
              title: item.title,
            });
            obj[item.name] = item.value;
          });
          this.groups.push({ header: el.header, colSpan: el.items.length });
        });

        this.allColumns = this.columnsSchema.map(col => col.key);
        this.rerender();
        this.allWorks = this.groups.map(col => col.header);

        this.dataSource.data = [obj];

        this.isLoading = false;
      },
      _ => (this.isLoading = false)
    );
  }

  ngOnInit() {
    this.loadRegisterData();
  }

  onExtendChange(event: MatSlideToggleChange) {
    this.reset();
    this.loadRegisterData();
  }

  onHiddenChange(event: any[]) {
    this.isLoading = true;
    this.reset();
    const intersection = this.allWorks.filter(x => event.includes(x));
    const obj: { [key: string]: string } = {};
    intersection.forEach(work => {
      this.data.forEach(el => {
        if (el.header == work) {
          el.items.forEach(item => {
            this.columnsSchema.push({
              key: item.name,
              title: item.title,
            });
            obj[item.name] = item.value;
          });
          this.groups.push({ header: el.header, colSpan: el.items.length });
        }
      });
    });

    this.rerender();

    this.dataSource.data = [obj];

    this.isLoading = false;
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
