﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ToDosMinimalAPI.ToDo
{
    public static class ToDoRequests 
    {

        //enkapsulacja endpointów

        public static WebApplication RegisterEndpoints(this WebApplication app) 
        {
            app.MapGet("/todos", ToDoRequests.GetAll)
                .Produces<List<ToDo>>()
                .WithTags("To Dos")
                ;

            app.MapGet("/todos/{id}", ToDoRequests.GetById)
                .Produces<ToDo>()
                .Produces(StatusCodes.Status404NotFound);
                ;
            app.MapPost("/todos", ToDoRequests.Create)
                .Produces<ToDo>(StatusCodes.Status201Created)
                .Accepts<ToDo>("application/json")
                .WithValidator<ToDo>()
                ;
            app.MapPut("/todos/{id}", ToDoRequests.Update)
                .WithValidator<ToDo>()
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Accepts<ToDo>("application/json")
                ;
            app.MapDelete("/todos/{id}", ToDoRequests.Delete)
                .Produces (StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                ;

            return app;
        }


        public static IResult GetAll(IToDoService service)
        {

            var todos = service.GetAll();
            return Results.Ok(todos);

        }


        public static IResult GetById(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(todo);
        }

        public static IResult Create(IToDoService service, ToDo toDo, IValidator<ToDo> validator)
        {
           
            service.Create(toDo);
            return Results.Created($"/todos/{toDo.Id}", toDo);
        }
        public static IResult Delete(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound();
            }
            service.Delete(id);
            return Results.NoContent();
        }
        public static IResult Update(IToDoService service, Guid id, ToDo toDo, IValidator<ToDo> validator) 
        {
            
            var todo = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound();
            }
            service.Update(toDo);
            return Results.Ok();
        }
        



    }
}
