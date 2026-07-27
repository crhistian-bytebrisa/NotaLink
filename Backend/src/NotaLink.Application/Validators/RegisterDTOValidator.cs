using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NotaLink.Application.DTOs.Auth;
using NotaLink.Domain.Entities;

namespace NotaLink.Application.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        private readonly UserManager<User> userManager;

        public RegisterDTOValidator(UserManager<User> userManager)
        {
            this.userManager = userManager;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.")
                .Matches(@"^[^\r\n]*$").WithMessage("El nombre de usuario no puede contener saltos de línea.")
                .MustAsync(async (username, cancellation) =>
                {
                    var existingUser = await userManager.FindByNameAsync(username);
                    return existingUser is null;
                })
                .WithMessage("El nombre de usuario ya está registrado."); ;


            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El formato del email no es válido.")
                .MustAsync(async (email, cancellation) =>
                {
                    var existingUser = await userManager.FindByEmailAsync(email);
                    return existingUser is null;
                })
                .WithMessage("El email ya está registrado.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("La contraseña debe tener al menos una mayúscula.")
                .Matches("[0-9]").WithMessage("La contraseña debe tener al menos un número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe tener al menos un carácter especial.");
        }
    }
}
