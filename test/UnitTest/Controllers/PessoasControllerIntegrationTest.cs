using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAPIApplication;
using Xunit;
using System.Net.Http;
using System.Text;

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
                Nome = "Rafael dos Santos",
                Twitter = "rsantosdev"
            };

            var response = await Client.PostAsync(BaseUrl, new StringContent(JsonConvert.SerializeObject(pessoa), Encoding.UTF8, "application/json"));            
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var retonoInclusao = JsonConvert.DeserializeObject<Pessoa>(responseString);
            
            retonoInclusao.Nome = "Ton Borges";
            retonoInclusao.Twitter = "borgeston";

            response = await Client.PutAsync(BaseUrl, new StringContent(JsonConvert.SerializeObject(retonoInclusao), Encoding.UTF8, "application/json"));            
            response.EnsureSuccessStatusCode();

            responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Pessoa>(responseString);

            Assert.Equal(data.Nome, retonoInclusao.Nome);
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
    }
}