import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { TeacherRegisterDataModel } from '@shared/models/teacher-register-data.model';
import { GroupInfoModel } from '@shared/models/group-info.model';
import { SubjectInfoModel } from '@shared/models/subject-info.model';
import { PresetViewModel } from '@shared/models/presets.models';
import { Observable } from 'rxjs';
import { ScheduleItemViewModel } from '@shared/models/schedule.models';
import { isExtended } from '@angular/compiler-cli/src/ngtsc/shims/src/expando';

@Injectable({
  providedIn: 'root',
})
export class TeacherService {
  constructor(private http: HttpClient) {}

  public getRegisterData(groupId: number, subjectId: number, isExtended: boolean) {
    return this.http.get<TeacherRegisterDataModel[][]>(
      `${environment.apiUrl}/teacher/get-register-data/${groupId}/${subjectId}/${isExtended}`
    );
  }

  public getTeacherGroups() {
    return this.http.get<GroupInfoModel[]>(`${environment.apiUrl}/teacher/get-my-groups`);
  }

  public getTeacherGroupsBySubject(subjectId: number) {
    return this.http.get<GroupInfoModel[]>(`${environment.apiUrl}/teacher/get-my-groups-by-subject/${subjectId}`);
  }

  public getPresets(groupId: number, subjectId: number) {
    return this.http.get<PresetViewModel[]>(
      `${environment.apiUrl}/teacher/get-presets/${groupId}/${subjectId}`
    );
  }

  public getStudentAttendance(groupId: number, subjectId: number, date: string) {
    return this.http.get<PresetViewModel[]>(
      `${environment.apiUrl}/teacher/get-students-attendance/${groupId}/${subjectId}/${date}`
    );
  }

  public setStudentAttendance(
    studentId: string,
    subjectId: number,
    date: string,
    attendance: boolean
  ) {
    return this.http.get(
      `${environment.apiUrl}/teacher/set-student-attendance/${studentId}/${subjectId}/${date}/${attendance}`
    );
  }

  public getTeacherSubjectsInGroup(groupId: number) {
    return this.http.get<SubjectInfoModel[]>(
      `${environment.apiUrl}/teacher/get-my-subjects-in-group/${groupId}`
    );
  }

  public getTeacherSubjects() {
    return this.http.get<SubjectInfoModel[]>(
      `${environment.apiUrl}/teacher/get-my-subjects`
    );
  }

  public addWork(data: {
    name: string;
    groupId: number;
    subjectId: number;
    items: { name: string; maxGrade: number; removable: boolean }[];
  }) {
    return this.http.post<void>(`${environment.apiUrl}/teacher/add-work`, data);
  }

  addMark(colId: string, uId: string, mark: string) {
    return this.http.get<void>(`${environment.apiUrl}/teacher/add-grade/${uId}/${colId}/${mark}`);
  }

  getScheduleItemsByDay(dayId: number): Observable<ScheduleItemViewModel[]> {
    return this.http.get<ScheduleItemViewModel[]>(
      `${environment.apiUrl}/teacher/get-schedule/${dayId}`
    );
  }
}
