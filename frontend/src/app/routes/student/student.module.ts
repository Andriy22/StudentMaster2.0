import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { StudentRoutingModule } from './student-routing.module';
import { StudentRegisterComponent } from './register/register.component';
import { StudentStudentScheduleComponent } from './student/schedule/schedule.component';
import { StudentChatsComponent } from './chats/chats.component';
import { StudentMaterialsComponent } from './materials/materials.component';
import { StudentHomeworksComponent } from './homeworks/homeworks.component';
import { StudentHomeworkComponent } from './homework/homework.component';

const COMPONENTS: any[] = [StudentRegisterComponent, StudentStudentScheduleComponent, StudentChatsComponent, StudentMaterialsComponent, StudentHomeworksComponent, StudentHomeworkComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [
    SharedModule,
    StudentRoutingModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_DYNAMIC
  ]
})
export class StudentModule { }
