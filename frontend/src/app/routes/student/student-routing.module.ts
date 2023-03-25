import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentRegisterComponent } from './register/register.component';

const routes: Routes = [{ path: 'register', component: StudentRegisterComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
