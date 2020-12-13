using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.ExceptionObjects
{
    public class AccountNotFoundException : ArgumentException
    {
        public AccountNotFoundException(Account account)
        {
            this.account = account;
        }

        public AccountNotFoundException(Account account, string message)
            : this(account)
        {
            this.message = message;
        }
        public AccountNotFoundException(string message)
        {
            this.message = message;
        }

        public Account account { get; private set; }
        private string message = "Usuário não encontrado {0}";

        public override string Message {
            get {
                if (!account.IsNull())
                    return string.Format(message, account.Email);
                else
                    return string.Format(message);
            }
        }
    }
}
