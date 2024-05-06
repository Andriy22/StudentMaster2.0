import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TeacherService } from '@core/services/teacher.service';
import { GroupInfoModel } from '@shared/models/group-info.model';

@Component({
  selector: 'app-teacher-materials-add-edit-material',
  templateUrl: './add-edit-material.component.html',
  styleUrls: ['./add-edit-material.component.scss']
})
export class TeacherMaterialsAddEditMaterialComponent implements OnInit {
  firstFormGroup = this._formBuilder.group({
    title: ['', Validators.required],
    type: ['0', Validators.required],
    path: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    secondCtrl: ['', Validators.required],
  });
  isLinear = true;

  groups: GroupInfoModel[] = [];
  allGroups: GroupInfoModel[] = [];

  selectedGroupIds: string[] = [];

  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<TeacherMaterialsAddEditMaterialComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private teacherService: TeacherService
  ) {}

  ngOnInit() {
    this.allGroups = this.data?.groups ?? [];
  }

}
