using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.XPath;
using WebApiTut.DTO;
using WebApiTut.Models;

namespace WebApiTut.Controllers
{
    // this routing attribute defines the base route for all actions in this controller.
    // The [Action] token allows you to specify the action name in the URL, making it more flexible for different endpoints.
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
        /*Way of Attribut Routing
         [HttpGet("getAll")]
        another way
         [Route("getAll")]
        another way
         [Route("api/[controller]/[Action]")]   // in this case it atoumatically take action method name.
         */

        [HttpGet]
        [Route("GetAll")] // This route will be api/todo/GetAll
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
        [HttpGet("SearchById /{id}")] // This route will be api/todo/SearchById/{id}
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

        [HttpPost]
        public IActionResult CreateNewItem(ToDoItemDTO newItemDTO)
        {
            if (newItemDTO == null || string.IsNullOrEmpty(newItemDTO.Title))
            {
                // If the request body is null or the title is empty, return a 400 Bad Request response
                return BadRequest("Invalid to-do item data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newItem = new ToDoItem()
            {
                Id = _toDoItems.Count > 0 ? _toDoItems.Max(i => i.Id) + 1 : 1, // Generate a new ID
                Title = newItemDTO.Title,
                Completed = newItemDTO.Completed
            };
            _toDoItems.Add(newItem);
            var createdItemDTO = MaToDoItemToDTO(newItem);
            // Return a 201 Created response with the created item
            return CreatedAtAction(nameof(GetToDoItembyId), new { id = createdItemDTO.Id }, createdItemDTO);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateToDoItem(int id, [FromBody] ToDoItem UpdateToDoItem)
        {
            if (UpdateToDoItem == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingItem = _toDoItems.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                NotFound();
            }

            // Update the existing item with the new values
            existingItem.Title = UpdateToDoItem.Title;
            existingItem.Completed = UpdateToDoItem.Completed;

            // In a real application, you would typically save the updated item to a database here.
            var updatedItemDTO = MaToDoItemToDTO(existingItem);

            return Ok(updatedItemDTO);

        }

        [HttpPatch("{id}")]
        public IActionResult patchDocument(int id, [FromBody] JsonPatchDocument <ToDoItem> patchDoc)
        {
            // Check if the patch document is null
            if (patchDoc == null)
            {
                return BadRequest();
            }

            //retrieve the existing item from the in-memory list based on the provided ID
            var existingItem = _toDoItems.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }

            // Apply the patch document to the existing item.
            // This will modify the existing item based on the operations defined in the patch document.
            patchDoc.ApplyTo(existingItem);
            return Ok(existingItem);

        }

        [HttpDelete("{id}")]
        public IActionResult deleteToDoItem(int id)
        {
            var existingItem = _toDoItems.FirstOrDefault(i => i.Id == id);
            if(existingItem == null)
            {
                return NotFound();
            }
            _toDoItems.Remove(existingItem);
            return NoContent ();
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
