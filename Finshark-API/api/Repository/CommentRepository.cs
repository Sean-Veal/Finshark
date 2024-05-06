using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentToRemove = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentToRemove == null) 
            {
                return null;
            }
            _context.Comments.Remove(commentToRemove);
            await _context.SaveChangesAsync();
            return commentToRemove;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a => a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var commentToUpdate = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentToUpdate == null) 
            {
                return null;
            }

            commentToUpdate.Title = comment.Title;
            commentToUpdate.Content = comment.Content;

            await _context.SaveChangesAsync();
            return commentToUpdate;
        }
    }
}