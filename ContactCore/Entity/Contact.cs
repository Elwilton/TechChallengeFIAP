namespace ContactCore.Entity;

public class Contact
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string DDD { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
