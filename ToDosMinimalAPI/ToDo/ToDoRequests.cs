namespace ToDosMinimalAPI.ToDo
{
    public class ToDoRequests
    {

        //enkapsulacja endpointów

        public static IResult GetAll(IToDoService service)
        {

            var todos = service.GetAll();
            return Results.Ok(todos);
    
         }

        ////
        /////1 commit
        /////2 commit
    }
}
