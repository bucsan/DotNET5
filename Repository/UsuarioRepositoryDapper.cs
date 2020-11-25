using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class UsuarioRepositoryDapper : IUsuarioDapperRepository
    {
        private IDbConnection db;
        private readonly ILogger<UsuarioRepositoryDapper> _logger;

        public UsuarioRepositoryDapper(IConfiguration configuration, ILogger<UsuarioRepositoryDapper> logger)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("Local"));
            _logger = logger;
        }

        public Usuario Adicionar(Usuario usuario)
        {
            var sql = 
                @"INSERT INTO [dbo].[Usuario]([Nome], [Email], [Endereco], [Cidade], [Estado], [CEP])
                VALUES (@Nome, @Email, @Endereco, @Cidade, @Estado, @CEP)
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var tempo = Stopwatch.StartNew();
            var id = db.Query<int>(sql, usuario).Single();
            tempo.Stop();

            _logger.LogInformation($"Adicionar: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            usuario.Id = id;
            return usuario;
        }

        public Usuario ObterPorId(int id)
        {
            var parametro = new DynamicParameters();
            parametro.Add("@Id", id, DbType.Int32);

            var sql =
                @"SELECT
	                [Id],
                    [Nome],
                    [Email],
                    [Endereco],
                    [Cidade],
                    [Estado],
                    [CEP]
                FROM [DotNET5].[dbo].[Usuario]
                WHERE [Id] = @Id";

            var tempo = Stopwatch.StartNew();
            var usuario = db.Query<Usuario>(sql, parametro).FirstOrDefault();
            tempo.Stop();

            _logger.LogInformation($"ObterPorId: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuario;
        }

        public List<Usuario> ObterTodos()
        {
            var sql =
                @"SELECT
	                [Id],
                    [Nome],
                    [Email],
                    [Endereco],
                    [Cidade],
                    [Estado],
                    [CEP]
                FROM [DotNET5].[dbo].[Usuario]";

            var tempo = Stopwatch.StartNew();
            var usuarios = db.Query<Usuario>(sql).ToList();
            tempo.Stop();

            _logger.LogInformation($"ObterTodos: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuarios;
        }

        public void Remover(int id)
        {
            var parametro = new DynamicParameters();
            parametro.Add("@Id", id, DbType.Int32);

            var sql =
                @"DELETE [DotNET5].[dbo].[Usuario]
                WHERE [Id] = @Id";

            var tempo = Stopwatch.StartNew();
            db.Execute(sql, parametro);
            tempo.Stop();

            _logger.LogInformation($"ObterTodos: {tempo.Elapsed.TotalMilliseconds.ToString()}");
        }

        public Usuario Atualizar(Usuario usuario)
        {
            var sql =
                @"UPDATE [DotNET5].[dbo].[Usuario]
                  SET
                    [Nome] = @Nome,
                    [Email] = @Email,
                    [Endereco] = @Endereco,
                    [Cidade] = @Cidade,
                    [Estado] = @Estado,
                    [CEP] = @CEP
                WHERE
                    [Id] = @Id";

            var tempo = Stopwatch.StartNew();
            db.Execute(sql, usuario);
            tempo.Stop();

            _logger.LogInformation($"Atualizar: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuario;
        }
    }
}