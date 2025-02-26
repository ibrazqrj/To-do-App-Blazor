using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    /** ToDoContext ist die Datenbankkontextklasse, die die Entitäten in der Datenbank darstellt.
     * Sie enthält eine Eigenschaft TodoItems, die eine DbSet-Instanz von TodoItem ist.
     */
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) // Bei diesem Konstruktor wird DbContextOptions als Parameter übergeben mit dem Typ ToDoContext und options als Parameter damit die Optionen für den Kontext festgelegt werden können
            : base(options) // Ruft den Konstruktor der Basisklasse auf und übergibt die Optionen (Genauer sind die Optionen folgende: optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } // Getter und setter für DbSet-Instanz von TodoItem
    }
}
