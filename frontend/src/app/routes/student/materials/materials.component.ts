import { Component, OnInit } from '@angular/core';
import { SubjectShortInfoModel } from '@shared/models/subject-short-info.model';
import { StudentService } from '@core/services/student.service';
import { CrudEducationMaterial } from '@shared/models/education-material.models';
import { AdditionalMaterialsService } from '@core/services/additional-materials.service';
import { EducationMaterialType } from '@shared/enums/education-material-type.enums';

@Component({
  selector: 'app-student-materials',
  templateUrl: './materials.component.html',
  styleUrls: ['./materials.component.scss'],
})
export class StudentMaterialsComponent implements OnInit {
  subjects: SubjectShortInfoModel[] = [];
  selectedSubjectId = -1;

  selectGroupId = -1;

  materials: CrudEducationMaterial[] = [];

  constructor(
    private studentService: StudentService,
    private materialsService: AdditionalMaterialsService
  ) {}

  ngOnInit() {
    this.studentService.getSubjects().subscribe(subjects => {
      this.subjects = subjects;

      if (subjects.length > 0) {
        this.selectedSubjectId = subjects[0].id;

        this.studentService.getMyGroup().subscribe(group => {
          this.selectGroupId = group.id;

          this.loadData();
        });
      }
    });
  }

  loadData = () => {
    this.materialsService
      .getAdditionalMaterials(this.selectedSubjectId, this.selectGroupId)
      .subscribe(materials => {
        this.materials = materials;
      });
  };

  onSubjectChange(updatedSubjectId: number) {
    this.selectedSubjectId = updatedSubjectId;
    this.loadData();
  }

  navigate(material: CrudEducationMaterial) {
    window
      .open(
        material.type === EducationMaterialType.File
          ? 'https://localhost:7189/static/EducationMaterials/' + material.url
          : material.url,
        '_blank'
      )
      ?.focus();
  }
}
