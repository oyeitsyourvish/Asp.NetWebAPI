// Note : this line like start the project engine
using WebApiTut.CustomMV;

var builder = WebApplication.CreateBuilder(args);


// Note : this line like enable the controller
// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
// Note : this line saying to descover all api endpoints
builder.Services.AddEndpointsApiExplorer();
// Note : This Create swagger doc or generate Api testing UI
builder.Services.AddSwaggerGen();


// Note : Everything is ready lets build the app
var app = builder.Build();

//Adding Custom Middleware in Request Processing Pipeline
app.UseMiddleware<Logging>(); // Custom Middleware 


// Note : This create the Url ex. localhost5156/swagger
// Configure Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); //Built-in Middileware - Non-terminal middleware.

// Note :  this HTTPS redirection http://localhot
app.UseHttpsRedirection(); // Built-in Middileware - terminal middleware.

// Note : For Autherization or Security Check Security Rules
app.UseAuthorization(); //Built-in Middileware - Non-terminal middleware.

// Note : Connect url to Ctrl method ex.api/todo (goes to ToDoController)
app.MapControllers(); //Built-in Middileware - Terminal middleware.

//Note : application is Running server is live now
app.Run();

/*
 - https://localhost:7093/swagger
 - Swagger shows:
                GET /api/todo
                GET /api/todo/{id}
 - User Clicks Execute on GET /api/todo
                GET /api/todo
 - Request Goes to ToDoController - Because of this:
                [Route("api/[controller]")] and ctrl name
 - [HttpGet] Method Executes This method runs:
                public IActionResult GetAll() - because request is:
                GET /api/todo
 - Data is Read from List
                private static List<ToDoItem> _toDoItems
 - This line runs:
                var toDoItemDTOs = _toDoItems.Select(item => MaToDoItemToDTO(item)).ToList();
    Why DTO ?
                Because we don’t want to expose full Model directly.
                We send only required fields.
 - Mapping Method Executes This method runs:
                private ToDoItemDTO MaToDoItemToDTO(ToDoItem item)
                It converts:
                Model -> ToDoItem
                        to
                DTO -> ToDoItemDTO
 - Response Sent Back
                return Ok(toDoItemDTOs);
                This sends:

                [
                  {
                    "id": 1,
                    "title": "Buy groceries",
                    "completed": false
                  }
                ]
 
 */