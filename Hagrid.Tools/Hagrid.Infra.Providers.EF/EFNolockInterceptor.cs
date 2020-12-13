using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hagrid.Infra.Providers.EntityFramework
{
    public class EFNolockInterceptor : DbCommandInterceptor
    {
        private static readonly Regex _tableAliasRegex = new Regex(@"(?<tableAlias>AS \[Extent\d+\](?! WITH \(NOLOCK\)))");

        [ThreadStatic]
        public static bool Suppress;

        [ThreadStatic]
        public static string CommandText;

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            IncludeNolock(command);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            IncludeNolock(command);
        }

        private void IncludeNolock(DbCommand command)
        {
            if (!Suppress)
            {

                var aliasMatches = _tableAliasRegex.Matches(command.CommandText);

                var replaced = new List<string>();

                foreach (Match match in aliasMatches)
                {
                    if (!replaced.Contains(match.Value))
                    {
                        command.CommandText = command.CommandText.Replace(match.Value, string.Format("{0} WITH (NOLOCK)", match.Value.Trim()));
                        replaced.Add(match.Value);
                    }
                }

                CommandText = command.CommandText;
            }
            else
            {
                Suppress = false;
            }
        }
    }
}
