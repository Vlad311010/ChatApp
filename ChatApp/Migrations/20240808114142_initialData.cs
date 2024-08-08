using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class initialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql =
            $"""
            DBCC CHECKIDENT ('Users', RESEED, 1);
            DBCC CHECKIDENT ('ChatGroups', RESEED, 1);

            INSERT INTO [dbo].[Users] 
                ([Login], [Password])
            VALUES 
                ('Ruf',      '111'),
                ('Azter',    '111');
    

            INSERT INTO [dbo].[ChatGroups] 
                ([Name], [OwnerId], [IsPublic], [Description])
            VALUES 
                ('Chat01', 1, 1, 'Chat 01 Description'),
                ('Chat02', 2, 1, 'Chat 02 Description'),
                ('Chat03', 1, 1, 'Chat 03 Description');


            INSERT INTO [dbo].[ChatGroupMembers] 
                ([UserId], [ChatGroupId])
            VALUES 
                (1, 1),
                (1, 2),
                (1, 3),

                (2, 1),
                (2, 2);
            """;


            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
