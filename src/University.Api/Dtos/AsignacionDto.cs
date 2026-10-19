using System.ComponentModel.DataAnnotations;
namespace ProyectoUniversityJonathan.Dtos;
public class AsignacionDto
{
    [Range(1, int.MaxValue)] public int EstudianteId { get; set; }
    [Range(1, int.MaxValue)] public int CursoId { get; set; }
    [Range(1, int.MaxValue)] public int DocenteId { get; set; }
}
