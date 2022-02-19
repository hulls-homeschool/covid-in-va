using CovidInVaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Open Data Portal Settings
builder.Services.Configure<OpenDataPortalSettings>(options =>
{
    var section = builder.Configuration.GetSection("OpenDataPortal");
    if (section.Exists())
    {
        options.AppToken = section.GetValue("AppToken", string.Empty);
        options.Url = section.GetValue("Url", string.Empty);
    }
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHostedService, OpenDataPortalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
