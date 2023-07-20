using System;
using TaskProximity.Models;

public class Comment
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Project Project { get; set; }
    public User User { get; set; }
}
