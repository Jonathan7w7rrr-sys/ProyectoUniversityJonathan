using System.ComponentModel.DataAnnotations;
namespace ProyectoUniversityJonathan.Dtos;
public class EstudianteDto
{
    [Required, StringLength(100)]
    public string Nombres { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;
    [Required, StringLength(250)]
    public string Direccion { get; set; } = string.Empty;
}
