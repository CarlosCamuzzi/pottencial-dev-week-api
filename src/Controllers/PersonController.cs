using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

using src.Models;
using src.Persistence;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]

public class PersonController : ControllerBase
{

  private DatabaseContext _context { get; set; }

  public PersonController(DatabaseContext context)
  {
    this._context = context;
  }

  [HttpGet]
  public ActionResult<List<Pessoa>> Get()
  {
    var result = _context.Pessoas.Include(p => p.contratos).ToList();

    if (!result.Any()) return NoContent();

    return result;
  }

  [HttpPost]
  public ActionResult<Pessoa> Post(
    [FromBody] Pessoa pessoa)
  {
    try
    {
      _context.Pessoas.Add(pessoa);
      _context.SaveChanges();
    }
    catch (System.Exception)
    {
      return BadRequest(new
      {
        msg = "Houve erro ao enviar ao enviar solicitacao",
        status = HttpStatusCode.BadRequest
      });
    }

    return Created("criado", pessoa);
  }

  [HttpPut("{id}")]
  public ActionResult<Object> Update(
    [FromRoute] int id,
    [FromBody] Pessoa pessoa)
  {

    var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

    if (result is null)
    {
      return NotFound(new
      {
        msg = "Registro não encontrado",
        status = HttpStatusCode.NotFound
      });
    }

    try
    {
      _context.Pessoas.Update(pessoa);
      _context.SaveChanges();
    }
    catch (System.Exception)
    {
      return BadRequest(new
      {
        msg = $"Houve erro ao enviar ao enviar solicitacao do ID {id}",
        status = HttpStatusCode.BadRequest
      });
    }

    return Ok(new
    {
      msg = "Dados do ID " + id + "atualizado",
      status = HttpStatusCode.OK
    });
  }

  [HttpDelete("{id}")]
  public ActionResult<Object> Delete(
    [FromRoute] int id)
  {
    var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

    if (result is null)
    {
      return BadRequest(new
      {
        msg = "Conteúdo inexistente",
        status = HttpStatusCode.BadRequest
      });
    }

    _context.Pessoas.Remove(result);
    _context.SaveChanges();

    return Ok(new
    {
      msg = "Deletado id " + id,
      status = HttpStatusCode.OK
    });
  }
}
