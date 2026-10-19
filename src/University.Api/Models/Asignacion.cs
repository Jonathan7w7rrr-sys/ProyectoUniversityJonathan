using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProyectoUniversityJonathan.Models;

public class Asignacion
{
    [Key]
    public int AsignacionId { get; set; }

    [Range(1, int.MaxValue)]
    public int EstudianteId { get; set; }

    [Range(1, int.MaxValue)]
    public int CursoId { get; set; }

    [Range(1, int.MaxValue)]
    public int DocenteId { get; set; }

    [ForeignKey(nameof(EstudianteId))]
    [JsonIgnore]
    public Estudiante Estudiante { get; set; } = null!;

    [ForeignKey(nameof(CursoId))]
    [JsonIgnore]
    public Curso Curso { get; set; } = null!;

    [ForeignKey(nameof(DocenteId))]
    [JsonIgnore]
    public Docente Docente { get; set; } = null!;
}
