using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
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
    public class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        public ApplicationConfiguration()
        {
            ToTable("THApplication");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Application");
            Property(x => x.Name).HasColumnName("Name_Application").IsRequired().HasMaxLength(50);
            Property(x => x.AuthType).HasColumnName("AuthType_Application").IsRequired();
            Property(x => x.MemberType).HasColumnName("MemberType_Application").IsRequired();
            
            Property(x => x.Status).HasColumnName("Status_Application").IsRequired();
            Property(x => x.ObjectSerialized).HasColumnName("Object_Application").IsRequired();

            Property(x => x.SaveDate).HasColumnName("SaveDate_Application").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Application").IsRequired();


            Property(t => t.Name)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Application_Name", 1)));


            Property(t => t.Status)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Application_Status", 1)));
        }
    }
}
