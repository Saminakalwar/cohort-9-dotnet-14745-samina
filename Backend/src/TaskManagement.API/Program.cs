var builder = WebApplication.CreateBuilder(args);
// webApplication here is the host of our appliaction
// purpose is to introduce http server implementationfor our that we can start listening the hhtp requests

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

//here we define wht is going to happen when these http requests starts arriving into our application
app.MapGet("/", ()=>"Hello World");


app.Run();
