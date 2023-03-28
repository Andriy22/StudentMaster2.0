export interface StudentRegisterDataModel {
  header: string;
  name?: string;
  items: StudentRegisterDataItemModel[];
}

export interface StudentRegisterDataItemModel {
  title: string;
  value: string;
  name: string;
  id?: string;
  limit: string;
  editable: boolean;
}
