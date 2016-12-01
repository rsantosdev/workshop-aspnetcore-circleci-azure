using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebAPIApplication
{
    [Route("api/pessoas")]
    public class PessoasController : Controller
    {
        private readonly DataContext _dataContext;

        public PessoasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPessoas()
        {
            var pessoas = await _dataContext.Pessoas.ToListAsync();
            return Json(pessoas);
        }

        [HttpPost]
        public async Task<IActionResult> CriaPessoa([FromBody]Pessoa modelo)
        {
            await _dataContext.Pessoas.AddAsync(modelo);
            await _dataContext.SaveChangesAsync();

            return Json(modelo);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizaPessoa([FromBody]Pessoa modelo)        
        {
            var pessoa = await  _dataContext.Pessoas.SingleOrDefaultAsync(x=> x.Id == modelo.Id);    

            pessoa.Nome = modelo.Nome;
            pessoa.Twitter = modelo.Twitter;

            await _dataContext.SaveChangesAsync();

            return Json(pessoa);
        }

        [HttpDelete("{id}")]
        public async Task RemovePessoa(int id)        
        {
            var pessoa = await _dataContext.Pessoas.SingleOrDefaultAsync(x=> x.Id == id);
            _dataContext.Pessoas.Remove(pessoa);
            await _dataContext.SaveChangesAsync();
        }
    }
}