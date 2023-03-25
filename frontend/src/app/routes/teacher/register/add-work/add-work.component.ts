import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, Validators } from '@angular/forms';
import { TeacherService } from '@core/services/teacher.service';

@Component({
  selector: 'app-teacher-register-add-work',
  templateUrl: './add-work.component.html',
  styleUrls: ['./add-work.component.scss'],
})
export class TeacherRegisterAddWorkComponent implements OnInit {
  firstFormGroup = this._formBuilder.group({
    firstCtrl: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  });
  isLinear = true;

  items: { name: string; maxGrade: number; removable: boolean }[] = [
    {
      name: 'Загальна',
      maxGrade: 10,
      removable: false,
    },
  ];

  constructor(
    public dialogRef: MatDialogRef<TeacherRegisterAddWorkComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private teacherService: TeacherService
  ) {}

  ngOnInit() {}

  addItem() {
    this.items.push({
      name: '',
      maxGrade: 10,
      removable: true,
    });
  }

  createWork() {
    const object: any = {};

    object.name = this.firstFormGroup.get('firstCtrl')?.value;
    object.items = this.items;
    object.groupId = this.data.groupId;
    object.subjectId = this.data.subjectId;

    this.teacherService.addWork(object).subscribe(_ => {
      this.dialogRef.close();
    });
  }

  removeItem(item: { name: string; maxGrade: number; removable: boolean }) {
    const index = this.items.indexOf(item);
    if (index !== -1) {
      this.items.splice(index, 1);
    }
  }

  onPresentChanged(event: any) {
    if (event !== 'none') {
      this.items = event.items;
    }
  }
}
