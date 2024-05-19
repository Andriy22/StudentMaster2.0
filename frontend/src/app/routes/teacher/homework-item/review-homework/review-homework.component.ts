import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HomeworkService } from '@core/services/homework.service';
import { Homework, HomeworkStudent, ReviewHomework } from '@shared/models/homeworks.model';
import { HOMEWORK_STATUS_LABELS } from '@shared/constants/homework.constants';

@Component({
  selector: 'app-teacher-homework-item-review-homework',
  templateUrl: './review-homework.component.html',
  styleUrls: ['./review-homework.component.scss'],
})
export class TeacherHomeworkItemReviewHomeworkComponent implements OnInit {
  firstFormGroup = this._formBuilder.group({
    mark: ['', []],
    status: ['2', Validators.required],
    comment: ['', []],
  });

  homework: Homework | null = null;
  item: HomeworkStudent | null = null;
  isLoading = false;
  protected readonly HOMEWORK_STATUS_LABELS = HOMEWORK_STATUS_LABELS;

  constructor(
    public dialogRef: MatDialogRef<TeacherHomeworkItemReviewHomeworkComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private homeworkService: HomeworkService
  ) {}

  onSubmit() {
    const formData = new FormData();

    const id = this.data?.item?.id;
    const grade = this.firstFormGroup.controls.mark.value;

    const rGrade = grade === null ? null : +grade;

    const model: ReviewHomework = {
      grade: +rGrade!,
      id: id,
      comment: this.firstFormGroup.controls.comment.value ?? '',
      status: +this.firstFormGroup.controls.status.value!,
    };
    this.homeworkService.reviewHomework(model).subscribe(
      _ => {
        this.dialogRef.close();
      },
      () => {
        this.dialogRef.close();
      }
    );
  }

  ngOnInit() {
    const material = this.data.item as HomeworkStudent;

    if (material) {
      this.firstFormGroup.controls.comment.setValue(material.comment);
      this.firstFormGroup.controls.mark.setValue(material.grade?.toString() ?? '');
      this.item = material;
    }

    const homework = this.data.homework as Homework;

    if (homework) {
      this.homework = homework;
      this.firstFormGroup.controls.mark.setValidators([Validators.max(homework.maxGrade)]);
    }
  }
}
