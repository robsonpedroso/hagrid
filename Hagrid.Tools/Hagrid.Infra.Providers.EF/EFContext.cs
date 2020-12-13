using System.Data.Entity;

namespace Hagrid.Infra.Providers.EntityFramework
{
    /// <summary>
    ///  A DbContext instance represents a combination of the Unit Of Work and Repository 
    ///  patterns such that it can be used to query from a database and group together
    ///  changes that will then be written back to the store as a unit.  DbContext is conceptually similar to ObjectContext.
    /// </summary>
    public abstract class EFContext : DbContext
    {
        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public EFContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    }
}
