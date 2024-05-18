import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HomeworkService } from '@core/services/homework.service';
import { Homework } from '@shared/models/homeworks.model';

@Component({
  selector: 'app-teacher-homework-add-edit-homework',
  templateUrl: './add-edit-homework.component.html',
  styleUrls: ['./add-edit-homework.component.scss'],
})
export class TeacherHomeworkAddEditHomeworkComponent implements OnInit {
  firstFormGroup = this._formBuilder.group({
    title: ['', Validators.required],
    type: ['0', Validators.required],
    description: ['', Validators.required],
    maxGrade: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
  });

  isLinear = true;

  file: File | null = null;
  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<TeacherHomeworkAddEditHomeworkComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private homeworkService: HomeworkService
  ) {}

  onSubmit() {
    const formData = new FormData();

    const id = this.data?.homework?.id;

    formData.append('Title', this.firstFormGroup.controls.title.value?.toString() ?? '');
    formData.append('Type', this.firstFormGroup.controls.type.value?.toString() ?? '0');
    formData.append('SubjectId', this.data?.subjectId ?? '0');
    formData.append('GroupId', this.data?.groupId ?? '0');
    formData.append('Description', this.firstFormGroup.controls.description?.value ?? '');
    formData.append('MaxGrade', this.firstFormGroup.controls.maxGrade?.value ?? '0');

    if (this.file) {
      formData.append('File', this.file as Blob);
    }

    if (id) {
      formData.append('id', id);

      this.homeworkService.editHomework(formData).subscribe(_ => {});
      this.dialogRef.close();
      return;
    }

    this.homeworkService.addHomework(formData).subscribe(_ => {});
    this.dialogRef.close();
  }

  upload(event: Event) {
    const target = event.target as HTMLInputElement;
    const files = target.files as FileList;
    this.file = files[0];
  }

  ngOnInit() {
    const material = this.data.homework as Homework;

    if (material) {
      this.firstFormGroup.controls.description.setValue(material.description);
      this.firstFormGroup.controls.title.setValue(material.title);
      this.firstFormGroup.controls.type.setValue(material.type.toString());
      this.firstFormGroup.controls.maxGrade.setValue(material.maxGrade.toString());
    }
  }
}
