using System.ComponentModel.DataAnnotations;
namespace ProyectoUniversityJonathan.Dtos;
public class DocenteDto
{
    [Required, StringLength(100)]
    public string Nombres { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;
}
