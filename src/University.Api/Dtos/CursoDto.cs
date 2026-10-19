using System.ComponentModel.DataAnnotations;
namespace ProyectoUniversityJonathan.Dtos;
public class CursoDto
{
    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
}
