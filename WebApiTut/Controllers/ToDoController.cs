using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTut.DTO;
using WebApiTut.Models;

namespace WebApiTut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {

        // In-memory list to store to-do items for demonstration purposes
        private static List<ToDoItem> _toDoItems = new List<ToDoItem>()
        {
            new ToDoItem() { Id = 1, Title = "Buy groceries", Completed = false },
            new ToDoItem() { Id = 2, Title = "Walk the dog", Completed = true },
            new ToDoItem() { Id = 3, Title = "Finish homework", Completed = false }
        };


        // GET api/todo
        [HttpGet]
        public IActionResult GetAll()
        {
            if (_toDoItems.Count == 0)
            {
                // If there are no to-do items, return a 404 Not Found response
                return NotFound();
            }

            var toDoItemDTOs = _toDoItems.Select(item => MaToDoItemToDTO(item)).ToList();

            // Map the list of ToDoItem to a list of ToDoItemDTO
            return Ok(toDoItemDTOs);
        }


        // GET api/todo/1
        [HttpGet("{id}")]
        public IActionResult GetToDoItembyId(int id)
        {
            if (_toDoItems.Count == 0)
            {
                // If there are no to-do items, return a 404 Not Found response
                return NotFound();
            }

            var toDoItem = _toDoItems.FirstOrDefault(i => i.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            var toDoItemDTO = MaToDoItemToDTO(toDoItem);
            // If the item with the specified ID is not found, return a 404 Not Found response
            return Ok(toDoItemDTO);


        }

        // we are mapping the ToDoItem to ToDoItemDTO manually here, but in a real application, you
        // might want to use a library like AutoMapper for this purpose.
        private ToDoItemDTO MaToDoItemToDTO(ToDoItem item)
        {
            // Map a ToDoItem to a ToDoItemDTO
            return new ToDoItemDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Completed = item.Completed
            };
        }
    }
}
