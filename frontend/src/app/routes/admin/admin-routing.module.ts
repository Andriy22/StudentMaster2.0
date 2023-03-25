import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminUsersComponent } from './users/users.component';
import { AdminConsoleComponent } from './console/console.component';
import { AdminSubjectsComponent } from './subjects/subjects.component';
import { AdminGroupsComponent } from './groups/groups.component';
import { AdminGroupComponent } from './group/group.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: '', component: AdminDashboardComponent },
  { path: 'dashboard', component: AdminDashboardComponent },
  {
    path: 'users',
    component: AdminUsersComponent,
  },
  { path: 'console', component: AdminConsoleComponent },
  { path: 'subjects', component: AdminSubjectsComponent },
  { path: 'groups', component: AdminGroupsComponent },
  { path: 'group/:id', component: AdminGroupComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
