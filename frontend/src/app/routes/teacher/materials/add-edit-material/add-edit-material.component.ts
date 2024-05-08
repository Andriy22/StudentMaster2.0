import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TeacherService } from '@core/services/teacher.service';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { AdditionalMaterialsService } from '@core/services/additional-materials.service';
import { CrudEducationMaterial } from '@shared/models/education-material.models';

@Component({
  selector: 'app-teacher-materials-add-edit-material',
  templateUrl: './add-edit-material.component.html',
  styleUrls: ['./add-edit-material.component.scss'],
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

  file: File | null = null;
  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<TeacherMaterialsAddEditMaterialComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private teacherService: TeacherService,
    private materialsService: AdditionalMaterialsService
  ) {}

  onSubmit() {
    const formData = new FormData();

    formData.append('Title', this.firstFormGroup.controls.title.value?.toString() ?? '');
    formData.append('Type', this.firstFormGroup.controls.type.value?.toString() ?? '0');
    formData.append('File', this.file as Blob);
    formData.append('Url', this.firstFormGroup.controls.path.value?.toString() ?? '0');
    formData.append('SubjectId', this.data?.subjectId ?? '0');
    formData.append('Groups', this.selectedGroupIds.join(','));

    this.materialsService.createAdditionalMaterial(formData).subscribe(_ => {});
  }

  upload(event: Event) {
    const target = event.target as HTMLInputElement;
    const files = target.files as FileList;
    this.file = files[0];
    this.firstFormGroup.controls.path.setValue(files[0].name);
  }

  ngOnInit() {
    this.allGroups = this.data?.groups ?? [];

    const material = this.data.material as CrudEducationMaterial;

    if (material) {
      this.firstFormGroup.controls.path.setValue(material.url);
      this.firstFormGroup.controls.title.setValue(material.title);
      this.firstFormGroup.controls.type.setValue(material.type.toString());

      this.selectedGroupIds = material.groups.map(x => x.toString());
    }
  }
}
