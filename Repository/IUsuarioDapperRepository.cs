using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface IUsuarioDapperRepository
    {
        Usuario ObterPorId(int id);
        List<Usuario> ObterTodos();
        Usuario Adicionar(Usuario usuario);
        Usuario Atualizar(Usuario usuario);
        void Remover(int id);
    }
}