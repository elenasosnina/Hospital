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
    
    public partial class Стоимость_услуг
    {
        public int ID_стоимости_услуги { get; set; }
        public Nullable<int> ID_услуги { get; set; }
        public decimal Стоимость { get; set; }
        public decimal Скидка { get; set; }
    
        public virtual Услуги Услуги { get; set; }
    }
}
