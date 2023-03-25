import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { AccountRoutingModule } from './account-routing.module';
import { AccountProfileComponent } from './profile/profile.component';
import { AccountSettingsComponent } from './settings/settings.component';

const COMPONENTS: any[] = [AccountProfileComponent, AccountSettingsComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [
    SharedModule,
    AccountRoutingModule
  ],
  declarations: [
    ...COMPONENTS,
    ...COMPONENTS_DYNAMIC
  ]
})
export class AccountModule { }
