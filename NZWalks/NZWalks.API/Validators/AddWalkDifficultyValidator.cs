using FluentValidation;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficultyValidator : AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public AddWalkDifficultyValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
