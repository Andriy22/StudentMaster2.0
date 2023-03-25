import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Token, User } from './interface';
import { Menu } from '@core';
import { map } from 'rxjs/operators';
import {environment} from "@env/environment";

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  constructor(protected http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<Token>(`${environment.apiUrl}/auth/login`, { email, password });
  }

  refresh(params: Record<string, any>) {
    return this.http.post<Token>(`${environment.apiUrl}/auth/refresh`, params);
  }

  logout() {
    return this.http.post<any>(`${environment.apiUrl}/auth/logout`, {});
  }

  me() {
    return this.http.get<User>(`${environment.apiUrl}/account/me`);
  }

  menu() {
    return this.http.get<{ menu: Menu[] }>('/assets/data/menu.json').pipe(map(res => res.menu));
  }
}
