import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GroupService } from '@core/services/group.service';
import { GroupInfoModel } from '@shared/models/group-info.model';

@Component({
  selector: 'app-teacher-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class TeacherGroupComponent implements OnInit {
  groupId = -1;
  group: GroupInfoModel | undefined;

  constructor(private route: ActivatedRoute, private groupService: GroupService) {
    route.params.subscribe(params => {
      this.groupId = params.id;
      this.groupService.getGroupInfoById(params.id).subscribe(group => {
        this.group = group;
      });
    });
  }

  ngOnInit() {}
}
