using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMapper {
    public static CommentDto ToDto(this Comment commentModel) {
        return new CommentDto() {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            StockId = commentModel.StockId,
        };
    }
    
    public static void UpdateModelFromDto(this Comment model, UpdateCommentDto dto)
    {
        model.Title = dto.Title;
        model.Content = dto.Content;
    }
    
    public static Comment ToModel(this CreateCommentDto commentDto, int stockId) {
        return new Comment() {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId,
        };
    }
}