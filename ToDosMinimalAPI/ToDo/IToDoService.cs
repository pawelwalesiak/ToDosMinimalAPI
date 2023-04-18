namespace ToDosMinimalAPI.ToDo
{
    public interface IToDoService
    {
        void Create(ToDo toDo);
        void Delete(Guid id);
        List<ToDo> GetAll();
        ToDo GetById(Guid Id);
        void Update(ToDo toDo);
    }
}