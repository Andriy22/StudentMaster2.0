import { EducationMaterialType } from '@shared/enums/education-material-type.enums';

export interface CrudEducationMaterial {
  id: number;
  title: number;
  type: EducationMaterialType,
  subjectId: number;
  url: string;
  userId: number;
  groups: number[];
}
