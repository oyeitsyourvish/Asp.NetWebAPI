namespace WebApiTut.CustomMV
{
    public class Logging
    {
        private readonly RequestDelegate _next;
        public Logging(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Handling Request " + context.Request.Path);
            await _next(context);
            Console.WriteLine(" Finished Handling Request");
        }
    }
}
