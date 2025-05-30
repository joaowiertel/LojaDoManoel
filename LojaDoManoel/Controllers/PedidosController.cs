using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class CaixaController : ControllerBase
{
    private EmpacotadorService _empacotador;

    public CaixaController()
    {
        _empacotador = new EmpacotadorService();
    }

    [HttpPost]
    public IActionResult Empacotar([FromBody] RetornoPedidos dados)
    {
        List<CaixaResultado> empacotamento = _empacotador.EmpacotarPedidos(dados.Pedidos);
        return Ok(empacotamento);
    }
}