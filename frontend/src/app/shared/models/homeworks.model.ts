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
  filePath?: string;
  deadline: string;
  hasUnreviewedSubmissions?: boolean;
};

export interface HomeworkStudent {
  id: number;
  status: number;
  grade: number | null;
  comment: string;
  isDeleted: boolean;
  isModified: boolean;
  createdAt: string;
  updatedAt: string;
  reviewedAt: string;
  ownerName: string;
  filePath: string;
  reviewerName: string;
}

export type ReviewHomework = {
  id: number;
  grade?: number | null;
  status: number;
  comment: string;
};
