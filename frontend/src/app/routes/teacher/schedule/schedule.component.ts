import { Component, OnInit } from '@angular/core';
import { ScheduleService } from '@core/services/schedule.service';
import { ScheduleDayViewModel, ScheduleItemViewModel } from '@shared/models/schedule.models';
import { MatTableDataSource } from '@angular/material/table';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { TeacherService } from '@core/services/teacher.service';

@Component({
  selector: 'app-teacher-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class TeacherScheduleComponent implements OnInit {
  scheduleDays: ScheduleDayViewModel[] = [];

  displayedColumns: string[] = ['subject', 'type', 'start', 'end', 'url', 'comment'];

  dataSource = new MatTableDataSource<ScheduleItemViewModel>();

  selectedDay = 2;

  constructor(private scheduleService: ScheduleService, private teacherService: TeacherService) {}

  ngOnInit() {
    this.scheduleService.getScheduleDays().subscribe(days => (this.scheduleDays = days));
  }

  onDayChanged(event: MatTabChangeEvent) {
    const _id = this.scheduleDays.find(x => x.name == event.tab.textLabel)?.id;

    if (_id) {
      this.teacherService.getScheduleItemsByDay(_id).subscribe(data => {
        this.dataSource.data = data;
      });
    }
  }
}
