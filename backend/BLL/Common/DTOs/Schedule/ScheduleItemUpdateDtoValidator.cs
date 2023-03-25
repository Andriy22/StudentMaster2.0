using backend.BLL.Common.DTOs.Schedule;
using FluentValidation;

public class ScheduleItemUpdateDtoValidator : AbstractValidator<EditScheduleItemDTO>
{
    public ScheduleItemUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Start)
            .NotEmpty()
            .Matches(@"^(\d{2}:\d{2})$");

        RuleFor(x => x.End)
            .NotEmpty()
            .Matches(@"^(\d{2}:\d{2})$");

        RuleFor(x => x.Comment)
            .MaximumLength(500);

        RuleFor(x => x.SubjectId)
            .NotEmpty();

        RuleFor(x => x.ScheduleItemTypeId)
            .NotEmpty();
    }
}