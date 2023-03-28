import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StudentAttendanceModel } from '@shared/models/student-attendance.model';
import { environment } from '@env/environment';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  constructor(private http: HttpClient) {}

  public getStudentAttendance(studentId: string, subjectId: number) {
    return this.http.get<StudentAttendanceModel[]>(
      `${environment.apiUrl}/attendance/get-student-attendance/${studentId}/${subjectId}`
    );
  }
}
