using FluentValidation;
using SuperLocker.Core.Command;

public class LockCommandValidator : AbstractValidator<LockCommand> {
	public LockCommandValidator() {
		RuleFor(x => x.LockId).NotNull();
		RuleFor(x => x.UserId).NotNull();
	}
}