using FluentValidation;

namespace backend.BLL.Common.DTOs.Work;

public class CreateWorkItemDTOValidator : AbstractValidator<CreateWorkItemDTO>
{
    public CreateWorkItemDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Criteria name can't be empty!");
        RuleFor(x => x.MaxGrade).GreaterThan(0).WithMessage("Max grade can't be less than 1");
    }
}