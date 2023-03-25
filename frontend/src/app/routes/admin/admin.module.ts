import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminUsersComponent } from './users/users.component';
import { NgxPermissionsModule } from 'ngx-permissions';
import { CreateUserComponent } from './users/modals/create-user/create-user.component';
import { AdminConsoleComponent } from './console/console.component';
import { AdminSubjectsComponent } from './subjects/subjects.component';
import { AdminSubjectsAddSubjectComponent } from './subjects/add-subject/add-subject.component';
import { AdminGroupsComponent } from './groups/groups.component';
import { AdminGroupsAddGroupComponent } from './groups/add-group/add-group.component';
import { AdminGroupsEditGroupComponent } from './groups/edit-group/edit-group.component';
import { AdminUsersChangeStudentGroupComponent } from './users/change-student-group/change-student-group.component';
import { AdminGroupComponent } from './group/group.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminGroupChangeGroupSubjectsComponent } from './group/change-group-subjects/change-group-subjects.component';
import { AdminGroupChangeGroupTeachersComponent } from './group/change-group-teachers/change-group-teachers.component';
import { AdminUsersChangeTeacherSubjectsComponent } from './users/change-teacher-subjects/change-teacher-subjects.component';
import { AdminGroupRegisterComponent } from './group/register/register.component';
import { AdminGroupScheduleEditorComponent } from './group/schedule-editor/schedule-editor.component';

const COMPONENTS: any[] = [
  AdminUsersComponent,
  CreateUserComponent,
  AdminConsoleComponent,
  AdminSubjectsComponent,
  AdminGroupsComponent,
  AdminGroupComponent,
  AdminDashboardComponent,
];
const COMPONENTS_DYNAMIC: any[] = [
  CreateUserComponent,
  AdminSubjectsAddSubjectComponent,
  AdminGroupsAddGroupComponent,
  AdminGroupsEditGroupComponent,
  AdminUsersChangeStudentGroupComponent,
  AdminGroupChangeGroupSubjectsComponent,
  AdminGroupChangeGroupTeachersComponent,
  AdminUsersChangeTeacherSubjectsComponent,
  AdminGroupRegisterComponent,
  AdminGroupScheduleEditorComponent,
];

@NgModule({
  imports: [
    SharedModule,
    AdminRoutingModule,
    NgxPermissionsModule.forChild({
      permissionsIsolate: true,
      rolesIsolate: true,
    }),
  ],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class AdminModule {}
