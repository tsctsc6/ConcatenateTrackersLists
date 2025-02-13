using System.Text.Json.Serialization;

namespace ConcatenateTrackersLists;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        /*
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
        });
        */

        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<Concater>();
        builder.Configuration.AddJsonFile("TrackersListSource.json", optional: true, reloadOnChange: true);
        
        var app = builder.Build();
        
        app.MapGet("/TrackersList", async () =>
        {
            var trackersListSource = app.Configuration.GetSection("TrackerListSource")
                .GetChildren().Select(c => c.Value!);
            return await app.Services.GetRequiredService<Concater>().Concat(trackersListSource);
        });

        app.Run();
    }
}

/*
public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
*/
