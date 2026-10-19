using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProyectoUniversityJonathan.Data;
using ProyectoUniversityJonathan.Dtos;
using ProyectoUniversityJonathan.Models;

namespace ProyectoUniversityJonathan.Controllers;

[ApiController]
[Route("api/asignaciones")]
public class AsignacionesController(UniversityDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asignacion>>> GetAll() =>
        await db.Asignaciones.AsNoTracking().OrderBy(x => x.AsignacionId).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Asignacion>> Get(int id)
    {
        var entity = await db.Asignaciones.FindAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    private async Task<bool> ReferencesExist(AsignacionDto dto) =>
        await db.Estudiantes.AnyAsync(x => x.EstudianteId == dto.EstudianteId) &&
        await db.Cursos.AnyAsync(x => x.CursoId == dto.CursoId) &&
        await db.Docentes.AnyAsync(x => x.DocenteId == dto.DocenteId);

    [HttpPost]
    public async Task<ActionResult<Asignacion>> Create(AsignacionDto dto)
    {
        if (!await ReferencesExist(dto))
            return BadRequest(new { mensaje = "Debe existir el estudiante, curso y docente." });
        var entity = new Asignacion { EstudianteId = dto.EstudianteId,
            CursoId = dto.CursoId, DocenteId = dto.DocenteId };
        db.Asignaciones.Add(entity);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
        { return BadRequest(new { mensaje = "Una referencia ya no existe." }); }
        return CreatedAtAction(nameof(Get), new { id = entity.AsignacionId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AsignacionDto dto)
    {
        var entity = await db.Asignaciones.FindAsync(id);
        if (entity is null) return NotFound();
        if (!await ReferencesExist(dto))
            return BadRequest(new { mensaje = "Debe existir el estudiante, curso y docente." });
        entity.EstudianteId = dto.EstudianteId;
        entity.CursoId = dto.CursoId;
        entity.DocenteId = dto.DocenteId;
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
        { return BadRequest(new { mensaje = "Una referencia ya no existe." }); }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.Asignaciones.FindAsync(id);
        if (entity is null) return NotFound();
        db.Asignaciones.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
