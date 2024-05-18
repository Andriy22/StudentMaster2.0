export type Homework = {
  id?: number;
  title: string;
  type: number;
  description: string;
  maxGrade: number;
  file?: File;
  subjectId: number;
  groupId: number;
  createdAt?: string;
  updatedAt?: string;
  hasUnreviewedSubmissions?: boolean;
};
