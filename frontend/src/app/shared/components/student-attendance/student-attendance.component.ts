import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ToolsService } from '@shared/services/tools.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AttendanceService } from '@core/services/attendance.service';

@Component({
  selector: 'app-student-attendance',
  templateUrl: './student-attendance.component.html',
  styleUrls: ['./student-attendance.component.scss'],
})
export class StudentAttendanceComponent implements OnInit, OnDestroy {
  isLoading = false;

  displayedColumns: string[] = ['date', 'isPresent'];

  dataSource: any[] = [];

  $subscription: Subscription | undefined;

  constructor(
    private tools: ToolsService,
    public dialogRef: MatDialogRef<StudentAttendanceComponent>,
    private attendance: AttendanceService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit() {
    this.attendance
      .getStudentAttendance(this.data.studentId, this.data.subjectId)
      .subscribe(data => (this.dataSource = data));
  }

  cancel() {
    this.dialogRef.close();
  }

  ngOnDestroy(): void {
    this.$subscription?.unsubscribe();
  }
}
