namespace BaseData.DataAccess.Migrations
{
    using Model;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BaseData.DataAccess.MyDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BaseData.DataAccess.MyDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.ProductTypes.AddOrUpdate(
            //  p => p.ProductTypeID,
            //  new ProductType { ProductTypeName = "饮料" },
            //  new ProductType { ProductTypeName = "食品" },
            //  new ProductType { ProductTypeName = "日常百货" }
            //);
        }
    }
}
