using api_counter.wwwapi8;
using api_counter.wwwapi8.Helpers;
using api_counter.wwwapi8.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

CounterHelper.Initialize();

var counters = app.MapGroup("/counters");
//TODO: 1. write a method that returns all counters in the counters list.  use method below as a starting point
counters.MapGet("/", () =>
{
    return Results.Ok(CounterHelper.Counters);
});


//TODO: 2. write a method to return a single counter based on the id being passed in.  complete method below
counters.MapGet("/{id}", (int id) =>
{
    Counter? counter_ = CounterHelper.Counters.Find(x => x.Id == id);
    if(counter_ == null)
    {
        return Results.NotFound($"Task with id: {id} not found.");
    }
    return Results.Ok(counter_);
});

//TODO: 3.  write another controller method that returns counters that have a value greater than the {number} passed in.        
counters.MapGet("/greaterthan/{number}", (int number) =>
{
    var counter_ = CounterHelper.Counters.Where(x => x.Value > number).ToList();
    return Results.Ok(counter_);
});

////TODO:4. write another controller method that returns counters that have a value less than the {number} passed in.
counters.MapGet("/lessthan/{number}", (int number) =>
{
    var counter_ = CounterHelper.Counters.Where(x => x.Value < number).ToList();
    return Results.Ok(counter_);
});


//Extension #1
//TODO:  1. Write a controller method that increments the Value property of a counter of any given Id.
//e.g.  with an Id=1  the Books counter Value should be increased from 5 to 6
//return the counter you have increased
counters.MapPost("/increment/{id}", (int id) =>
{
    var counter_ = CounterHelper.Counters.Find(x =>x.Id == id);
    counter_.Value += 1;
    return Results.Ok(counter_);
});

//Extension #2
//TODO: 2. Write a controller method that decrements the Value property of a counter of any given Id.
//e.g.  with an Id=1  the Books counter Value should be decreased from 5 to 4
//return the counter you have decreased
counters.MapPost("/decrement/{id}", (int id) =>
{
    var counter_ = CounterHelper.Counters.Find(x => x.Id == id);
    counter_.Value -= 1;
    return Results.Ok(counter_);
});


app.Run();

