using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class EnderecoValidation : AbstractValidator<Endereco>
    {
        public EnderecoValidation()
        {
            RuleFor(endereco => endereco.Logradouro)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(min: 2, max: 200)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(endereco => endereco.Bairro)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(min: 2, max:100)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(endereco => endereco.Cep)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(8)
                    .WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(endereco => endereco.Cidade)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(min: 2, max: 100)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(endereco => endereco.Estado)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(min: 2, max: 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(endereco => endereco.Numero)
                .NotEmpty()
                    .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(min: 1, max: 50)
                    .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
