using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProyectoUniversityJonathan.Data;
using ProyectoUniversityJonathan.Models;
using ProyectoUniversityJonathan.Dtos;

namespace ProyectoUniversityJonathan.Controllers;

[ApiController]
[Route("api/cursos")]
public class CursosController(UniversityDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Curso>>> GetAll() =>
        await db.Cursos.AsNoTracking().OrderBy(x => x.CursoId).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Curso>> Get(int id)
    {
        var entity = await db.Cursos.FindAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<Curso>> Create(CursoDto dto)
    {
        var entity = new Curso();
        entity.Nombre = dto.Nombre.Trim();
        db.Cursos.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.CursoId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CursoDto dto)
    {
        var entity = await db.Cursos.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Nombre = dto.Nombre.Trim();
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.Cursos.FindAsync(id);
        if (entity is null) return NotFound();
        if (await db.Asignaciones.AnyAsync(x => x.CursoId == id))
            return Conflict(new { mensaje = "El registro tiene asignaciones asociadas." });
        db.Cursos.Remove(entity);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
        { return Conflict(new { mensaje = "El registro tiene referencias asociadas." }); }
        return NoContent();
    }
}
