﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hospital
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C_Procedures> C_Procedures { get; set; }
        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Diseases> Diseases { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<DoctorSchedule> DoctorSchedule { get; set; }
        public virtual DbSet<MedicalRecords> MedicalRecords { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }
        public virtual DbSet<Record_an_Appointment> Record_an_Appointment { get; set; }
        public virtual DbSet<Recording_a_procedure> Recording_a_procedure { get; set; }
        public virtual DbSet<ServicesCost> ServicesCost { get; set; }
    }
}