using FluentValidation;
using TelemetryMicroservice.Domain.Dtos;

namespace TelemetryMicroservice.Domain.Validators;

public class AddTelemetryDtoValidator:AbstractValidator<AddTelemetryDto>
{
    public AddTelemetryDtoValidator()
    {
        RuleFor(t => t.VehicleId).NotEmpty().WithMessage("Provide a valid VehicleId");
        RuleFor(t => t.ActualSpeed).Must(v=>v >= 0).WithMessage("Provide a positive actual speed");
        RuleFor(t => t.KilometersSinceStart).Must(v=>v >= 0).WithMessage("Provide a positive kilometers since start value");
        RuleFor(t => t.Fuel).Must(t => t is >= 0 and <= 100).WithMessage("Provide a valid percentage for fuel");
        RuleFor(t => t.TirePressure).Must(t => t >= 0).WithMessage("Provide a positive value for tire pressure");
    }
}
