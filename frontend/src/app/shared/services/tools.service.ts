import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class ToolsService {
  constructor(private notify: MatSnackBar) {}

  showNotification(error = 'Unknown error...') {
    this.notify.open(error, '', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
