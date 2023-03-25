import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountProfileComponent } from './profile/profile.component';

const routes: Routes = [{ path: 'profile', component: AccountProfileComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {}
