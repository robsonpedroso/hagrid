using Newtonsoft.Json;
using Hagrid.Core.Domain.Contracts.Policies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.Policies
{
    public class PasswordPolicy : IPasswordPolicy
    {
        public bool Validate(string email, string password, bool throwEx = true)
        {
            var ex = new PasswordException();

            Action<PasswordIssueType> newIssue = (type) =>
            {
                ex.AddIssue(type);
                if (throwEx) throw ex;
            };

            if (string.IsNullOrWhiteSpace(password))
                newIssue(PasswordIssueType.Empty);

            if (password.Equals(email))
                newIssue(PasswordIssueType.EqualsToEmail);

            if (new Regex("^[*]+$").IsMatch(password))
                newIssue(PasswordIssueType.OnlyAsterisk);

            if (password.Length < 8)
                ex.AddIssue(PasswordIssueType.LengthMin);

            if (password.Length > 50)
                ex.AddIssue(PasswordIssueType.LengthMax);

            if (new Regex(@"\s+").IsMatch(password))
                ex.AddIssue(PasswordIssueType.WhiteSpace);

            if (password.HasUnicodeChar())
                ex.AddIssue(PasswordIssueType.InvalidChar);

            if (ex.HasIssues)
            {
                if (throwEx) throw ex;

                return false;
            }

            return true;
        }
    }

    public class PasswordException : ArgumentException
    {
        private List<PasswordIssueType> issues;

        public bool HasIssues { get { return (issues != null && issues.Count > 0); } }

        public PasswordException()
        {
            issues = new List<PasswordIssueType>();
        }

        internal PasswordException AddIssue(PasswordIssueType issue)
        {
            issues.Add(issue);
            return this;
        }

        public object Issues { get { return issues; } }

        public override string Message
        {
            get
            {
                return JsonConvert.SerializeObject(issues, new Newtonsoft.Json.Converters.StringEnumConverter());
            }
        }
    }

    internal enum PasswordIssueType
    {
        [Description("Não pode estar em branco.")]
        Empty,
        [Description("Não pode ser seu e-mail.")]
        EqualsToEmail,
        [Description("Não pode ser formada somente pelo caractere \"*\" (asterisco).")]
        OnlyAsterisk,
        [Description("Deve conter ao menos 8 (oito) caracteres.")]
        LengthMin,
        [Description("Deve ser menor que 50 (cinquenta) caracteres.")]
        LengthMax,
        [Description("Não pode conter espaços.")]
        WhiteSpace,
        [Description("Possui caracteres inválidos.")]
        InvalidChar
    }
}
