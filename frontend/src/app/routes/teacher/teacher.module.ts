import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TeacherRoutingModule } from './teacher-routing.module';
import { TeacherRegisterComponent } from './register/register.component';
import { TeacherRegisterAddWorkComponent } from './register/add-work/add-work.component';
import { TeacherGroupsComponent } from './groups/groups.component';
import { TeacherGroupComponent } from './group/group.component';
import { TeacherDashboardComponent } from './dashboard/dashboard.component';
import { TeacherScheduleComponent } from './schedule/schedule.component';
import { TeacherGroupAttendanceComponent } from './group/attendance/attendance.component';

const COMPONENTS: any[] = [TeacherRegisterComponent, TeacherGroupsComponent, TeacherGroupComponent, TeacherDashboardComponent, TeacherScheduleComponent];
const COMPONENTS_DYNAMIC: any[] = [TeacherRegisterAddWorkComponent, TeacherGroupAttendanceComponent];

@NgModule({
  imports: [
    SharedModule,
    TeacherRoutingModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_DYNAMIC
  ]
})
export class TeacherModule { }
