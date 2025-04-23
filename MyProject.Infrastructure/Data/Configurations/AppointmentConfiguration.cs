using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyProject.Core.Entities;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);

        // Hasta ilişkisi
        builder.HasOne(a => a.PatientProfile)
               .WithMany(p => p.Appointments)      // şimdi ICollection ile eşleşiyor
               .HasForeignKey(a => a.PatientProfileId)
               .OnDelete(DeleteBehavior.Restrict);

        // Doktor ilişkisi
        builder.HasOne(a => a.DoctorProfile)
               .WithMany(d => d.Appointments)      // şimdi ICollection ile eşleşiyor
               .HasForeignKey(a => a.DoctorProfileId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.Status)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(a => a.StartTime).IsRequired();
        builder.Property(a => a.EndTime).IsRequired();
    }
}
