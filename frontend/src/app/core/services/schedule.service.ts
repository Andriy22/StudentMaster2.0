import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  ScheduleDayViewModel,
  ScheduleItemTypeViewModel,
  ScheduleItemViewModel,
} from '@shared/models/schedule.models';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

@Injectable({
  providedIn: 'root',
})
export class ScheduleService {
  constructor(private http: HttpClient) {}

  getScheduleDays(): Observable<ScheduleDayViewModel[]> {
    return this.http.get<ScheduleDayViewModel[]>(`${environment.apiUrl}/schedule/get-days`);
  }

  getScheduleItemTypes(): Observable<ScheduleItemTypeViewModel[]> {
    return this.http.get<ScheduleItemTypeViewModel[]>(
      `${environment.apiUrl}/schedule/get-item-types`
    );
  }

  getScheduleItemsByGroupAndDay(
    groupId: number,
    dayId: number
  ): Observable<ScheduleItemViewModel[]> {
    return this.http.get<ScheduleItemViewModel[]>(
      `${environment.apiUrl}/schedule/get-schedule-group/${groupId}/day/${dayId}`
    );
  }

  addNewScheduleItem(groupId: number, dayId: number) {
    return this.http.get(`${environment.apiUrl}/schedule/create-new-item/${groupId}/${dayId}`);
  }

  removeScheduleItem(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/schedule/remove-item/${id}`);
  }

  updateScheduleItem(
    id: number,
    subjectId: number,
    scheduleItemTypeId: number,
    url: string,
    start: string,
    end: string,
    comment: string
  ) {
    return this.http.post(`${environment.apiUrl}/schedule/update-item`, {
      id,
      subjectId,
      scheduleItemTypeId,
      url,
      start,
      end,
      comment,
    });
  }
}
