using BlazorToDoApp.Resources;
using BlazorToDoApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks.Dataflow;


namespace BlazorToDoApp.Pages
{
    public class BaseTodoComponent : ComponentBase
    {
        // [Inject] protected HttpTodoService TodoService { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected IStringLocalizer<Translation> Localizer { get; set; } = default!;
        [Inject] protected IAccessTokenProvider TokenProvider { get; set; } = default!;
        [Inject] protected ITodoService TodoService { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;


        protected List<TodoItem>? todos;
        protected Virtualize<TodoItem>? virtualizeComponent;
        protected bool isLoading = true;
        protected int offset = 0;
        protected int limit = 10;
        protected bool isCompleteFilter = false;

        // Bei Initialisierung
        protected override async Task OnInitializedAsync()
        {
            try
            {
                todos = await TodoService.GetTodosAsync(isCompleted: isCompleteFilter);
                offset += limit;
                isLoading = false;
            }
            catch (HttpRequestException)
            {
                Snackbar.Add(@Localizer["Connection failed"], Severity.Error);
                isLoading = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der Daten: {ex.Message}");
                isLoading = false;
            }
        }

        // Buttons um Todo zu editieren oder ein neues hinzufügen.
        protected void OpenEditTodo(int id) => Navigation.NavigateTo($"/edittodo/{id}");
        protected void OpenAddTodo() => Navigation.NavigateTo("/addtodo");

        // isComplete true : false
        protected async Task UpdateTodoStatus(int id, bool isComplete)
        {
            try
            {
                var todo = todos?.FirstOrDefault(t => t.Id == id);
                if (todo == null) return;

                todo.IsComplete = isComplete;

                var success = await TodoService.UpdateTodoAsync(todo);

                if (success)
                {
                    todos = todos?.Where(t => t.Id != id).ToList();
                    if (virtualizeComponent != null)
                    {
                        await virtualizeComponent.RefreshDataAsync();
                    }
                    await InvokeAsync(StateHasChanged);
                    if (todo.IsComplete == true)
                    {
                        Snackbar.Add(@Localizer["Sucessfully moved your todo into the recycling bin!"], Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add(@Localizer["Sucessfully moved your todo into the active todos!"], Severity.Success);
                    }
                }
                else
                {
                    Console.WriteLine($"Fehler: Todo mit ID {id} konnte nicht aktualisiert werden.");
                }
            }
            catch (HttpRequestException)
            {
                Snackbar.Add(@Localizer["Connection failed"], Severity.Error);
            }
        }

        // Todos in der Virtualization mit Gruppierung anzeigen
        protected async ValueTask<ItemsProviderResult<TodoItem>> LoadTodos(ItemsProviderRequest request)
        {
            try
            {
                Console.WriteLine($"Loading Todos: StartIndex={request.StartIndex}, Count={request.Count}");

                var total = await TodoService.GetTodoCount(isCompleteFilter) ?? 0;
                var result = await TodoService.GetTodosAsync(request.StartIndex, request.Count, isCompleteFilter);

                Console.WriteLine($"Loaded {result.Count} Todos from DB: {string.Join(", ", result.Select(t => t.Title))}");

                var groupedTodos = new List<TodoItem>();
                var groupedByCategory = result.OrderBy(t => t.Time).GroupBy(t => GetTodoCategory(t.Time));

                foreach (var group in groupedByCategory)
                {
                    Console.WriteLine($"Processing group: {group.Key} with {group.Count()} items.");

                    groupedTodos.Add(new TodoItem
                    {
                        Id = -1,
                        Title = group.Key,
                        IsGrouped = true
                    });

                    foreach (var item in group)
                    {
                        Console.WriteLine($"Adding Todo: {item.Title}");
                        groupedTodos.Add(item);
                    }
                }

                Console.WriteLine($"Final grouped list contains {groupedTodos.Count} items.");

                return new ItemsProviderResult<TodoItem>(groupedTodos, total + groupedTodos.Where(x => x.Id == -1).Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler in LoadTodos: {ex.Message}");
                return new ItemsProviderResult<TodoItem>(Array.Empty<TodoItem>(), 0);
            }
        }

        protected async Task LoadNextItems()
        {
            todos?.AddRange(await TodoService.GetTodosAsync(offset, limit, isCompleteFilter));
            offset += limit;
            StateHasChanged();
        }

        protected async Task DeleteTodo(int id)
        {
            var success = await TodoService.DeleteTodoAsync(id);
            if (success)
            {
                todos = todos.Where(t => t.Id != id).ToList();
                await virtualizeComponent.RefreshDataAsync();
                Snackbar.Add(@Localizer["Sucessfully deleted your todo!"], Severity.Success);
            }
            else
            {
                Snackbar.Add(@Localizer["Couldn't delete your todo. Please try again!"], Severity.Error);
            }
        }

        protected async Task DeleteAllBinTodos()
        {
            try
            {
                var success = await TodoService.DeleteAllBinTodosAsync(isCompleteFilter);
                if (success)
                {
                    todos = todos.Where(t => t.IsComplete != isCompleteFilter).ToList();
                    await virtualizeComponent.RefreshDataAsync();
                    Snackbar.Add(@Localizer["Sucessfully cleared your recycling bin!"], Severity.Success);
                }
                else
                {
                    Console.WriteLine("Couldn't delete all bin tasks");
                }
            }
            catch (HttpRequestException)
            {
                Snackbar.Add(@Localizer["Something went wrong... Please try again!"], Severity.Warning);
            }
        }

        protected string GetTodoCategory(DateTime todoDate)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);

            if (todoDate.Date == today) return Localizer["TODAY"];
            if (todoDate.Date == tomorrow) return Localizer["TOMORROW"];
            if (todoDate.Date == yesterday) return Localizer["YESTERDAY"];
            if (todoDate.Date > tomorrow) return Localizer["PENDING"];
            return Localizer["PREVIOUS"];
        }
    }
}
