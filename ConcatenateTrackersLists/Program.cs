using ConcatenateTrackersLists;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<Concater>();
builder.Configuration.AddJsonFile("TrackersListSource.json", optional: true, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/TrackersList", async () =>
    {
        var trackersListSource = app.Configuration.GetSection("TrackerListSource")
            .GetChildren().Select(c => c.Value!);
        return await app.Services.GetRequiredService<Concater>().Concat(trackersListSource);
    })
    .WithName("GetTrackersList")
    .WithOpenApi();

app.Run();
