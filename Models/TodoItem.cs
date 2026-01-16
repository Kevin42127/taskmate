using System;
using System.Collections.Generic;

namespace TodoApp.Models;

public class TodoItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}

