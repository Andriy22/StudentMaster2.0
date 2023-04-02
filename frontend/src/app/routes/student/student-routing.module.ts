import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentRegisterComponent } from './register/register.component';
import { StudentStudentScheduleComponent } from './student/schedule/schedule.component';
import { StudentChatsComponent } from './chats/chats.component';

const routes: Routes = [{ path: 'register', component: StudentRegisterComponent },
{ path: 'schedule', component: StudentStudentScheduleComponent },
{ path: 'chats', component: StudentChatsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
