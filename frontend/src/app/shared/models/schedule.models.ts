import { SubjectShortInfoModel } from '@shared/models/subject-short-info.model';

export interface ScheduleDayViewModel {
  id: number;
  name: string;
}

export interface ScheduleItemTypeViewModel {
  id: number;
  name: string;
}

export interface ScheduleItemViewModel {
  id: number;
  position: number;
  subject: string;
  start: string;
  end: string;
  url: string;
  comment: string;
  subjectShortInfo: SubjectShortInfoModel;
  scheduleDay: ScheduleDayViewModel;
  scheduleItemType: ScheduleItemTypeViewModel;
}
