using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Configuration
{
    public class ApplicationStoreConfiguration : EntityTypeConfiguration<ApplicationStore>
    {
        public ApplicationStoreConfiguration()
        {
            ToTable("THApplicationStore");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_ApplicationStore");
            
            Property(x => x.ConfClient).HasColumnName("ConfClient_ApplicationStore");
            Property(x => x.ConfSecret).HasColumnName("ConfSecret_ApplicationStore").HasMaxLength(32);
            Property(x => x.JSClient).HasColumnName("JSClient_ApplicationStore");
            Property(x => x.JSAllowedOrigins).HasColumnName("JSAllowedOrigins_ApplicationStore");
            Property(x => x.Status).HasColumnName("Status_ApplicationStore").IsRequired();

            HasRequired<Application>(x => x.Application).WithMany(a => a.ApplicationsStore).HasForeignKey(x => x.ApplicationCode).WillCascadeOnDelete(false);
            Property(x => x.ApplicationCode).HasColumnName("Code_Application");

            HasRequired<Store>(x => x.Store).WithMany(s => s.ApplicationsStore).HasForeignKey(x => x.StoreCode).WillCascadeOnDelete(false);
            Property(x => x.StoreCode).HasColumnName("Code_Store");

            Ignore(x => x.ClientAuthType);

            #region "  IX_ApplicationStore_ConfClient  "

            Property(t => t.ConfClient) 
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ApplicationStore_ConfClient", 1) { IsUnique = true }));

            Property(t => t.ConfSecret)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ApplicationStore_ConfClient", 2)));

            Property(t => t.Status)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ApplicationStore_ConfClient", 3)));

            #endregion

            #region  "  IX_ApplicatioStore_JSClient  "

            Property(t => t.JSClient)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ApplicatioStore_JSClient", 1) { IsUnique = true }));

            Property(t => t.Status)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_ApplicatioStore_JSClient", 2)));

            #endregion

        }
    }
}
