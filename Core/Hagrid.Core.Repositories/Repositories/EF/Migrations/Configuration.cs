namespace Hagrid.Core.Infrastructure.Migrations
{
    using Hagrid.Core.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Hagrid.Infra.Utils;
    using Hagrid.Core.Domain.Enums;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using Hagrid.Core.Infrastructure.Repositories.EF.Migrations;
    using System.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<Hagrid.Core.Providers.EntityFramework.Context.CustomerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            AutomaticMigrationDataLossAllowed = false;
            MigrationsDirectory = @"Repositories\EF\Migrations\Updates";
        }

        protected override void Seed(Hagrid.Core.Providers.EntityFramework.Context.CustomerContext context)
        {
            if (ConfigurationManager.AppSettings["RunSeed"].AsBool(false))
            {
                var localhostSeed = new LocalhostSeed(context);
                localhostSeed.AddApplication();
                localhostSeed.AddStore();
                localhostSeed.AddApplicationStore();
                localhostSeed.AddRole();
                localhostSeed.AddUser();
                localhostSeed.AddStoreAccount();

                context.SaveChanges();
            }
        }
    }
}
