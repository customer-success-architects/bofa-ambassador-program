namespace DotnetApp.Services
{
    using System.Collections.Concurrent;
    using DotnetApp.Models;

    public class InMemoryTaskService : ITaskService
    {
        private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
        private int _nextId = 1;

        public IEnumerable<TaskItem> GetAllTasks() => _tasks.Values;

        public TaskItem? GetTaskById(int id) => _tasks.TryGetValue(id, out var task) ? task : null;

        public void CreateTask(TaskItem task)
        {
            var id = System.Threading.Interlocked.Increment(ref _nextId);
            task.Id = id;
            _tasks[id] = task;
        }

        public bool UpdateTask(int id, TaskItem updatedTask)
        {
            if (!_tasks.ContainsKey(id)) return false;
            updatedTask.Id = id;
            _tasks[id] = updatedTask;
            return true;
        }

        public bool DeleteTask(int id) => _tasks.TryRemove(id, out _);
    }
}
