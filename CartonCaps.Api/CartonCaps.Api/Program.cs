using CartonCaps.Repository;
using CartonCaps.Model;
using Microsoft.AspNetCore.Diagnostics;
using CartonCaps.Api.Repository.Interface;
using CartonCaps.Api.Service.Interface;
using CartonCaps.Api.Repository.Mock;
using CartonCaps.Common.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.Configure<MockStorageOptions>(builder.Configuration.GetSection(MockStorageOptions.SectionName)); 
builder.Services.AddSingleton<JsonMock>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IReferralRepository, ReferralRepository>(); 
builder.Services.AddScoped<IReferralService, CartonCaps.Service.ReferralService>();
builder.Services.AddScoped<IUserService, CartonCaps.Service.UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?
            .Error;

        context.Response.ContentType = "application/json";

        if (exception is NotFoundException)
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        else if (exception is CartonCaps.Common.Exceptions.ValidationException)
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        else if (exception is ConflictException)
            context.Response.StatusCode = StatusCodes.Status409Conflict;
        else if (exception is UnauthorizedActionException)
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        else
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(new
        {
            error = exception?.Message
        });
    });
});

app.UseAuthorization();

app.MapControllers();

app.Run();
