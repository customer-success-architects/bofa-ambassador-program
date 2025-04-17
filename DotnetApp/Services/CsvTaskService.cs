using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DotnetApp.Models;

namespace DotnetApp.Services
{
    public class CsvTaskService : ITaskService
    {
        private readonly string _filePath;
        private readonly object _lock = new object();
        private int _nextId;

        public CsvTaskService()
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, "tasks.csv");
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "Id,Title,Description,IsCompleted,Status,Priority,CreatedAt\n");
            }
            var tasks = ReadAll();
            _nextId = tasks.Any() ? tasks.Max(t => t.Id) : 0;
        }

        private List<TaskItem> ReadAll()
        {
            var lines = File.ReadAllLines(_filePath);
            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split(',');
                    return new TaskItem
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1],
                        Description = string.IsNullOrEmpty(parts[2]) ? null : parts[2],
                        IsCompleted = bool.Parse(parts[3]),
                        Status = parts[4],
                        Priority = int.Parse(parts[5]),
                        CreatedAt = DateTime.Parse(parts[6], null, DateTimeStyles.RoundtripKind)
                    };
                })
                .ToList();
        }

        private void WriteAll(IEnumerable<TaskItem> tasks)
        {
            var lines = new List<string> { "Id,Title,Description,IsCompleted,Status,Priority,CreatedAt" };
            lines.AddRange(tasks.Select(t =>
                string.Join(",",
                    t.Id,
                    Escape(t.Title),
                    Escape(t.Description),
                    t.IsCompleted,
                    t.Status,
                    t.Priority,
                    t.CreatedAt.ToString("O")
                )
            ));
            File.WriteAllLines(_filePath, lines);
        }

        private string Escape(string? value) => value?.Replace("\"", "\"\"") ?? string.Empty;

        // Rename public methods to match ITaskService
        public IEnumerable<TaskItem> GetAllTasks() => ReadAll();

        public TaskItem? GetTaskById(int id) => ReadAll().FirstOrDefault(t => t.Id == id);

        public void CreateTask(TaskItem task)
        {
            lock (_lock)
            {
                task.Id = ++_nextId;
                var tasks = ReadAll();
                tasks.Add(task);
                WriteAll(tasks);
            }
        }

        public bool UpdateTask(int id, TaskItem updatedTask)
        {
            lock (_lock)
            {
                var tasks = ReadAll();
                var existing = tasks.FirstOrDefault(t => t.Id == id);
                if (existing == null) return false;
                updatedTask.Id = id;
                tasks.Remove(existing);
                tasks.Add(updatedTask);
                WriteAll(tasks);
                return true;
            }
        }

        public bool DeleteTask(int id)
        {
            lock (_lock)
            {
                var tasks = ReadAll();
                var removed = tasks.RemoveAll(t => t.Id == id) > 0;
                if (!removed) return false;
                WriteAll(tasks);
                return true;
            }
        }
    }
}
