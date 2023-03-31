using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly DataContext _context;

    public PostController(DataContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult Get([FromQuery] string lang)
    {
        var result = _context.Posts.AsEnumerable().Select(x => new
        {
            Id = x.Id,
            Title = x.Title.TryGetValue(lang, out string tvalue) ? tvalue : x.Title.FirstOrDefault().Value,
            Description = x.Description.TryGetValue(lang, out string dvalue) ? dvalue : x.Description.FirstOrDefault().Value
        }).ToList();

        return Ok(result);
    }

    [HttpPost]
    public IActionResult Post([FromBody] AddPost model)
    {
        var post = new Post
        {
            Title = new Dictionary<string, string>() { { model.Key, model.Title } },
            Description = new Dictionary<string, string>() { { model.Key, model.Content } }
        };
        _context.Posts.Add(post);
        _context.SaveChanges();
        return Ok();
    }

    [HttpPut]
    public IActionResult Update([FromBody] AddPost model)
    {
        var existing = _context.Posts.Find(model.Id);
        if (existing != null)
        {
            var title = existing.Title;
            if (title.ContainsKey(model.Key))
                title[model.Key] = model.Title;
            else
                title.Add(model.Key, model.Title);

            var description = existing.Description;
            if (description.ContainsKey(model.Key))
                description[model.Key] = model.Content;
            else
                description.Add(model.Key, model.Content);
            
            _context.Posts.Entry(existing).Properties
                .Where(x => x.Metadata.Name == "Title" || x.Metadata.Name == "Description")
                .ToList().ForEach(x => x.IsModified = true);
            existing.Title = title;
            existing.Description = description;
           var res =  _context.SaveChanges();
        }

        return Ok();
    }

    // record
    public class AddPost
    {
        public string Key { get; init; }
        public int Id { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
    }
}