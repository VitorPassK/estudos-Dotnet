using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecrutamentoDiversidade.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CANDIDATO_NET",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_CANDIDATO.NEXTVAL"),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(20)", nullable: true),
                    Linkedin = table.Column<string>(type: "NVARCHAR2(255)", nullable: true),
                    CurriculoUrl = table.Column<string>(type: "NVARCHAR2(500)", nullable: true),
                    Genero = table.Column<string>(type: "NVARCHAR2(30)", nullable: true),
                    RacaEtnia = table.Column<string>(type: "NVARCHAR2(30)", nullable: true),
                    PessoaComDeficiencia = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValue: false),
                    TipoDeficiencia = table.Column<string>(type: "NVARCHAR2(100)", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CANDIDATO_NET", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO_NET",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_USUARIO.NEXTVAL"),
                    Nome = table.Column<string>(type: "NVARCHAR2(150)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    Role = table.Column<string>(type: "NVARCHAR2(20)", nullable: false),
                    Ativo = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValue: true),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO_NET", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_VAGA_NET",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_VAGA.NEXTVAL"),
                    Titulo = table.Column<string>(type: "NVARCHAR2(200)", nullable: false),
                    Descricao = table.Column<string>(type: "NCLOB", nullable: true),
                    Departamento = table.Column<string>(type: "NVARCHAR2(100)", nullable: false),
                    LocalTrabalho = table.Column<string>(type: "NVARCHAR2(150)", nullable: true),
                    TipoContrato = table.Column<string>(type: "NVARCHAR2(30)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", nullable: false, defaultValue: "Aberta"),
                    MetaDiversidadePct = table.Column<decimal>(type: "NUMBER(5,2)", nullable: false, defaultValue: 30m),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EncerradoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    UsuarioId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_VAGA_NET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VAGA_NET_USUARIO",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO_NET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_CANDIDATURA_NET",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_CANDIDATURA.NEXTVAL"),
                    CandidatoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    VagaId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(30)", nullable: false, defaultValue: "Inscrito"),
                    PrioridadeDiversidade = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValue: false),
                    Observacoes = table.Column<string>(type: "NVARCHAR2(1000)", nullable: true),
                    InscritoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    AtualizadoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CANDIDATURA_NET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CANDIDATURA_NET_CANDIDATO",
                        column: x => x.CandidatoId,
                        principalTable: "TB_CANDIDATO_NET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CANDIDATURA_NET_VAGA",
                        column: x => x.VagaId,
                        principalTable: "TB_VAGA_NET",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UK_CANDIDATO_NET_EMAIL",
                table: "TB_CANDIDATO_NET",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_CANDIDATURA_NET_VagaId",
                table: "TB_CANDIDATURA_NET",
                column: "VagaId");

            migrationBuilder.CreateIndex(
                name: "UK_CANDIDATURA_NET_UNICA",
                table: "TB_CANDIDATURA_NET",
                columns: new[] { "CandidatoId", "VagaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_USUARIO_NET_EMAIL",
                table: "TB_USUARIO_NET",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_VAGA_NET_UsuarioId",
                table: "TB_VAGA_NET",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CANDIDATURA_NET");

            migrationBuilder.DropTable(
                name: "TB_CANDIDATO_NET");

            migrationBuilder.DropTable(
                name: "TB_VAGA_NET");

            migrationBuilder.DropTable(
                name: "TB_USUARIO_NET");
        }
    }
}
