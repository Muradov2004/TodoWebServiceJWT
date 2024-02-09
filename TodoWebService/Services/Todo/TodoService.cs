using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;
using TodoWebService.Providers;

namespace TodoWebService.Services.Todo;

public class TodoService(TodoDbContext context) : ITodoService
{
    private readonly TodoDbContext _context = context;

    public async Task<TodoItemDto> ChangeTodoItemStatus(ChangeStatusRequest request, UserInfo info)
    {
        var todoItem = await _context.TodoItems
            .Where(t => t.UserId == info.Id)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
        if (todoItem is not null)
        {
            todoItem.IsCompleted = request.IsCompeleted;
            todoItem.UpdatedTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return new(todoItem.Id, todoItem.Text, todoItem.IsCompleted, todoItem.CreatedTime);
        }
        else return null!;
    }

    public async Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request, UserInfo info)
    {
        var todo = new TodoItem()
        {
            Text = request.Text,
            IsCompleted = false,
            UserId = info.Id
        };
        await _context.TodoItems.AddAsync(todo);
        await _context.SaveChangesAsync();
        var lastItem = await _context.TodoItems
            .Where(t => t.UserId == info.Id)
            .OrderBy(t => t.Id)
            .LastAsync();
        return new TodoItemDto(lastItem.Id, lastItem.Text, lastItem.IsCompleted, lastItem.CreatedTime);
    }

    public async Task<bool> DeleteTodo(int id, UserInfo info)
    {
        var todoItem = await _context.TodoItems
            .Where(t => t.UserId == info.Id)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (todoItem != null)
        {
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }
        else return false;
    }

    public async Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, bool? isCompleted, UserInfo info)
    {
        IQueryable<TodoItem> query = _context.TodoItems
            .Where(t => t.UserId == info.Id)
            .AsQueryable();

        if (isCompleted.HasValue)
            query = query.Where(e => e.IsCompleted == isCompleted);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        var totalCount = await query.CountAsync();

        return new PaginatedListDto<TodoItemDto>(
            items.Select(e => new TodoItemDto(
                id: e.Id,
                text: e.Text,
                isCompleted: e.IsCompleted,
                createdTime: e.CreatedTime
            )),
            new PaginationMeta(page, pageSize, totalCount)
            );
    }

    public async Task<TodoItemDto?> GetTodoItem(int id, UserInfo info)
    {
        var todoItem = await _context.TodoItems
            .Where(t => t.UserId == info.Id)
            .FirstOrDefaultAsync(e => e.Id == id);

        return todoItem is not null
            ? new TodoItemDto(
                id: todoItem.Id,
                text: todoItem.Text,
                isCompleted: todoItem.IsCompleted,
                createdTime: todoItem.CreatedTime)
            : null;
    }
}
