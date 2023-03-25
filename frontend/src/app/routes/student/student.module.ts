import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { StudentRoutingModule } from './student-routing.module';
import { StudentRegisterComponent } from './register/register.component';

const COMPONENTS: any[] = [StudentRegisterComponent];
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
