using Microsoft.AspNetCore.Mvc;
using ToDosMinimalAPI.ToDo;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Konfiguracja serwisów zalezności
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IToDoService, ToDoService>();
//rejestracja validatora w kontenerze zalezności po to by był poprawnie wstrzykniety do metody Create Delete
builder.Services.AddValidatorsFromAssemblyContaining(typeof (ToDoValidator));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))

        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
//middle ware autoryzacji
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CRUD

//Wykorzystanie delegaty do servisu

//ToDoRequests.RegisterEndpoints(app);
app.RegisterEndpoints();

//Wykorzystanie lambdy

//app.MapGet("/todos", (IToDoService service) => service.GetAll());
//app.MapGet("/todos/{id}", ([FromServices]IToDoService service,[FromRoute] Guid id) => service.GetById(id));
//app.MapPost("/todos", (IToDoService service, [FromBody]ToDo toDo) => service.Create(toDo));
//app.MapPut("/todos/{id}", (IToDoService service, Guid id, ToDo toDo) => service.Update(toDo));
//app.MapDelete("/todos/{id}", (IToDoService service, Guid id) => service.Delete(id));

app.MapGet("/token", () =>

{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier,"user-id"),
        new Claim(ClaimTypes.Name,"Test Name"),
        new Claim(ClaimTypes.Role,"Admin"),
    };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["JwtIssuer"],
        audience: builder.Configuration["Jwtissuer"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(60),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])), SecurityAlgorithms.HmacSha256)

    );

    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
    return jwtToken;
}).WithTags("JwtToken");
    
    
    

app.Run();
