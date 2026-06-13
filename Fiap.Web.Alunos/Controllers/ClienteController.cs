using Fiap.Web.Alunos.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Web.Alunos.Controllers
{
    public class ClienteController : Controller
    {

        private List<ClienteModel> _clientes;

        public ClienteController()
        {
            _clientes = GerarClientesMocados();
        }
        public IActionResult Index()
        {
            return View();
        }

        public static List<ClienteModel> GerarClientesMocados()
        {
            var clientes = new List<ClienteModel>();
            for (int i = 1; i <=5; i++) 
            {
                var cliente = new ClienteModel
                {
                    ClientId = i,
                    Nome = "Cliente" + i,
                    Sobrenome = "Sobrenome" + i,
                    Email = "cliente" + i + "@exemplo.com",
                    DataNascimento = DateTime.Now.AddYears(-30),
                    Observacao = "Observação do cliente " + i,
                    Representante = new RepresentanteModel
                    {
                        RepresentanteId = i,
                        NomeRepresentante = "Representante" + i,
                        Cpf = "0000000000" + i
                    }
                };

                clientes.Add(cliente);


            }
}
