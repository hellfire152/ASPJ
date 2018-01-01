namespace ASPJ_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForumContextcomments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        commentID = c.Int(nullable: false, identity: true),
                        content = c.String(nullable: false),
                        username = c.String(nullable: false),
                        date = c.DateTime(nullable: false),
                        threadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.commentID)
                .ForeignKey("dbo.Threads", t => t.threadId, cascadeDelete: true)
                .Index(t => t.threadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "threadId", "dbo.Threads");
            DropIndex("dbo.Comments", new[] { "threadId" });
            DropTable("dbo.Comments");
        }
    }
}
