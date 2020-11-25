using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class UsuarioRepositoryEF : IUsuarioEFRepository
    {
        private readonly SistemaContext _db;
        private readonly ILogger<UsuarioRepositoryEF> _logger;

        public UsuarioRepositoryEF(SistemaContext db, ILogger<UsuarioRepositoryEF> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Usuario Adicionar(Usuario usuario)
        {
            _db.Usuario.Add(usuario);

            var tempo = Stopwatch.StartNew();
            _db.SaveChanges();
            tempo.Stop();

            _logger.LogInformation($"Adicionar: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuario;
        }

        public Usuario ObterPorId(int id)
        {
            var tempo = Stopwatch.StartNew();
            var usuario = _db.Usuario.FirstOrDefault(o => o.Id == id);
            tempo.Stop();

            _logger.LogInformation($"ObterPorId: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuario;
        }

        public List<Usuario> ObterTodos()
        {
            var tempo = Stopwatch.StartNew();
            var usuarios = _db.Usuario.ToList();
            tempo.Stop();

            _logger.LogInformation($"ObterTodos: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuarios;
        }

        public void Remover(int id)
        {
            var tempo = Stopwatch.StartNew();
            Usuario usuario = _db.Usuario.FirstOrDefault(o => o.Id == id);
            _db.Usuario.Remove(usuario);
            _db.SaveChanges();
            tempo.Stop();

            _logger.LogInformation($"Remover: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return;
        }

        public Usuario Atualizar(Usuario usuario)
        {
            var tempo = Stopwatch.StartNew();
            _db.Usuario.Update(usuario);
            _db.SaveChanges();
            tempo.Stop();

            _logger.LogInformation($"Atualizar: {tempo.Elapsed.TotalMilliseconds.ToString()}");

            return usuario;
        }
    }
}