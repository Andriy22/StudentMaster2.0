import { Injectable } from '@angular/core';
import { switchMap, tap } from 'rxjs/operators';
import { NgxPermissionsService, NgxRolesService } from 'ngx-permissions';
import { AuthService, User } from '@core/authentication';
import { Menu, MenuService } from './menu.service';

@Injectable({
  providedIn: 'root',
})
export class StartupService {
  constructor(
    private authService: AuthService,
    private menuService: MenuService,
    private rolesService: NgxRolesService,
    private permissionService: NgxPermissionsService
  ) {}

  /**
   * Load the application only after get the menu or other essential informations
   * such as permissions and roles.
   */
  load() {
    return new Promise<void>((resolve, reject) => {
      this.authService
        .change()
        .pipe(
          tap(user => this.setPermissions(user)),
          switchMap(() => this.authService.menu()),
          tap(menu => this.setMenu(menu))
        )
        .subscribe(
          () => resolve(),
          () => resolve()
        );
    });
  }

  private setMenu(menu: Menu[]) {
    this.menuService.addNamespace(menu, 'menu');
    this.menuService.set(menu);
  }

  private setPermissions(user: User) {
    const roles: string[] = [];

    user?.roles?.forEach((role: string) => {
      roles.push(role);
    });

    this.permissionService.loadPermissions(roles);
  }
}
