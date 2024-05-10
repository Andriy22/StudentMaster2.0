import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { CrudEducationMaterial } from '@shared/models/education-material.models';

@Injectable({
  providedIn: 'root',
})
export class AdditionalMaterialsService {
  constructor(private _http: HttpClient) {}

  public getAdditionalMaterials(subjectId: number, groupId?: number) {
    return this._http.get<CrudEducationMaterial[]>(
      `${environment.apiUrl}/educationMaterial/get-education-materials/${subjectId}/${
        groupId ? groupId : 0
      }`
    );
  }

  public createAdditionalMaterial(material: FormData) {
    return this._http.post(
      `${environment.apiUrl}/educationMaterial/create-education-material`,
      material
    );
  }

  public editAdditionalMaterial(material: FormData) {
    return this._http.put(
      `${environment.apiUrl}/educationMaterial/edit-education-material`,
      material
    );
  }

  public removeAdditionalMaterial(id: number) {
    return this._http.delete(
      `${environment.apiUrl}/educationMaterial/delete-education-material/${id}`
    );
  }
}
