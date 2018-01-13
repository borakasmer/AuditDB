using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=*****Product.database.********;Initial Catalog=ProductDB;Persist Security Info=False;User ID=******;Password=******;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

    public override int SaveChanges()
    {
        try
        {
            // Değişen Var mı? Boya Var mı :)            
            var modifiedEntities = ChangeTracker.Entries()
               .Where(p => p.State == EntityState.Modified).ToList();
            var now = System.DateTime.UtcNow;
            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                
                var PrimaryKey=change.OriginalValues.Properties.FirstOrDefault(prop=>prop.IsPrimaryKey()==true).Name;

                foreach (IProperty prop in change.OriginalValues.Properties)
                {                   

                    var originalValue = change.OriginalValues[prop.Name].ToString();
                    var currentValue = change.CurrentValues[prop.Name].ToString();

                    if (originalValue != currentValue) //Sadece Değişen kayıt Log'a atılır.
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = int.Parse(change.OriginalValues[PrimaryKey].ToString()),                       
                            PropertyName = prop.Name,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now,
                            State = EnumState.Update
                        };
                        //Değişen kayıt Log'u ElasticSearch'e atılır.
                        ElasticSearch.CheckExistsAndInsert(log);
                    }
                }

            }
            return base.SaveChanges();
        }
        catch (Exception ex)
        {
            var error = ex.Message;
            return 0;
        }
    }
}