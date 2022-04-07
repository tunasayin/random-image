using System.Diagnostics;
using System.Net.Http;
using HtmlAgilityPack;
using Vlingo.UUID;

class RandomImage
{
    static string[] CHAR_LIST = {   
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z",
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9", 
    };

    static string[] USER_AGENTS =
    {
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/602.1.50 (KHTML, like Gecko) Version/10.0 Safari/602.1.50",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.11; rv:49.0) Gecko/20100101 Firefox/49.0",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_1) AppleWebKit/602.2.14 (KHTML, like Gecko) Version/10.0.1 Safari/602.2.14",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12) AppleWebKit/602.1.50 (KHTML, like Gecko) Version/10.0 Safari/602.1.50",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
        "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36",
        "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
        "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko",
        "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0",
        "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36",
        "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:49.0) Gecko/20100101 Firefox/49.0"
    };

    static string ARCHIVE_PATH = $"{Directory.GetCurrentDirectory()}/archive";

    static string GenerateRandomImageId()
    {
        string imageId = "";

        Random random = new Random();


        for (int i = 0; i < 5; i++)
        {
            imageId += CHAR_LIST[random.Next(0, CHAR_LIST.Length)];
        }

        return imageId;
    }

    static string GetRandomUserAgent()
    {
        Random random = new Random();

        return USER_AGENTS[random.Next(0, USER_AGENTS.Length)];
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

            dynamic imageUrl = doc.GetElementbyId("screenshot-image").GetAttributeValue("src", "not_found");

            if (imageUrl == "//st.prntscr.com/2022/02/22/0717/img/0_173a7b_211be8ff.png" || imageUrl == null) imageUrl = "not_found";

            if (imageUrl == "not_found") return await GetRandomImageUrl();
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

    static void Main(string[] args)
    {

        Console.Write("Please specify the image amount you want to fetch: ");

        try
        {
            int imageAmount = Convert.ToInt32(Console.ReadLine());

            FetchImagesAndWrite(imageAmount).Wait();
        }
        catch (Exception ex)
        {
            Console.Write(ex);
            Console.WriteLine("Invalid image amount was specified! Aborted.");
        }

        return;
    }
}