import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { environment } from '@env/environment';
import { UserInfoModel } from '@shared/models/user-info.model';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { UserShortInfoModel } from '@shared/models/user-short-info.model';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  constructor(private http: HttpClient) {}

  public getGroupInfoById(groupId: number) {
    return this.http.get<GroupInfoModel>(`${environment.apiUrl}/group/get-group-info/${groupId}`);
  }

  public getGroupSubjects(groupId: number) {
    return this.http.get<SubjectInfoModel[]>(
      `${environment.apiUrl}/group/get-group-subjects/${groupId}`
    );
  }

  public getStudentsByGroupId(groupId: number) {
    return this.http.get<UserInfoModel[]>(
      `${environment.apiUrl}/group/get-group-students/${groupId}`
    );
  }

  public getTeachersByGroupId(groupId: number) {
    return this.http.get<UserShortInfoModel[]>(
      `${environment.apiUrl}/group/get-group-teachers/${groupId}`
    );
  }
}
