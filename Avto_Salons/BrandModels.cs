//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Avto_Salons
{
    using System;
    using System.Collections.Generic;
    
    public partial class BrandModels
    {
        public int Id_brand_model { get; set; }
        public int brand_id { get; set; }
        public int model_id { get; set; }
    
        public virtual Brands Brands { get; set; }
        public virtual Models Models { get; set; }
    }
}
