using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProyectoUniversityJonathan.Data;
using ProyectoUniversityJonathan.Models;
using ProyectoUniversityJonathan.Dtos;

namespace ProyectoUniversityJonathan.Controllers;

[ApiController]
[Route("api/estudiantes")]
public class EstudiantesController(UniversityDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estudiante>>> GetAll() =>
        await db.Estudiantes.AsNoTracking().OrderBy(x => x.EstudianteId).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Estudiante>> Get(int id)
    {
        var entity = await db.Estudiantes.FindAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<Estudiante>> Create(EstudianteDto dto)
    {
        var entity = new Estudiante();
        entity.Nombres = dto.Nombres.Trim();
        entity.Apellidos = dto.Apellidos.Trim();
        entity.Direccion = dto.Direccion.Trim();
        db.Estudiantes.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.EstudianteId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EstudianteDto dto)
    {
        var entity = await db.Estudiantes.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Nombres = dto.Nombres.Trim();
        entity.Apellidos = dto.Apellidos.Trim();
        entity.Direccion = dto.Direccion.Trim();
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.Estudiantes.FindAsync(id);
        if (entity is null) return NotFound();
        if (await db.Asignaciones.AnyAsync(x => x.EstudianteId == id))
            return Conflict(new { mensaje = "El registro tiene asignaciones asociadas." });
        db.Estudiantes.Remove(entity);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
        { return Conflict(new { mensaje = "El registro tiene referencias asociadas." }); }
        return NoContent();
    }
}
