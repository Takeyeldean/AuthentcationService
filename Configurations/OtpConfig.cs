using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AuthentcationServiceForTradingMarket.Model;

namespace AuthentcationServiceForTradingMarket.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<OtpAndEmail>
    {
        public void Configure(EntityTypeBuilder<OtpAndEmail> builder)
        {
            // Table name
            builder.ToTable("OtpsAndEmails");

            // Primary key
            builder.HasKey(x => x.ID);
            builder.Property(e => e.Email).IsRequired().IsUnicode();
            builder.Property(e => e.OTP).IsRequired();



        }
    }
}
