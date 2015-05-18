namespace Lab2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Media", "SamplingRate", c => c.String());
            AlterColumn("dbo.Media", "SamplingRate1", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Media", "SamplingRate1", c => c.Double());
            AlterColumn("dbo.Media", "SamplingRate", c => c.Double());
        }
    }
}
