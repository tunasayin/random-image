using System.Diagnostics;
using System.Net.Http;
using HtmlAgilityPack;
using Vlingo.UUID;

class RandomImage
{
    static string ARCHIVE_PATH = $"{Directory.GetCurrentDirectory()}/archive";

    static string GenerateRandomImageId()
    {
        string imageId = "";

        Random random = new Random();


        for (int i = 0; i < 5; i++)
        {
            imageId += Constants.CHAR_LIST[random.Next(0, Constants.CHAR_LIST.Length)];
        }

        return imageId;
    }

    static string GetRandomUserAgent()
    {
        Random random = new Random();

        return Constants.USER_AGENTS[random.Next(0, Constants.USER_AGENTS.Length)];
    }

    static async Task<string> GetRandomImageUrl()
    {
        string imageId = GenerateRandomImageId();
        string userAgent = GetRandomUserAgent();

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", userAgent);

        var siteData =  await client.GetStringAsync($"https://prnt.sc/{imageId}");
        
        if (siteData != null)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(siteData);

            dynamic? imageUrl = doc.GetElementbyId("screenshot-image")?.GetAttributeValue("src", null);

            if (imageUrl == "//st.prntscr.com/2022/02/22/0717/img/0_173a7b_211be8ff.png") imageUrl = null;

            if (imageUrl == null) return await GetRandomImageUrl();
            else return imageUrl;
        }
        else
        {
            return await GetRandomImageUrl();
        }

    }

    static async Task FetchImagesAndWrite(int imageAmount)
    {
        string images = "";

        for(var i = 0; i < imageAmount; i++)
        {
            var image = await GetRandomImageUrl();

            images += $"<img src={image}>\n\n";

            Console.WriteLine($"Fetched image {i + 1}/{imageAmount}");
        }

        if (!Directory.Exists(ARCHIVE_PATH)) Directory.CreateDirectory(ARCHIVE_PATH);

        RandomBasedGenerator generator = new RandomBasedGenerator();
        string fileName = generator.GenerateGuid().ToString();
        string filePath= Path.Join(ARCHIVE_PATH, fileName + ".html");

        await File.WriteAllTextAsync(filePath, images);

        Console.WriteLine($"Created Html file: {filePath}");

        // TODO: Add support for linux as well
        Process.Start(@"cmd.exe ", @"/c " + filePath);

        return; 
    }

    static void Main()
    {

        Console.Write("Please specify the image amount you want to fetch: ");

        try
        {
            int imageAmount = Convert.ToInt32(Console.ReadLine());

            FetchImagesAndWrite(imageAmount).Wait();
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid image amount was specified! Aborted.\n");

            Main();
        }

        Console.ReadKey();

        return;
    }
}