using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAPIApplication;
using Xunit;
using System.Net.Http;
using System.Text;
using System.Net;

namespace UnitTest
{
    public class PessoasControllerIntegrationTest : BaseIntegrationTest
    {
        private const string BaseUrl = "/api/pessoas";
        public PessoasControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task DeveRetornarListaDePessoasVazia()
        {
            var response = await Client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Pessoa>>(responseString);

            Assert.Equal(data.Count, 0);
        }

        [Fact]
        public async Task DeveRetornarListaDePessoas()
        {
            var pessoa = new Pessoa
            {
                Nome = "Rafael dos Santos",
                Twitter = "rsantosdev"
            };

            await TestDataContext.AddAsync(pessoa);
            await TestDataContext.SaveChangesAsync();

            var response = await Client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Pessoa>>(responseString);

            Assert.Equal(data.Count, 1);
            Assert.Contains(data, x => x.Nome == pessoa.Nome);
        }

        [Fact]
        public async Task DeveAdicionarPessoa()
        {
            var pessoa = new Pessoa
            {
                Nome = "Rafael dos Santos",
                Twitter = "rsantosdev"
            };

            var response = await Client.PostAsync(BaseUrl, new StringContent(JsonConvert.SerializeObject(pessoa), Encoding.UTF8, "application/json"));            
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Pessoa>(responseString);

            Assert.Equal(data.Nome, pessoa.Nome);
        }

        [Fact]
        public async Task DeveAtualizarPessoa()
        {
            var pessoa = new Pessoa
            {
                Nome = "Washington Borges",
                Twitter = "borgeston"
            };

            await TestDataContext.AddAsync(pessoa);
            await TestDataContext.SaveChangesAsync();
           
           var pessoaEditada = new Pessoa
           { 
                Id = pessoa.Id,
                Nome = "Ton Borges",
                Twitter = "borgeston"
            };

            var response = await Client.PutAsync($"{BaseUrl}/{pessoa.Id}", new StringContent(JsonConvert.SerializeObject(pessoaEditada), Encoding.UTF8, "application/json"));            
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Pessoa>(responseString);

            Assert.Equal(data.Nome, pessoaEditada.Nome);
            Assert.Equal(data.Twitter, pessoaEditada.Twitter);
        }

        [Fact]
        public async Task DeveDeletarPessoa()
        {
            var pessoa = new Pessoa
            {
                Nome = "Rafael dos Santos",
                Twitter = "rsantosdev"
            };

            await TestDataContext.AddAsync(pessoa);
            await TestDataContext.SaveChangesAsync();

            var response = await Client.DeleteAsync($"{BaseUrl}/{pessoa.Id}");            
            response.EnsureSuccessStatusCode();

            response = await Client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Pessoa>>(responseString);

            Assert.Equal(data.Count, 0);
        }

        
        [Fact]
        public async Task DeveObterPessoa()
        {
            var pessoa = new Pessoa
            {
                Nome = "Washington Borges",
                Twitter = "borgeston"
            };

            await TestDataContext.AddAsync(pessoa);
            await TestDataContext.SaveChangesAsync();

            var response = await Client.GetAsync($"{BaseUrl}/{pessoa.Id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Pessoa>(responseString);

            Assert.Equal(data.Id, pessoa.Id);
            Assert.Equal(data.Nome, pessoa.Nome);
            Assert.Equal(data.Twitter, pessoa.Twitter);
        }

        [Fact]
        public async Task DeveRetornaNaoEncontraoParaAtualizarPessoaComIdInvalido()
        {
            var pessoa = new Pessoa
            {
                Nome = "Washington Borges",
                Twitter = "borgeston"
            };

            await TestDataContext.AddAsync(pessoa);
            await TestDataContext.SaveChangesAsync();
           
           var pessoaEditada = new Pessoa
           { 
                Id = pessoa.Id,
                Nome = "Ton Borges",
                Twitter = "borgeston"
            };

            var response = await Client.PutAsync($"{BaseUrl}/{pessoa.Id+1000}", new StringContent(JsonConvert.SerializeObject(pessoaEditada), Encoding.UTF8, "application/json"));

            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}