using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Setup DB connection
DatabaseHandler.Setup();

// Allow CORS
string CorsPolicyName = "DevOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicyName,
                      builder => {builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();});
});

// Add services to the container.
builder.Services.AddControllers();

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
app.UseStaticFiles();
app.UseRouting();

app.UseCors(CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
