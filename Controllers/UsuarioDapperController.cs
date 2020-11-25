using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.Repository;

namespace WebApplication.Controllers
{
    public class UsuarioDapperController : Controller
    {
        private readonly IUsuarioDapperRepository _usuarioRepository;

        public UsuarioDapperController(IUsuarioDapperRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await Task.Run(() => _usuarioRepository.ObterTodos());
            return View(usuarios);
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await Task.Run(() => _usuarioRepository.ObterPorId(id.GetValueOrDefault()));
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("Id,Nome,Email,Endereco,Cidade,Estado,CEP")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => _usuarioRepository.Adicionar(usuario));
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Atualizar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await Task.Run(() => _usuarioRepository.ObterPorId(id.GetValueOrDefault()));
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(int id, [Bind("Id,Nome,Email,Endereco,Cidade,Estado,CEP")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await Task.Run(() => _usuarioRepository.Atualizar(usuario));
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await Task.Run(() => _usuarioRepository.Remover(id.GetValueOrDefault()));
            return RedirectToAction(nameof(Index));
        }
    }
}
