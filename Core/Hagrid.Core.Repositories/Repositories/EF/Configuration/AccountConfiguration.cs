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
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            ToTable("THAccount");

            HasKey(x => x.Code);
            Property(x => x.Code).HasColumnName("Code_Account");
            Property(x => x.Login).HasColumnName("Login_Account").IsRequired().HasMaxLength(255);
            Property(x => x.Password).HasColumnName("Password_Account").IsRequired().HasMaxLength(500);
            Property(x => x.Email).HasColumnName("Email_Account").IsRequired().HasMaxLength(255);
            Property(x => x.Document).HasColumnName("Document_Account").HasMaxLength(20);
            Property(x => x.QtyWrongsPassword).HasColumnName("QtyWrongsPassword_Account");
            Property(x => x.LockedUp).HasColumnName("LockedUp_Account");
            Property(x => x.Status).HasColumnName("Status_Account").IsRequired();
            Property(x => x.Removed).HasColumnName("Removed_Account").IsRequired();
            Property(x => x.SaveDate).HasColumnName("SaveDate_Account").IsRequired();
            Property(x => x.UpdateDate).HasColumnName("UpdateDate_Account").IsRequired();
            Property(x => x.IsResetPasswordNeeded).HasColumnName("IsResetPasswordNeeded_Account");
            Property(x => x.OtherPasswordType).HasColumnName("OtherPasswordType_Account");
            Property(x => x.FacebookId).HasColumnName("FacebookId_Account").HasMaxLength(25);
            Property(x => x.DataFingerprint).HasColumnName("DataFingerprint_Account");

            HasMany(s => s.BlacklistCollection)
               .WithRequired(s => s.Account)
               .HasForeignKey(s => s.AccountCode);

            HasMany(s => s.Metadata)
              .WithRequired(s => s.Account)
              .HasForeignKey(s => s.AccountCode);

            #region " IX_Account_Login "

            Property(t => t.Login)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Login", 1)));

            Property(t => t.Password)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Login", 2)));

            Property(t => t.Removed)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Login", 3)));

            Property(t => t.Status)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Login", 4)));

            #endregion

            #region " IX_Account_Email "

            Property(t => t.Email)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Email", 1)));

            Property(t => t.Password)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Email", 2)));

            Property(t => t.Removed)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Email", 3)));

            Property(t => t.Status)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_Email", 4)));

            #endregion

            #region " IX_Account_IsResetPasswordNeeded "

            Property(t => t.IsResetPasswordNeeded)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Account_IsResetPasswordNeeded")));

            #endregion
        }
    }
}
