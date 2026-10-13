using TimeTracker.Dtos.Post;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class PostMapper {
    public static PostDto ToDto(this Post postModel) {
        return new PostDto() {
            Id = postModel.Id,
            Title = postModel.Title,
            Body = postModel.Body,
            CreatedOn = postModel.CreatedOn,
            IsDeleted = postModel.IsDeleted,
            User = postModel.User!.ToDto(),
        };
    }

    public static PagedPostDto ToDto(this PagedList<Post> pagedList) {
        return new PagedPostDto() {
            Items = pagedList.Items.Select(p => p.ToDto()).ToList(),
            PageNumber = pagedList.PageNumber,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasNextPage = pagedList.HasNextPage,
            HasPreviousPage = pagedList.HasPreviousPage,
        };
    }

    public static Post ToModel(this CreatePostDto postDto, string userId) {
        return new Post() {
            Title = postDto.Title.Trim(),
            Body = postDto.Body,
            CreatedOn = DateTime.UtcNow,
            UserId = userId,
        };
    }

    public static void UpdateModelFromDto(this Post postModel, UpdatePostDto postDto) {
        postModel.Title = postDto.Title.Trim();
        postModel.Body = postDto.Body;
    }
}
