namespace WebApiTut.DTO
{
    public class ToDoItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; } = false;
    }
}
