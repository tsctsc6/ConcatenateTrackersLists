namespace ConcatenateTrackersLists;

public class Concater
{
    private readonly HttpClient httpClient;
    
    public Concater(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> Concat(IEnumerable<string> uris)
    {
        var results = await Task.WhenAll(uris.Select(Concat));
        return string.Join(Environment.NewLine + Environment.NewLine, results);
    }

    private async Task<string> Concat(string uri)
    {
        try
        {
            var resp = await httpClient.GetAsync(uri);
            resp.EnsureSuccessStatusCode();
            var content = await resp.Content.ReadAsStringAsync();
            return content;
        }
        catch (Exception e)
        {
            return $"{uri}: {e.Message}";
        }
    }
}