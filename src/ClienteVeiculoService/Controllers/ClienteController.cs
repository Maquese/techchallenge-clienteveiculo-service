using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Clientes;
using Application.Models.Requests;

namespace AutoReparaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [Authorize]
    public class ClienteController : ControllerBase
    {
        public ClienteController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromServices]AdicionarClienteHandler adicionarClienteHandler, [FromBody]AddClienteRequest cliente)
        {
            var id = await adicionarClienteHandler.Handle(cliente);
            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarCliente([FromServices]AtualizarClienteHandler clienteAppService, [FromBody]UpdateClienteRequest cliente)
        {            
            return Ok(await clienteAppService.Handle(cliente));
        }
        
        [HttpGet]
        public async Task<IActionResult> BuscarCliente([FromServices]VerificaCadastroClienteHandler verificaCadastroClienteHandler, [FromQuery]string cpf)
        {            
            var cliente = await verificaCadastroClienteHandler.Handle(cpf);
            return Ok(cliente);
        }

        [HttpDelete]
        public async Task<IActionResult> InativarCliente([FromServices]InativarClienteHandler inativarClienteHandler, [FromQuery]int id)
        {
            
            return Ok(await inativarClienteHandler.Handle(id));
        }      

        [HttpPost]
        public async Task<IActionResult> AdicionarVeiculo([FromServices]AdicionarVeiculoClienteHandler adicionarVeiculoClienteHandler, [FromBody]AddVeiculoRequest veiculo)
        {            
            return Ok(await adicionarVeiculoClienteHandler.Handle(veiculo));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarVeiculo([FromServices]BuscarVeiculoPlacaClienteHandler buscarVeiculoPlacaClienteHandler, [FromQuery]string placa)
        {
            var veiculo = await buscarVeiculoPlacaClienteHandler.Handle(placa);
            return Ok(veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarVeiculo([FromServices]AtualizarVeiculoClienteHandler atualizarVeiculoClienteHandler, [FromBody]UpdateVeiculoRequest veiculo)
        {           
            return Ok( await atualizarVeiculoClienteHandler.Handle(veiculo));
        }

        [HttpDelete]
        public async Task<IActionResult> InativarVeiculo([FromServices]InativarVeiculoClienteHandler inativarVeiculoClienteHandler, [FromQuery]int id)
        {
            return Ok(await inativarVeiculoClienteHandler.Handle(id));
        }
    }
}
