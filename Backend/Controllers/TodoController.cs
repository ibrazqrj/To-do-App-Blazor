using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    /** TodoController dient als Controller für die API.
     * Es enthält Methoden zum Abrufen, Hinzufügen, Aktualisieren und Löschen von TodoItems.
     */
    [ApiController]
    [Authorize]
    [Route("api/[controller]")] // Route zum Controller
    public class TodoController : ControllerBase
    {
        private readonly ToDoContext _context;

        public TodoController(ToDoContext context)
        {
            _context = context;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTodoCount([FromQuery] bool? completed = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var count = await _context.TodoItems.CountAsync(x => x.IsComplete == completed);

            return Ok(count);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos([FromQuery] int offset = 0, [FromQuery] int limit = 10, [FromQuery] bool? completed = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var todos = await _context.TodoItems.Where(t => t.UserId == userId && completed != null ? t.IsComplete == completed : true)
                .OrderBy(x => x.Time)
                .Skip(offset)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();

            int totalTasks = await _context.TodoItems.CountAsync(t => t.UserId == userId);
            int totalActiveTasks = await _context.TodoItems.CountAsync(t => t.UserId == userId && t.IsComplete == false);
            int totalBinTasks = await _context.TodoItems.CountAsync(t => t.UserId == userId && t.IsComplete == true);
        
            Response.Headers.Add("X-Total-Active-Count", totalActiveTasks.ToString());
            Response.Headers.Add("X-Total-Bin-Count", totalBinTasks.ToString());

            var testCount = await _context.TodoItems.CountAsync();

            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                               DB LOG                                 ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");
            Console.ResetColor();

            Console.WriteLine($"║ Gesamtzahl der ausstehenden Tasks gesendet: {totalActiveTasks,-25}║");
            Console.WriteLine($"║ Gesamtzahl der erledigten Tasks gesendet: {totalBinTasks,-25}  ║");
            Console.WriteLine($"║ Gesamtzahl in der Datenbank: {testCount,-30}          ║");
            Console.ResetColor();

            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var todo = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
            {
                return NotFound("The Task couldn't be found or is not yours!");
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodo([FromBody] TodoItem todoItem)
        {
            /* User enthält infos vom Benutzer und Claims eine Sammlung von Claims/Ansprüchen, die den Benutzer beschreiben.
             Es sucht nach einem Claim namens "sub", welche den Authentifizierungstoken oder Benutzer ID enthält. Falls kein Token vorhanden mit ? wirds nullable
            und somit entstehen keine Fehler, damit es weiterhin funktioniert. Wenn der Wert gefunden wird, wirds in Value gespeichert danach in userId*/
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) // Wenn der String/Token leer ist 
            {
                return Unauthorized("User is not authenticated"); // Als unauthorized melden und Fehlermeldung rausgeben
            }

            todoItem.UserId = userId; // Wenn Token einen Inhalt hat und UserId von todoItem dieselbe wie die von der Benutzerprüfung ist

            _context.TodoItems.Add(todoItem); // Die Eingabe zu todoItem hinzufügen
            await _context.SaveChangesAsync(); // Änderungen speichern

            /* Gibt die gespeicherten Daten "frei", heisst es wird ein Anonymes Objekt mit einer ID erstellt, welcher an todoItem übergeben wird
             und es eine tatsächliche ID erhält und in todoItem gespeichert wird*/
            return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] int id, [FromBody] TodoItem todoItem)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            if (id != todoItem.Id)
            {
                return BadRequest("The id does not match with the Body");
            }

            todoItem.UserId = userId;

            var existingTodo = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existingTodo == null)
            {
                return NotFound("The Task couldn't be found or is not yours!");
            }

            existingTodo.Title = todoItem.Title;
            existingTodo.Description = todoItem.Description;
            existingTodo.Time = todoItem.Time;
            existingTodo.IsComplete = todoItem.IsComplete;

            _context.Entry(existingTodo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound("Todo Item not found or ist not yours!");
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllBinTodos([FromQuery] bool? completed = true)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            var todosToDelete = _context.TodoItems
                .Where(t => t.UserId == userId && t.IsComplete == completed);

            _context.TodoItems.RemoveRange(todosToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
