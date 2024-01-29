using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
class Program
{
    static async Task Main()
    {
        string baseUrl = "https://images-api.nasa.gov/search";
        string q = "Mars surface";
        string year_start = "2018";
        string year_end = "2018";
        string media_type = "image";
        string page_size = "5";

        // Construct the URL with parameters
        string apiUrl = baseUrl + $"?q={q}&year_start={year_start}&year_end={year_end}&media_type={media_type}&page_size={page_size}";
        await HoustoneMameProblem(apiUrl, ".jpg");

        q = "Mars";
        year_start = "2018";
        year_end = "2018";
        media_type = "video";
        page_size = "1";

        apiUrl = baseUrl + $"?q={q}&year_start={year_start}&year_end={year_end}&media_type={media_type}&page_size={page_size}";
        await HoustoneMameProblem(apiUrl, ".mp4");


    }
    static async Task HoustoneMameProblem(string apiUrl, string fileExtension)
    {
        using HttpClient client = new();
        try
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic? dynamicObject = JsonConvert.DeserializeObject<JObject>(responseBody);

                foreach (dynamic item in dynamicObject.collection.items)
                {
                    // Console.WriteLine(item.href);
                    using HttpClient client2 = new();
                    HttpResponseMessage response2 = await client2.GetAsync(item.href.ToString());
                    if (response2.IsSuccessStatusCode)
                    {
                        string response2Body = await response2.Content.ReadAsStringAsync();
                        // Console.WriteLine(response2Body);

                        string[] files = JsonConvert.DeserializeObject<string[]>(response2Body);
                        foreach (string file in files)
                        {
                            if (fileExtension == Path.GetExtension(file))
                            {
                                Console.WriteLine(file);
                                break;
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}
