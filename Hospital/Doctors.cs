//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Doctors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctors()
        {
            this.DoctorSchedule = new HashSet<DoctorSchedule>();
            this.Record_an_Appointment = new HashSet<Record_an_Appointment>();
        }
    
        public int DoctorID { get; set; }
        public string Surname { get; set; }
        public string DoctorName { get; set; }
        public string Patronymic { get; set; }
        public string Specialty { get; set; }
        public int Сabinet { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoctorSchedule> DoctorSchedule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Record_an_Appointment> Record_an_Appointment { get; set; }
    }
}
