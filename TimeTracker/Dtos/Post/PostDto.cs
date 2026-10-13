using TimeTracker.Dtos.User;

namespace TimeTracker.Dtos.Post;

public class PostDto {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public bool IsDeleted { get; set; }
    public required UserDto User { get; set; }
}
