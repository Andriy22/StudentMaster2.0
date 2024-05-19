import { Component, OnInit } from '@angular/core';
import { SubjectShortInfoModel } from '@shared/models/subject-short-info.model';
import { CrudEducationMaterial } from '@shared/models/education-material.models';
import { StudentService } from '@core/services/student.service';
import { EducationMaterialType } from '@shared/enums/education-material-type.enums';
import { HomeworkService } from '@core/services/homework.service';
import { Homework } from '@shared/models/homeworks.model';

@Component({
  selector: 'app-student-homeworks',
  templateUrl: './homeworks.component.html',
  styleUrls: ['./homeworks.component.scss'],
})
export class StudentHomeworksComponent implements OnInit {
  subjects: SubjectShortInfoModel[] = [];
  selectedSubjectId = -1;

  selectGroupId = -1;

  homeworks: Homework[] = [];

  constructor(private studentService: StudentService, private homeworkService: HomeworkService) {}

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
    this.homeworkService
      .getHomeworks(this.selectGroupId, this.selectedSubjectId)
      .subscribe(homeworks => {
        this.homeworks = homeworks;
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
