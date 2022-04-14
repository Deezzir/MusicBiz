namespace W2022A6KI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MediaItem_Filename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArtistMediaItems", "FileName", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArtistMediaItems", "FileName");
        }
    }
}
