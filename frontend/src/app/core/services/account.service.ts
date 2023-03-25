import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import {
  AbstractControl,
  ValidationErrors,
  ɵElement,
  ɵFormGroupRawValue,
  ɵGetProperty,
  ɵTypedOrUntyped,
} from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  public changePassword(password: string, newPassword: string, confirmPassword: string) {
    return this.http.post(`${environment.apiUrl}/account/change-password`, {
      password,
      newPassword,
      confirmPassword,
    });
  }

  public confirmAccount(
    code: number,
    password:
      | ɵGetProperty<
          ɵTypedOrUntyped<
            {
              [K in keyof {
                confirmationCode: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
                password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                confirmPassword: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
              }]: ɵElement<
                {
                  confirmationCode: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                  password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                  confirmPassword: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                }[K],
                never
              >;
            },
            ɵFormGroupRawValue<{
              [K in keyof {
                confirmationCode: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
                password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                confirmPassword: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
              }]: ɵElement<
                {
                  confirmationCode: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                  password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                  confirmPassword: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                }[K],
                never
              >;
            }>,
            any
          >,
          'password'
        >
      | undefined,
    confirmPassword:
      | ɵGetProperty<
          ɵTypedOrUntyped<
            {
              [K in keyof {
                confirmationCode: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
                password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                confirmPassword: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
              }]: ɵElement<
                {
                  confirmationCode: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                  password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                  confirmPassword: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                }[K],
                never
              >;
            },
            ɵFormGroupRawValue<{
              [K in keyof {
                confirmationCode: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
                password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                confirmPassword: (
                  | string
                  | ((control: AbstractControl) => ValidationErrors | null)[]
                )[];
              }]: ɵElement<
                {
                  confirmationCode: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                  password: (string | ((control: AbstractControl) => ValidationErrors | null)[])[];
                  confirmPassword: (
                    | string
                    | ((control: AbstractControl) => ValidationErrors | null)[]
                  )[];
                }[K],
                never
              >;
            }>,
            any
          >,
          'confirmPassword'
        >
      | undefined
  ) {
    return this.http.post<void>(`${environment.apiUrl}/account/confirm-account`, {
      code,
      password,
      confirmPassword,
    });
  }

  public changeAvatar(data: FormData) {
    return this.http.post<void>(`${environment.apiUrl}/account/change-avatar`, data);
  }
}
