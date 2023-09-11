namespace APIEstudiantes;

public class Student
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public DateOnly BirthDate { get; set; }

    public string Email { get; set; }
}
