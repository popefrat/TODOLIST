using Microsoft.AspNetCore.Cors;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    policy =>
    {
    policy.WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod();
    });
});
//יוצר מופע חדש בעבור כל גישה למסד נתונים
// builder.Services.AddDbContext<ToDoDbContext>();

//יוצר מופע יחיד המשמש לכל הגישות למסד נתונים
builder.Services.AddSingleton<ToDoDbContext>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
    });
});

var app = builder.Build();
app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// app.MapGet("/", () => "Hello World!");

app.MapGet("/item", (ToDoDbContext context) =>
{

  return context.Items.ToList();
});
app.MapPost("/item",async(ToDoDbContext context , Item item )=>{
    context.Add(item);
    await context.SaveChangesAsync();
    return item;
});

app.MapPut("/item/{id}", async(ToDoDbContext context,Item item, int id)=>{
    var existItem = await context.Items.FindAsync(id);
    if(existItem is null) return Results.NotFound();

    existItem.IsComplete = item.IsComplete;

    await context.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/item/{id}",async(ToDoDbContext context , int id)=>{
    var existItem = await context.Items.FindAsync(id);
    if(existItem is null) return Results.NotFound();

    context.Items.Remove(existItem);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();
