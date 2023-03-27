using backend.BLL.Common.VMs.Subject;

namespace backend.BLL.Services.Interfaces;

public interface ISubjectService
{
    Task<List<SubjectInfoVM>> GetSubjectsAsync();
    Task CreateSubjectAsync(string subjectName);
    Task ChangeSubjectStatusAsync(int subjectId);
    Task<List<SubjectInfoVM>> GetTeacherSubjectsAsync(string teacherId);
    Task AddSubjectToTeacher(int subjectId, string teacherId);
    Task RemoveSubjectFromTeacher(int subjectId, string teacherId);
}