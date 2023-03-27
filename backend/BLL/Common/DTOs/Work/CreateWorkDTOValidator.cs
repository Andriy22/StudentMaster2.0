using FluentValidation;

namespace backend.BLL.Common.DTOs.Work;

public class CreateWorkDTOValidator : AbstractValidator<CreateWorkDTO>
{
    public CreateWorkDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Work name can't be empty");
        RuleFor(x => x.SubjectId).GreaterThanOrEqualTo(0).WithMessage("Subject id can't be less than 1");
        RuleFor(x => x.GroupId).GreaterThanOrEqualTo(0).WithMessage("Group id can't be less than 1");
        RuleForEach(x => x.Items).SetInheritanceValidator(v => { v.Add(new CreateWorkItemDTOValidator()); });
    }
}