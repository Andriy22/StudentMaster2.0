export interface PresetItemViewModel {
  name: string;
  maxGrade: number;
  removable: boolean;
}

export interface PresetViewModel {
  name: string;
  items: PresetItemViewModel[];
}
