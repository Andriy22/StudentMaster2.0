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
import { TeacherMaterialsComponent } from './materials/materials.component';
import { TeacherMaterialsAddEditMaterialComponent } from './materials/add-edit-material/add-edit-material.component';
import { TeacherHomeworkComponent } from './homework/homework.component';
import { TeacherHomeworkAddEditHomeworkComponent } from './homework/add-edit-homework/add-edit-homework.component';

const COMPONENTS: any[] = [TeacherRegisterComponent, TeacherGroupsComponent, TeacherGroupComponent, TeacherDashboardComponent, TeacherScheduleComponent, TeacherMaterialsComponent, TeacherHomeworkComponent];
const COMPONENTS_DYNAMIC: any[] = [TeacherRegisterAddWorkComponent, TeacherGroupAttendanceComponent, TeacherMaterialsAddEditMaterialComponent, TeacherHomeworkAddEditHomeworkComponent];

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
