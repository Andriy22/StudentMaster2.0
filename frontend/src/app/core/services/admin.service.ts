import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { UserInfoModel } from '@shared/models/user-info.model';
import { CreateUserModel } from '@shared/models/create-user.model';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { GroupInfoModel } from '@shared/models/group-info.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient) {}

  public getUsersByRole(role: string) {
    return this.http.get<UserInfoModel[]>(`${environment.apiUrl}/admin/get-users-by-role/${role}`);
  }

  public getRoles() {
    return this.http.get<string[]>(`${environment.apiUrl}/admin/get-roles`);
  }

  public createUser(user: CreateUserModel) {
    return this.http.post(`${environment.apiUrl}/account/create-account`, user);
  }

  public getSubjects() {
    return this.http.get<SubjectInfoModel[]>(`${environment.apiUrl}/admin/get-subjects`);
  }

  public getTeacherSubjects(teacherId: string) {
    return this.http.get<SubjectInfoModel[]>(
      `${environment.apiUrl}/admin/get-teacher-subjects/${teacherId}`
    );
  }

  public createSubject(name: string) {
    return this.http.get(`${environment.apiUrl}/admin/create-subject/${name}`, {});
  }

  public changeSubjectStatus(subjectId: number) {
    return this.http.get(`${environment.apiUrl}/admin/change-subject-status/${subjectId}`, {});
  }

  public getGroups() {
    return this.http.get<GroupInfoModel[]>(`${environment.apiUrl}/admin/get-groups`);
  }

  public getGroupByStudentId(studentId: string) {
    return this.http.get<GroupInfoModel>(
      `${environment.apiUrl}/admin/get-group-by-student/${studentId}`
    );
  }

  public createGroup(name: string) {
    return this.http.get(`${environment.apiUrl}/admin/create-group/${name}`, {});
  }

  public changeGroupStatus(groupId: number) {
    return this.http.get(`${environment.apiUrl}/admin/change-group-status/${groupId}`, {});
  }

  public changeGroupName(groupId: number, newGroupName: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/change-group-name/${groupId}/${newGroupName}`,
      {}
    );
  }

  public changeStudentGroup(groupId: number, studentId: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/change-student-group/${studentId}/${groupId}`,
      {}
    );
  }

  public addSubjectToGroup(groupId: number, subjectId: number) {
    return this.http.get(
      `${environment.apiUrl}/admin/add-subject-to-group/${groupId}/${subjectId}`
    );
  }

  public removeSubjectFromGroup(groupId: number, subjectId: number) {
    return this.http.get(
      `${environment.apiUrl}/admin/remove-subject-from-group/${groupId}/${subjectId}`
    );
  }

  public addTeacherToGroup(groupId: number, teacherId: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/add-teacher-to-group/${groupId}/${teacherId}`
    );
  }

  public removeTeacherFromGroup(groupId: number, teacherId: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/remove-teacher-from-group/${groupId}/${teacherId}`
    );
  }

  public addSubjectToTeacher(subjectId: number, teacherId: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/add-subject-to-teacher/${subjectId}/${teacherId}`
    );
  }

  public removeSubjectFromTeacher(subjectId: number, teacherId: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/remove-subject-from-teacher/${subjectId}/${teacherId}`
    );
  }

  public removeStudentFromGroup(groupId: number, studentId: string) {
    return this.http.get(
      `${environment.apiUrl}/admin/remove-student-from-group/${studentId}/${groupId}`
    );
  }
}
