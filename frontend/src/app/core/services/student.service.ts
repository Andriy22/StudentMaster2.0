import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { StudentRegisterDataModel } from '@shared/models/student-register-data.model';
import { Observable } from 'rxjs';
import { SubjectShortInfoModel } from '@shared/models/subject-short-info.model';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  constructor(private http: HttpClient) {}

  public getRegisterData(
    subjectId: number,
    isExtended: boolean
  ): Observable<StudentRegisterDataModel[][]> {
    return this.http.get<StudentRegisterDataModel[][]>(
      `${environment.apiUrl}/student/get-register-data/${subjectId}/${isExtended}`
    );
  }

  public getSubjects(): Observable<SubjectShortInfoModel[]> {
    return this.http.get<SubjectShortInfoModel[]>(`${environment.apiUrl}/student/get-subjects`);
  }
}
