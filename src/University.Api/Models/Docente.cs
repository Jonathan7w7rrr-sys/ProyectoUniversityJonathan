using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoUniversityJonathan.Models;

public class Docente
{
    [Key]
    public int DocenteId { get; set; }

    [Required]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    public string Apellidos { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Asignacion> Asignaciones { get; set; }
        = new List<Asignacion>();
}
