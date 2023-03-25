import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { environment } from '@env/environment';

import { AdminLayoutComponent } from '../theme/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from '../theme/auth-layout/auth-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './sessions/login/login.component';
import { RegisterComponent } from './sessions/register/register.component';
import { Error403Component } from './sessions/403.component';
import { Error404Component } from './sessions/404.component';
import { Error500Component } from './sessions/500.component';
import { AuthGuard } from '@core';
import { NgxPermissionsGuard } from 'ngx-permissions';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: '403', component: Error403Component },
      { path: '404', component: Error404Component },
      { path: '500', component: Error500Component },
      {
        path: 'account',
        loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
      },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
        data: {
          permissions: { only: 'Admin', redirectTo: '/teacher' },
        },
        canActivate: [NgxPermissionsGuard],
      },
      {
        path: 'teacher',
        loadChildren: () => import('./teacher/teacher.module').then(m => m.TeacherModule),
        data: {
          permissions: { only: 'Teacher', redirectTo: '/dashboard' },
        },
        canActivate: [NgxPermissionsGuard],
      },
      {
        path: 'student',
        loadChildren: () => import('./student/student.module').then(m => m.StudentModule),
      },
    ],
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ],
  },
  { path: '**', redirectTo: '404' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: environment.useHash,
    }),
  ],
  exports: [RouterModule],
})
export class RoutesRoutingModule {}
