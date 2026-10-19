using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoUniversityJonathan.Models;

public class Estudiante
{
    [Key]
    public int EstudianteId { get; set; }

    [Required]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    public string Direccion { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Asignacion> Asignaciones { get; set; }
        = new List<Asignacion>();
}
