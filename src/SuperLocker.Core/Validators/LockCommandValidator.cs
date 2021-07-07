using FluentValidation;
using SuperLocker.Core.Command;

public class UnlockCommandValidator : AbstractValidator<UnlockCommand> {
	public UnlockCommandValidator() {
		RuleFor(x => x.LockId).NotNull();
		RuleFor(x => x.UserId).NotNull();
	}
}