import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { Homework } from '@shared/models/homeworks.model';

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

  public addHomework(homework: FormData) {
    return this.http.post(`${environment.apiUrl}/homework/create-homework`, homework);
  }

  public editHomework(homework: FormData) {
    return this.http.put(`${environment.apiUrl}/homework/edit-homework`, homework);
  }

  public deleteHomework(id: number) {
    return this.http.delete(`${environment.apiUrl}/homework/delete-homework/${id}`);
  }
}
