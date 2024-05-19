import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentRegisterComponent } from './register/register.component';
import { StudentStudentScheduleComponent } from './student/schedule/schedule.component';
import { StudentChatsComponent } from './chats/chats.component';
import { StudentMaterialsComponent } from './materials/materials.component';
import { StudentHomeworksComponent } from './homeworks/homeworks.component';
import { StudentHomeworkComponent } from './homework/homework.component';

const routes: Routes = [
  { path: 'register', component: StudentRegisterComponent },
  { path: 'schedule', component: StudentStudentScheduleComponent },
  { path: 'chats', component: StudentChatsComponent },
  { path: 'materials', component: StudentMaterialsComponent },
  { path: 'homeworks', component: StudentHomeworksComponent },
  { path: 'homework/:id', component: StudentHomeworkComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StudentRoutingModule {}
