import { Component, OnInit } from '@angular/core';
import { ScheduleDayViewModel, ScheduleItemViewModel } from '@shared/models/schedule.models';
import { MatTableDataSource } from '@angular/material/table';
import { ScheduleService } from '@core/services/schedule.service';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { StudentService } from '@core/services/student.service';

@Component({
  selector: 'app-student-student-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class StudentStudentScheduleComponent implements OnInit {
  scheduleDays: ScheduleDayViewModel[] = [];

  displayedColumns: string[] = ['subject', 'type', 'start', 'end', 'url', 'comment'];

  dataSource = new MatTableDataSource<ScheduleItemViewModel>();

  selectedDay = 2;

  constructor(private scheduleService: ScheduleService, private studentService: StudentService) {}

  ngOnInit() {
    this.scheduleService.getScheduleDays().subscribe(days => (this.scheduleDays = days));
  }

  onDayChanged(event: MatTabChangeEvent) {
    const _id = this.scheduleDays.find(x => x.name == event.tab.textLabel)?.id;

    if (_id) {
      this.studentService.getScheduleItemsByDay(_id).subscribe(data => {
        this.dataSource.data = data;
      });
    }
  }
}
