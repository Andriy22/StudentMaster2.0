export interface StudentRegisterDataModel {
  header: string;
  items: StudentRegisterDataItemModel[];
}

export interface StudentRegisterDataItemModel {
  title: string;
  value: string;
  name: string;
  id?: string;
}
