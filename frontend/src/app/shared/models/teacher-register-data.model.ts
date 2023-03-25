export interface TeacherRegisterDataModel {
  header: string;
  name?: string;
  items: TeacherRegisterDataItemModel[];
}

export interface TeacherRegisterDataItemModel {
  title: string;
  value: string;
  name: string;
  id?: string;
  limit: string;
  editable: boolean;
}
