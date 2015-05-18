namespace Lab2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sharding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Counter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Shards");
        }
    }
}
