using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtterlyAPI.Models;

namespace UtterlyAPI.DAL;

public class PostManager
{
    private readonly UtterlyContext _utterlyContext;
    private List<UtterlyPost> _posts;

    public PostManager(UtterlyContext utterlyContext)
    {
        _utterlyContext = utterlyContext;
        _posts = new();

    }
    public async Task<List<UtterlyPost>> GetPostsAsync()
    {
        if (_posts.Count == 0)
        {
            _posts = await _utterlyContext.UtterlyPosts.ToListAsync();
        }
        return _posts;
    }
    public async Task<UtterlyPost?> GetPostByIdAsync(int id)
    {
        return await _utterlyContext.UtterlyPosts.FindAsync(id);
    }
    public async Task CreatePostAsync([FromBody] UtterlyPost post)
    {
        if (post == null) throw new ArgumentNullException(nameof(post));
        _utterlyContext.UtterlyPosts.Add(post);
        await _utterlyContext.SaveChangesAsync();
    }
    public async Task UpdatePostAsync(int id, [FromBody] UtterlyPost post)
    {
        var existingPost = _utterlyContext.UtterlyPosts.FirstOrDefault(p => p.Id == id);
        if (existingPost != null)
        {
            existingPost.Content = post.Content;
            _utterlyContext.UtterlyPosts.Update(existingPost);
        }
    }
    public async Task DeletePostAsync(int id)
    {
        var post = await _utterlyContext.UtterlyPosts.FindAsync(id);
        if (post != null)
        {
            _utterlyContext.UtterlyPosts.Remove(post);
            await _utterlyContext.SaveChangesAsync();
        }
    }
}
