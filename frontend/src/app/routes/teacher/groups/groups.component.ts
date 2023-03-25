import { Component, OnInit } from '@angular/core';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { TeacherService } from '@core/services/teacher.service';

@Component({
  selector: 'app-teacher-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss'],
})
export class TeacherGroupsComponent implements OnInit {
  groups: GroupInfoModel[] = [];

  constructor(private teacherService: TeacherService) {}

  ngOnInit() {
    this.teacherService.getTeacherGroups().subscribe(groups => (this.groups = groups));
  }
}
