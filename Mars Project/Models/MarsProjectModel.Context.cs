﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mars_Project.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MarsProjectDBEntities : DbContext
    {
        public MarsProjectDBEntities()
            : base("name=MarsProjectDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminInfo> AdminInfo { get; set; }
        public virtual DbSet<NavMenu> NavMenu { get; set; }
        public virtual DbSet<SceneryInfo> SceneryInfo { get; set; }
        public virtual DbSet<TrainInfo> TrainInfo { get; set; }
    }
}
