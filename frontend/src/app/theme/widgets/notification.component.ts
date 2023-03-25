import { Component, OnInit } from '@angular/core';
import { SettingsService } from '@core';

@Component({
  selector: 'app-theme-switch',
  template: `
    <button (click)="changeTheme()" mat-icon-button>
      <mat-icon *ngIf="theme === 'dark'">nightlight</mat-icon>
      <mat-icon *ngIf="theme === 'light'">nightlight</mat-icon>
    </button>
  `,
})
export class NotificationComponent implements OnInit {
  theme = 'dark';
  private htmlElement!: HTMLHtmlElement;

  constructor(private options: SettingsService) {
    this.htmlElement = document.querySelector('html')!;
  }

  ngOnInit() {
    this.options.notify.subscribe(x => {
      if (x.theme) {
        this.theme = x.theme;
      }
    });
  }

  changeTheme() {
    console.log(this.options.getOptions().theme);
    if (this.options.getOptions().theme !== 'dark') {
      this.htmlElement.classList.add('theme-dark');
    } else {
      this.htmlElement.classList.remove('theme-dark');
    }
  }
}
