using Microsoft.AspNetCore.Mvc;

namespace ClienteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        // GET: api/clientes
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { mensaje = "Lista de clientes" });
        }
        
        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new { mensaje = $"Cliente con ID {id}" });
        }

        // POST: api/clientes
        [HttpPost]
        public IActionResult Post()
        {
            return Ok(new { mensaje = "Cliente registrado" });
        }
    }
}
