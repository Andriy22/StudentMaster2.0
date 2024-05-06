import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { CrudEducationMaterial } from '@shared/models/education-material.models';

@Injectable({
  providedIn: 'root'
})
export class AdditionalMaterialsService {

  constructor(private _http: HttpClient) { }

  public getAdditionalMaterials(subjectId: number, groupId?: number) {
    return this._http.get<CrudEducationMaterial[]>(`${environment.apiUrl}/educationMaterial/get-education-materials/${subjectId}/${groupId? groupId : 0}`);
  }
}
