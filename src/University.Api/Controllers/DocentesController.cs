using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProyectoUniversityJonathan.Data;
using ProyectoUniversityJonathan.Models;
using ProyectoUniversityJonathan.Dtos;

namespace ProyectoUniversityJonathan.Controllers;

[ApiController]
[Route("api/docentes")]
public class DocentesController(UniversityDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Docente>>> GetAll() =>
        await db.Docentes.AsNoTracking().OrderBy(x => x.DocenteId).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Docente>> Get(int id)
    {
        var entity = await db.Docentes.FindAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<Docente>> Create(DocenteDto dto)
    {
        var entity = new Docente();
        entity.Nombres = dto.Nombres.Trim();
        entity.Apellidos = dto.Apellidos.Trim();
        db.Docentes.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.DocenteId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DocenteDto dto)
    {
        var entity = await db.Docentes.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Nombres = dto.Nombres.Trim();
        entity.Apellidos = dto.Apellidos.Trim();
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.Docentes.FindAsync(id);
        if (entity is null) return NotFound();
        if (await db.Asignaciones.AnyAsync(x => x.DocenteId == id))
            return Conflict(new { mensaje = "El registro tiene asignaciones asociadas." });
        db.Docentes.Remove(entity);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
        { return Conflict(new { mensaje = "El registro tiene referencias asociadas." }); }
        return NoContent();
    }
}
