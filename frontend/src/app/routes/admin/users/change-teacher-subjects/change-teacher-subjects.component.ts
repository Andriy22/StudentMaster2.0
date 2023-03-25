import { Component, Inject, OnInit } from '@angular/core';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { ToolsService } from '@shared/services/tools.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from '@core/services/admin.service';

@Component({
  selector: 'app-admin-users-change-teacher-subjects',
  templateUrl: './change-teacher-subjects.component.html',
  styleUrls: ['./change-teacher-subjects.component.scss'],
})
export class AdminUsersChangeTeacherSubjectsComponent implements OnInit {
  subjects: SubjectInfoModel[] = [];
  allSubjects: SubjectInfoModel[] = [];

  groups: { title: string; items: SubjectInfoModel[] }[] = [];

  selectedSubjectIds: number[] = [];

  isLoading = false;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<AdminUsersChangeTeacherSubjectsComponent>,
    private adminService: AdminService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.update();
  }

  ngOnInit(): void {}

  update() {
    this.isLoading = true;
    this.adminService.getSubjects().subscribe(data => {
      this.allSubjects = data;
      this.groups = [];
      this.groups.push({ title: 'Активні', items: data.filter(x => !x.isDeleted) });
      this.groups.push({ title: 'Деактивовані', items: data.filter(x => x.isDeleted) });

      this.adminService.getTeacherSubjects(this.data.teacherId).subscribe(subjects => {
        this.subjects = subjects;
        this.selectedSubjectIds = subjects.map(x => x.id);
        this.isLoading = false;
      });
    });
  }

  close() {
    this.dialogRef.close();
  }

  onItemSelected(id: number) {
    if (this.subjects.find(x => x.id === id)) {
      this.adminService.removeSubjectFromTeacher(id, this.data.teacherId).subscribe(() => {
        this.tools.showNotification('Предмет успішно видалено!');
        this.update();
      });
      return;
    }

    this.adminService.addSubjectToTeacher(id, this.data.teacherId).subscribe(() => {
      this.tools.showNotification('Предмет успішно додано!');
      this.update();
    });
  }
}
