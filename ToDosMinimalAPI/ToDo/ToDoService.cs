namespace ToDosMinimalAPI.ToDo
{
    public class ToDoService : IToDoService
    {
        //ctor
        public ToDoService()
        {
            var sampleToDo = new ToDo { Value = "MinimalApi" };
            _toDos[sampleToDo.Id] = sampleToDo;
        }

        private readonly Dictionary<Guid, ToDo> _toDos = new();

        public ToDo GetById(Guid Id)
        {
            return _toDos.GetValueOrDefault(Id);
        }

        public List<ToDo> GetAll()
        {
            return _toDos.Values.ToList();
        }

        public void Create(ToDo toDo)
        {
            if (toDo is null)
            {
                return;
            }

            _toDos[toDo.Id] = toDo;
        }

        public void Update(ToDo toDo)
        {
            var existingToDo = GetById(toDo.Id);
            if (existingToDo is null)
            {
                return;
            }
            _toDos[toDo.Id] = toDo;
        }

        public void Delete(Guid id)
        {
            _toDos.Remove(id);
        }


    }
}
