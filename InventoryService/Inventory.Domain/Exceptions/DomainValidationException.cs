using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Exceptions
{
    /// <summary>
    /// Exceção para regras de negócio específica para evitar criação de objetos inválidos.
    /// </summary>
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message)
        {
        }
        static readonly HashSet<char> Apostrofos = new() { '\'', '’', '‘' };
        /// <summary>
        /// Validação contra condição e erro
        /// </summary>
        public static void When(bool condition, string error)
        {
            if (condition)
            {
                throw new DomainValidationException(error);
            }
        }
        /// <summary>
        /// Validação contra caracteres invalidos
        ///  '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v',
        /// '#', '$', '%',                         
        ///  '\\', '^', '`', '{', '|', '}',           
        ///   '§', '£', '¢', '¬', '¦' , ']','['
        /// </summary>

        public static void ValidateInvalidCharacters(string stringValue)
        {
            char[] invalidCharacters = new char[]
            {
        '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', // Caracteres de escape
        '#', '$', '%',                            // Caracteres especiais
        '\\', '^', '`', '{', '|', '}',             // Outros caracteres especiais
        '§', '£', '¢', '¬', '¦' , ']','[' };

            if (!string.IsNullOrEmpty(stringValue) && Apostrofos.Contains(stringValue[0]))
                When(true, "Não é permitido apóstrofo inicial.");

            if (!string.IsNullOrEmpty(stringValue) && Apostrofos.Contains(stringValue[^1]))
                When(true, "Não é permitido apóstrofo final.");

            var invalidChar = stringValue.FirstOrDefault(c => invalidCharacters.Contains(c));

            if (invalidChar != default(char))
            {
                When(true, $"Caracter '{invalidChar}' é inválido, remova-o e tente novamente");
            }
        }

        /// <summary>
        /// Validação de JSON
        /// </summary>
        public static void ValidateJson(Dictionary<string, string?>? json, string nomeCampo, int maxProperties = 20, int maxValueLength = 255)
        {
            DomainValidationException.When(json == null, $"O campo '{nomeCampo}' não pode ser nulo");

            if (json is not null)
            {
                DomainValidationException.When(
                    json.Count > maxProperties,
                    $"Os parâmetros de '{nomeCampo}' não podem ter mais de {maxProperties} propriedades."
                );

                foreach (var par in json)
                {
                    var value = par.Value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        DomainValidationException.When(
                            value.Length > maxValueLength,
                            $"O valor da chave '{par.Key}' em '{nomeCampo}' excede {maxValueLength} caracteres."
                        );
                    }
                }
            }
        }
        /// <summary>
        /// Validação de ENUM, verifica se está dentro do escopo do JSON esperado
        /// </summary>
        public static void ValidateEnum<TEnum>(TEnum value, string fieldName) where TEnum : struct, Enum
        {
            DomainValidationException.When(
                !Enum.IsDefined(typeof(TEnum), value),
                $"O campo '{fieldName}' não está no formato esperado. Valores válidos: {string.Join(", ", Enum.GetValues<TEnum>())}"
            );
        }

        public static void ValidateGuid(Guid id, string fieldName)
        {
            DomainValidationException.When(
                id == Guid.Empty,
                $"O identificador '{fieldName}' não pode ser nulo ou default"
            );
        }
    }
}
