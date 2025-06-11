using Microsoft.AspNetCore.Mvc;
using UtterlyAPI.DAL;
using UtterlyAPI.Models;

namespace UtterlyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : Controller
{
    private readonly PostManager _postManager;

    public PostController(PostManager postManager)
    {
        _postManager = postManager;
    }
    [HttpGet]
    public async Task<List<UtterlyPost>> GetUtterlyPosts()
    {
        return await _postManager.GetPostsAsync();
    }
    [HttpGet("{id}")]
    public async Task<UtterlyPost?> GetPostById(int id)
    {
        var posts = await _postManager.GetPostsAsync();
        return posts.FirstOrDefault(p => p.Id == id);
    }
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] UtterlyPost post)
    {
        await _postManager.CreatePostAsync(post);
        return Ok(post);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] UtterlyPost post)
    {
        try
        {
            if (id != post.Id)
                return BadRequest("Post ID mismatch");

            var postToUpdate = await GetPostById(id);

            if (postToUpdate == null)
                return NotFound($"Post with Id = {id} not found");

            postToUpdate.Content = post.Content;
            postToUpdate.ParentPostId = post.ParentPostId;
            postToUpdate.ThreadId = post.ThreadId;
            postToUpdate.UserId = post.UserId;
            postToUpdate.CreatedAt = post.CreatedAt;

            return await _postManager.UpdatePostAsync(postToUpdate);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        await _postManager.DeletePostAsync(id);
        return Ok();
    }
    [HttpGet("search")]
    public async Task<List<UtterlyPost>> SearchPosts([FromQuery] string query)
    {
        var posts = await _postManager.GetPostsAsync();
        return posts.Where(p => p.Content.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    [HttpGet("User/{userId}")]
    public async Task<List<UtterlyPost>> GetPostsByUser(string userId)
    {
        var posts = await _postManager.GetPostsAsync();
        return posts.Where(p => p.UserId == userId).ToList();
    }
    [HttpGet("User/{userId}/count")]
    public async Task<int> GetPostCountByUser(string userId)
    {
        var posts = await _postManager.GetPostsAsync();
        return posts.Count(p => p.UserId == userId);
    }

    [HttpGet("parent/{parentId}")]
    public async Task<List<UtterlyPost>> GetPostsByParent(int parentId)
    {
        var posts = await _postManager.GetPostsAsync();
        return posts.Where(p => p.ParentPostId == parentId).ToList();
    }

    [HttpGet("Thread/{threadId}")]
    public async Task<List<UtterlyPost>> GetPostsByThread(int threadId)
    {
        return await _postManager.GetPostsByThreadAsync(threadId);
    }
}