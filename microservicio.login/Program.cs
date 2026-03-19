
using Microsoft.AspNetCore.Builder;
using servicios.Handlers;
using servicios.Repositories;
using servicios.v1;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(option => option.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin(); //Permite cualquier origen
            builder.AllowAnyMethod(); //Permite cualquier metodo (PUT, POST, DELEte)
            builder.AllowAnyHeader(); //Cualquier peticion mande el encabezado a peticion?
        }
));

//Config conexion SQLITE (despues)
SqliteHandler.ConnectionString = builder.Configuration.GetConnectionString("defaultConnection");

//Dependencias
builder.Services.AddSingleton<ILoginRepository, LoginService>(); //Cada vez q quiero agregar un servicio nuevo, lo agrego aca

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.login");
    c.RoutePrefix = string.Empty;
});

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
