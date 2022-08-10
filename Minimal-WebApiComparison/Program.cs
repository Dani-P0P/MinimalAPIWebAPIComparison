global using Microsoft.EntityFrameworkCore;
global using Minimal_WebApiComparison.Data;
using Minimal_WebApiComparison;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>options.UseInMemoryDatabase("persondb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/person", async (DataContext context) => await context.Persons.ToListAsync());

app.MapGet("/person/{id}", async (DataContext context, int id) => await context.Persons.FindAsync(id)
    is Person person ? Results.Ok(person) : Results.NotFound("Persont not found."));

app.MapPost("/person", async (DataContext context, Person person) =>
{
    context.Persons.Add(person);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Persons.ToListAsync());
});

app.MapPut("/person/{id}", async (DataContext context, Person person, int id) =>
{
    var dbPerson = await context.Persons.FindAsync(id);
    if (dbPerson == null)
        return Results.NotFound("Person not found.");
    dbPerson.FirstName = person.FirstName;
    dbPerson.LastName = person.LastName;
    dbPerson.BirthDate = person.BirthDate;
    await context.SaveChangesAsync();
    return Results.Ok(await context.Persons.ToListAsync());
});

app.MapDelete("/person/{id}", async (DataContext context, int id) =>
{
    var dbPerson = await context.Persons.FindAsync(id);
    if (dbPerson == null)
        return Results.NotFound("Person not found.");
    context.Persons.Remove(dbPerson);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Persons.ToListAsync());
});

app.Run();