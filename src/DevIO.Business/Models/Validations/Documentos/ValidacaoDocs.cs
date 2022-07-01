using System;
using System.Collections.Generic;
using System.Linq;

namespace DevIO.Business.Models.Validations.Documentos
{
    public class CpfValidacao
    {
        public const int TAMANHO_CPF = 11;

        public static bool Validar(string cpf)
        {
            string _cpfNumeros = Utils.ApenasNumeros(cpf);

            if (!TamanhoValido(_cpfNumeros))
                return false;

            return !TemDigitosRepetidos(_cpfNumeros) && TemDigitosValidos(_cpfNumeros);
        }

        private static bool TamanhoValido(string valor)
        {
            return valor.Length == TAMANHO_CPF;
        }

        private static bool TemDigitosRepetidos(string valor)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999",
            };

            return invalidNumbers.Contains(valor);
        }

        private static bool TemDigitosValidos(string valor)
        {
            string _number = valor.Substring(0, TAMANHO_CPF - 2);

            DigitoVerificador _digitoVerificador = new DigitoVerificador(_number).ComMultiplicadoresDeAte(primeiroMultiplicador: 2, 
                                                                                                          ultimoMultiplicador: 11)
                                                                                 .Substituindo("0", 10, 11);

            string _firstDigit = _digitoVerificador.CalculaDigito();
            
            _digitoVerificador.AddDigito(_firstDigit);

            string _secondDigit = _digitoVerificador.CalculaDigito();

            return string.Concat(_firstDigit, _secondDigit) == valor.Substring(TAMANHO_CPF - 2, 2);
        }
    }

    public class CnpjValidacao
    {
        public const int TAMANHO_CNPJ = 14;

        public static bool Validar(string cpnj)
        {
            string _cnpjNumeros = Utils.ApenasNumeros(cpnj);

            if (!TemTamanhoValido(_cnpjNumeros))
                return false;

            return !TemDigitosRepetidos(_cnpjNumeros) && TemDigitosValidos(_cnpjNumeros);
        }

        private static bool TemTamanhoValido(string valor)
        {
            return valor.Length == TAMANHO_CNPJ;
        }

        private static bool TemDigitosRepetidos(string valor)
        {
            string[] invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };

            return invalidNumbers.Contains(valor);
        }

        private static bool TemDigitosValidos(string valor)
        {
            string _number = valor.Substring(0, TAMANHO_CNPJ - 2);

            DigitoVerificador _digitoVerificador = new DigitoVerificador(_number).ComMultiplicadoresDeAte(2, 9)
                                                                                 .Substituindo("0", 10, 11);

            string _firstDigit = _digitoVerificador.CalculaDigito();

            _digitoVerificador.AddDigito(_firstDigit);

            string _secondDigit = _digitoVerificador.CalculaDigito();

            return string.Concat(_firstDigit, _secondDigit) == valor.Substring(TAMANHO_CNPJ - 2, 2);
        }
    }

    public class DigitoVerificador
    {
        private const int MODULO = 11;

        private string _numero;
        private readonly List<int> _multiplicadores = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _substituicoes = new Dictionary<int, string>();
        private bool _complementarDoModulo = true;

        public DigitoVerificador(string numero)
        {
            _numero = numero;
        }

        public DigitoVerificador ComMultiplicadoresDeAte(int primeiroMultiplicador, int ultimoMultiplicador)
        {
            _multiplicadores.Clear();

            for (int indice = primeiroMultiplicador; indice <= ultimoMultiplicador; indice++)
                _multiplicadores.Add(indice);

            return this;
        }

        public DigitoVerificador Substituindo(string substituto, params int[] digitos)
        {
            foreach (int digito in digitos)
                _substituicoes[digito] = substituto;

            return this;
        }

        public void AddDigito(string digito)
        {
            _numero = string.Concat(_numero, digito);
        }

        public string CalculaDigito()
        {
            return !(_numero.Length > 0) ? string.Empty : GetDigitSum();
        }

        private string GetDigitSum()
        {
            int _soma = 0;

            for (int indice = _numero.Length - 1, multiplicador = 0; indice >= 0; indice--)
            {
                int _produto = (int)char.GetNumericValue(_numero[indice]) * _multiplicadores[multiplicador];
                _soma += _produto;

                if (++multiplicador >= _multiplicadores.Count) 
                    multiplicador = 0;
            }

            int _modulo = (_soma % MODULO);
            int _resultado = _complementarDoModulo ? MODULO - _modulo : _modulo;

            return _substituicoes.ContainsKey(_resultado) ? _substituicoes[_resultado] : _resultado.ToString();
        }
    }

    public class Utils
    {
        public static string ApenasNumeros(string valor)
        {
            string _onlyNumber = string.Empty;

            foreach (char caractere in valor)
            {
                if (char.IsDigit(caractere))
                    _onlyNumber += caractere;
            }

            return _onlyNumber.Trim();
        }
    }
}
