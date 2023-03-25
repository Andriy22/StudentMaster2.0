import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { StudentRegisterDataModel } from '@shared/models/student-register-data.model';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  constructor(private http: HttpClient) {}

  public getRegisterData(subjectId: number, isExtended: boolean) {
    return this.http.get<StudentRegisterDataModel[]>(
      `${environment.apiUrl}/student/get-register-data/${subjectId}/${isExtended}`
    );
  }
}
