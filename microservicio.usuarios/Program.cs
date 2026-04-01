
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using servicios.Handlers;
using servicios.Repositories;
using servicios.v1;
using System.Text;

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

var jwt = builder.Configuration.GetSection("Jwt"); 
var secret = jwt["Secret"] ?? throw new InvalidOperationException("Jwt: Secret no configurado");
var issuer = jwt["Issuer"] ?? "microservicio.login";
var audience = jwt["Audience"] ?? "microservicio.login";

//Esto lo podemos copiar y pegar para la api que necesitemos utilizarlo
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddAuthentication("bearer").AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = issuer,
      ValidAudience = audience,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

    };
});
builder.Services.AddAuthorization();

//Config conexion SQLITE
SqliteHandler.ConnectionString = builder.Configuration.GetConnectionString("defaultConnection");

////Dependencias
//builder.Services.AddSingleton<ILoginRepository, LoginService>(); //Cada vez q quiero agregar un servicio nuevo, lo agrego aca

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.login");
    c.RoutePrefix = string.Empty;
});

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
