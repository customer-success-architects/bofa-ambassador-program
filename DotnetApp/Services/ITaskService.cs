namespace DotnetApp.Services
{
    using DotnetApp.Models;
    using System.Collections.Generic;

    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAllTasks();
        TaskItem? GetTaskById(int id);
        void CreateTask(TaskItem task);
        bool UpdateTask(int id, TaskItem updatedTask);
        bool DeleteTask(int id);
    }
}
