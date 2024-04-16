using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projeto_Desenvolvedor_ElawIO.Models;
using Projeto_Desenvolvedor_ElawIO.Models.Cadastro;

namespace Projeto_Desenvolvedor_ElawIO.Controllers
{
    public class DadosController : Controller
    {
        private readonly Banco_Context _context;

        public DadosController(Banco_Context context)
        {
            _context = context;
        }

        #region Ações

        public async Task<IActionResult> Index(string filtroOrdem, string busca, DateTime? data, int tipoFiltro)
        {
            var dadosLista = from dados in _context.Dados select dados;


            switch (tipoFiltro)
            {
                case 1:
                    if (!busca.IsNullOrEmpty())
                        dadosLista = dadosLista.Where(x => x.Nome.Contains(busca));
                    break;
                case 2:

                    if (!busca.IsNullOrEmpty())
                        dadosLista = dadosLista.Where(x => x.Email.Contains(busca));
                    break;
                case 3:
                    if (data != null)
                    {
                        var dataFiltar = data.Value.Date;
                        dadosLista = dadosLista.Where(x => x.Data_Cadastro.Date == dataFiltar);
                    }
                    break;
                default:
                    break;
            }


            ViewBag.OrdemNome = filtroOrdem == "nome_desc" ? "nome_ordem" : "nome_desc";
            ViewBag.OrdemEmail = filtroOrdem == "email_desc" ? "email_ordem" : "email_desc";
            ViewBag.OrdemData = filtroOrdem == "Data" ? "data_desc" : "Data";

            switch (filtroOrdem)
            {
                case "nome_desc":
                    dadosLista = dadosLista.OrderBy(s => s.Nome);
                    break;
                case "email_desc":
                    dadosLista = dadosLista.OrderBy(s => s.Email);
                    break;       
                case "nome_ordem":
                    dadosLista = dadosLista.OrderByDescending(s => s.Nome);
                    break;
                case "email_ordem":
                    dadosLista = dadosLista.OrderByDescending(s => s.Email);
                    break;
                case "Data":
                    dadosLista = dadosLista.OrderByDescending(s => s.Data_Cadastro);
                    break;
                case "Data_Desc":
                    dadosLista = dadosLista.OrderBy(s => s.Data_Cadastro) ;
                    break;
                default:
                    break;
            }

            return dadosLista != null ? 
                          View(await dadosLista.ToListAsync()) :
                          Problem("Houve um erro ao tentar buscar os dados.");
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null || _context.Dados == null)
            {
                return NotFound();
            }

            var dados = await _context.Dados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dados == null)
            {
                return NotFound();
            }

            return View(dados);
        }

        public IActionResult Criar()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([Bind("Id,Nome,Email")] Dados dados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dados.setTimeCadastro();
                    List<string> msg = dados.check();
                    if (msg.Count != 0)
                    {
                        foreach (var mensagem in msg)
                        {
                            ModelState.AddModelError("", mensagem);
                        }
                        return View(dados);
                    }

                    _context.Add(dados);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar o registro. Por favor, tente novamente.");
                    return View(dados);
                }
            }
            return View(dados);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null || _context.Dados == null)
            {
                return NotFound();
            }

            var dados = await _context.Dados.FindAsync(id);
            if (dados == null)
            {
                return NotFound();
            }
            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nome,Email")] Dados dados)
        {
            if (id != dados.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dados.setTimeCadastro();
                    List<string> msg = dados.check();
                    if (msg.Count != 0)
                    {
                        foreach (var mensagem in msg)
                        {
                            ModelState.AddModelError("", mensagem);
                        }
                        return View(dados);
                    }

                    _context.Update(dados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValidarDados(dados.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dados);
        }

        public async Task<IActionResult> Deletar(int? id)
        {
            try
            {
                if (id == null || _context.Dados == null)
                {
                    return NotFound();
                }

                var dados = await _context.Dados
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (dados == null)
                {
                    return NotFound();
                }

                return View(dados);
            }
            catch
            {
                throw;
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfrimarDeletar(int id)
        {
            try
            {
                if (_context.Dados == null)
                {
                    return Problem("Houve um erro ao tentar buscar o dado para deletar.");
                }
                var dados = await _context.Dados.FindAsync(id);
                if (dados != null)
                {
                    _context.Dados.Remove(dados);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        #endregion


        private bool ValidarDados(int id)
        {
          return (_context.Dados?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
