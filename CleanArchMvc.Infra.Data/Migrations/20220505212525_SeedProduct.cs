using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchMvc.Infrastructure.Migrations
{
    public partial class SeedProduct : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into \"Products\" (\"Name\", \"Description\", \"Price\", \"Stock\", \"Image\", \"CategoryId\")" +
                " values ('Caderno espiral', 'Caderno espiral de 100 folhas', 7.45, 50, 'cadernoesp.jpg', 1)");

            mb.Sql("insert into \"Products\" (\"Name\", \"Description\", \"Price\", \"Stock\", \"Image\", \"CategoryId\")" +
                " values ('Estojo escolar', 'Estojo escolar preto', 5.47, 78, 'estojo.jpg', 1)");

            mb.Sql("insert into \"Products\" (\"Name\", \"Description\", \"Price\", \"Stock\", \"Image\", \"CategoryId\")" +
                " values ('Borracha escolar', 'Borracha escolar TK', 3.65, 100, 'borracha.jpg', 1)");

            mb.Sql("insert into \"Products\" (\"Name\", \"Description\", \"Price\", \"Stock\", \"Image\", \"CategoryId\")" +
                " values ('Calculadora escolar', 'Calculadora simples', 20.99, 30, 'calculadora.jpg', 2)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("delete from \"Products\"");
        }
    }
}