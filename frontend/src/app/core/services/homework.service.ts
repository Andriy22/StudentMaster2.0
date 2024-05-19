import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { Homework, HomeworkStudent, ReviewHomework } from '@shared/models/homeworks.model';

@Injectable({
  providedIn: 'root',
})
export class HomeworkService {
  constructor(private http: HttpClient) {}

  public getHomeworks(groupId: number, subjectId: number) {
    return this.http.get<Homework[]>(
      `${environment.apiUrl}/homework/get-homeworks/${subjectId}/${groupId}`
    );
  }

  public getHomework(homeworkId: number) {
    return this.http.get<Homework>(`${environment.apiUrl}/homework/get-homework/${homeworkId}`);
  }

  public addHomework(homework: FormData) {
    return this.http.post(`${environment.apiUrl}/homework/create-homework`, homework);
  }

  public editHomework(homework: FormData) {
    return this.http.put(`${environment.apiUrl}/homework/edit-homework`, homework);
  }

  public deleteHomework(id: number) {
    return this.http.delete(`${environment.apiUrl}/homework/delete-homework/${id}`);
  }

  public submitHomework(homework: FormData) {
    return this.http.post(`${environment.apiUrl}/homework/send-homework-to-review`, homework);
  }

  public reviewHomework(homework: ReviewHomework) {
    return this.http.post(`${environment.apiUrl}/homework/review-homework`, homework);
  }

  public getHomeworkStudent(homeworkId: number) {
    return this.http.get<HomeworkStudent[]>(
      `${environment.apiUrl}/homework/get-homework-student/${homeworkId}`
    );
  }

  public getHomeworksForReview(homeworkId: number) {
    return this.http.get<HomeworkStudent[]>(
      `${environment.apiUrl}/homework/get-homeworks-for-review/${homeworkId}`
    );
  }

  public cancelHomeworkToReview(id: number) {
    return this.http.delete(`${environment.apiUrl}/homework/cancel-homework-to-review/${id}`);
  }
}
