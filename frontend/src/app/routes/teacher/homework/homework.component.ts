import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-teacher-homework',
  templateUrl: './homework.component.html',
  styleUrls: ['./homework.component.scss'],
})
export class TeacherHomeworkComponent implements OnInit {
  @Input() groupId = -1;

  constructor() {}

  ngOnInit() {}
}
