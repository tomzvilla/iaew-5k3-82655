using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace APIEstudiantes.Controllers;

[ApiController]
[Route("api/estudiantes")]
[Authorize]
public class StudentController : ControllerBase
{

    private static readonly string[] Names = new[]
    {
        "Tomas", "Alejo", "Maxi", "Alejandra", "Maria", "Pepe", "Lionel"
    };

    private static readonly string[] Lastnames = new[]
    {
        "Villarreal", "Dominguez", "Luna", "Perez", "Gomez", "Rodriguez", "Messi"
    };

    private static readonly string[] Emails = new[]
    {
        "email1@gmail.com", "Dominguez", "Luna", "Perez", "Gomez", "Rodriguez", "Messi"
    };

    private static readonly List<Student> Students = Enumerable.Range(1, 5).Select(index => 
        {
        string nombreAleatorio = Names[Random.Shared.Next(Names.Length)];
        string apellidoAleatorio = Lastnames[Random.Shared.Next(Lastnames.Length)];
    
        
        return new Student
        {
            Id = index,
            Nombre = nombreAleatorio,
            Apellido = apellidoAleatorio,
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Email = $"{nombreAleatorio.ToLower()}.{apellidoAleatorio.ToLower()}@gmail.com"
        };
        })
        .ToList();
    
    private readonly ILogger<StudentController> _logger;

    public StudentController(ILogger<StudentController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetStudents")]
    [Authorize("read:estudiantes")]
    public ActionResult<Student> GetStudents()
    {
        var students = Students;

        if(students == null)
        {
            return NotFound(); // Si no se encuentra el estudiante, devuelve un 404
        }

        return Ok(students); // Devuelve el estudiante encontrado
    }

    [HttpGet("{id}", Name = "GetStudentById")]
    [Authorize("read:estudiantes")]
    public ActionResult<Student> GetStudentById(int id)
    {
        var student = Students.FirstOrDefault(e => e.Id == id);

        if (student == null)
        {
            return NotFound(); // Si no se encuentra el estudiante, devuelve un 404
        }

        return Ok(student); // Devuelve el estudiante encontrado
    }

    [HttpPost]
    [Authorize("write:estudiantes")]
    public ActionResult<Student> PostStudent([FromBody] Student inputStudent)
    {
        int newId = Students.Count + 1;
        var newStudent = new Student
        {
            Id = newId,
            Nombre = inputStudent.Nombre,
            Apellido = inputStudent.Apellido,
            BirthDate = inputStudent.BirthDate,
            Email = inputStudent.Email
        };

        Students.Add(newStudent);

        return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id}, newStudent);
    }

    [HttpPut("{id}", Name="PutStudent")]
    [Authorize("write:estudiantes")]
    public ActionResult<Student> PutStudent(int id, [FromBody] Student inputStudent)
    {
        var student = Students.Find(std => std.Id == id);
        if(student == null) {
            return NotFound();
        }; 

        student.Nombre = inputStudent.Nombre ?? student.Nombre;
        student.Apellido = inputStudent.Apellido ?? student.Apellido;
        if(inputStudent != null) {
            student.BirthDate = inputStudent.BirthDate;
        }
        student.Email = inputStudent.Email ?? student.Email;

        return Ok(student); // Devuelve el estudiante encontrado
    }

    [HttpDelete("{id}", Name="DeleteStudent")]
    [Authorize("write:estudiantes")]
    public ActionResult<Student> DeleteStudent(int id)
    {
        var student = Students.Find(std => std.Id == id);
        if(student == null) {
            return NotFound();
        }; 
        Students.Remove(student);

        return Ok(student); // Devuelve el estudiante encontrado
    }
}
