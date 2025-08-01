using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conteudo.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class InitialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Cor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IconeUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsAtiva = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValidoAte = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConteudoProgramatico_Resumo = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_Objetivos = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_PreRequisitos = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_PublicoAlvo = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_Metodologia = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_Recursos = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_Avaliacao = table.Column<string>(type: "TEXT", nullable: false),
                    ConteudoProgramatico_Bibliografia = table.Column<string>(type: "TEXT", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DuracaoHoras = table.Column<int>(type: "INTEGER", nullable: false),
                    Nivel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Instrutor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    VagasMaximas = table.Column<int>(type: "INTEGER", nullable: false),
                    VagasOcupadas = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cursos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Aulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<int>(type: "INTEGER", nullable: false),
                    DuracaoMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    VideoUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    TipoAula = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsObrigatoria = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublicada = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aulas_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materiais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AulaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    TipoMaterial = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsObrigatorio = table.Column<bool>(type: "INTEGER", nullable: false),
                    TamanhoBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    Extensao = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAtivo = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materiais_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_CursoId_Numero",
                table: "Aulas",
                columns: new[] { "CursoId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_IsPublicada",
                table: "Aulas",
                column: "IsPublicada");

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_TipoAula",
                table: "Aulas",
                column: "TipoAula");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_IsAtiva",
                table: "Categorias",
                column: "IsAtiva");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Ordem",
                table: "Categorias",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_Ativo",
                table: "Cursos",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_CategoriaId",
                table: "Cursos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_Nome",
                table: "Cursos",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_ValidoAte",
                table: "Cursos",
                column: "ValidoAte");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_AulaId_Nome",
                table: "Materiais",
                columns: new[] { "AulaId", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_IsAtivo",
                table: "Materiais",
                column: "IsAtivo");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_Ordem",
                table: "Materiais",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_TipoMaterial",
                table: "Materiais",
                column: "TipoMaterial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materiais");

            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
