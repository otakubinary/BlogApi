using BlogApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Inyeccion de dependencias PATRON

builder.Services.AddDbContext<BlogDb>(options => options.UseSqlite("Data Source=BlogDb.db"));




var app = builder.Build();

app.MapGet("/", () => "Welcome to Blog API");


app.MapGet("/posts", async (BlogDb db) => await db.Posts.ToListAsync());

// SELECT 
app.MapGet("/posts/{id}", async (int id, BlogDb db) =>

    // si lo he encontrado en la base de datos 200 OK Si no lo encuentro 404 Not found HTTP Codes.
    await db.Posts.FindAsync(id) is Post post ? Results.Ok(post) : Results.NotFound()
);

// Insert
app.MapPost("/posts", async (Post post, BlogDb db) =>
{
    db.Posts.Add(post);
    await db.SaveChangesAsync();

    return Results.Created($"/posts/{post.PostId}", post); // Http 201 Created.
});

app.MapPut("/posts/{id}", async (int id, Post inputPost, BlogDb db) => // HTTP 204 No Content Ha ido todo ok si no
                                                                       // devolvemos un codigo 404
{
    var post = await db.Posts.FindAsync(id);

    if (post == null)
    {
        return Results.NotFound();
    }

    post.Content = inputPost.Content;

    await db.SaveChangesAsync();

    return Results.NoContent(); // -> 204 No Content
});

// Id 3 -> no lo encuentro en la base de datos devuelvo 404 
app.MapDelete("/posts/{id}", async (int id, BlogDb db) =>
{
    if (await db.Posts.FindAsync(id) is Post post)
    {
        db.Posts.Remove(post);
        await db.SaveChangesAsync();
        return Results.Ok(post);
    }

    return Results.NotFound();
});



app.Run();
