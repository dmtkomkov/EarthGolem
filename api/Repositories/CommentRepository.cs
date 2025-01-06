using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class CommentRepository : ICommentRepository {
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context) {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync() {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id) {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<Comment> CreateAsync(Comment commentModel) {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto commentDto) {
        var commentModel = await _context.Comments.FindAsync(id);

        if (commentModel == null) {
            return null;
        }
        
        commentModel.UpdateModelFromDto(commentDto);
        await _context.SaveChangesAsync();

        return commentModel;
    }
    
    public async Task<Comment?> DeleteAsync(int id) {
        var commentModel = await _context.Comments.FindAsync(id);

        if (commentModel == null) {
            return null;
        }

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }
    
    public Task<bool> CommentExists(int id) {
        return _context.Comments.AnyAsync(s => s.Id == id);
    }
}