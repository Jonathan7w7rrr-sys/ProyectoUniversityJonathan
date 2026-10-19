using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoUniversityJonathan.Models;

public class Curso
{
    [Key]
    public int CursoId { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Asignacion> Asignaciones { get; set; }
        = new List<Asignacion>();
}
