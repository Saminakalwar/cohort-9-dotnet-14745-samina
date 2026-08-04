using TaskManagement.Persistence;


var builder = WebApplication.CreateBuilder(args);
// webApplication here is the host of our appliaction
// purpose is to introduce http server implementationfor our that we can start listening the hhtp requests

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

//here we define wht is going to happen when these http requests starts arriving into our application
app.MapGet("/", () => Results.Ok("Task Management API is running."));


app.Run();
