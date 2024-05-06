import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TeacherGroupsComponent } from './groups/groups.component';
import { TeacherGroupComponent } from './group/group.component';
import { TeacherDashboardComponent } from './dashboard/dashboard.component';
import { TeacherScheduleComponent } from './schedule/schedule.component';
import { TeacherMaterialsComponent } from './materials/materials.component';

const routes: Routes = [
  { path: '', component: TeacherDashboardComponent },
  { path: 'groups', component: TeacherGroupsComponent },
  { path: 'group/:id', component: TeacherGroupComponent },
  { path: 'dashboard', component: TeacherDashboardComponent },
  { path: 'schedule', component: TeacherScheduleComponent },
  { path: 'materials', component: TeacherMaterialsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TeacherRoutingModule {}
