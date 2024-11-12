using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using ConsoleApp6;

namespace Lab6_AIS
{
    internal class Program
    {
        public static async Task Main()
        {
            var list = await GetProductLinks("https://e-catalog.co.uk/list/122/");
            using (var context = new AppDbContext())
            {
                foreach (var product in list)
                {
                    var newSmartphone = product;

                    // Добавляем новый объект в контекст
                    context.Smartphones.Add(newSmartphone);
                }
                // Сохраняем изменения в базе данных
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Смартфоны успешно добавлены в базу данных.");
            await DisplaySmartphonesFromDatabase();
        }

        public async static Task<List<Smartphone>> GetProductLinks(string url)
        {
            // Конфигурация AngleSharp с включенным HTTP-загрузчиком
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            // Загружаем документ с сайта
            var document = await context.OpenAsync(url);

            var productLinks = document.QuerySelectorAll("a.model-short-title.no-u");

            List<Smartphone> listName = new List<Smartphone>();

            foreach (var link in productLinks)
            {
                // Создаем новый объект Smartphone для каждого продукта
                var newSmartphone = new Smartphone();

                var href = link.GetAttribute("href");
                newSmartphone.Name = link.GetAttribute("title");

                if (!string.IsNullOrEmpty(href))
                {
                    // Добавляем базовый URL, если ссылка относительная
                    var fullUrl = href.StartsWith("http") ? href : new Uri(new Uri(url), href).ToString();

                    // Переход на страницу продукта
                    var productPage = await context.OpenAsync(fullUrl);

                    // Извлечение цены
                    var priceElement = productPage.QuerySelector("span[itemprop='lowPrice']");
                    newSmartphone.Cost = priceElement?.GetAttribute("content") ?? "Unknown";

                    // Извлечение информации о батарее
                    var batteryElement = productPage.QuerySelector("div.m-s-f3[title*='Battery']");
                    newSmartphone.Battery = batteryElement?.GetAttribute("title") ?? "Unknown";

                    // Извлечение информации о процессоре
                    var cpuElement = productPage.QuerySelector("div.m-s-f3[title*='CPU']");
                    newSmartphone.CPU = cpuElement?.GetAttribute("title") ?? "Unknown";
                }

                listName.Add(newSmartphone);
            }
            return listName;
        }

        public static async Task DisplaySmartphonesFromDatabase()
        {
            using (var context = new AppDbContext())
            {
                var smartphones = await context.Smartphones.ToListAsync();
                foreach (var smartphone in smartphones)
                {
                    Console.WriteLine($"Name: {smartphone.Name}, Price: {smartphone.Cost}, {smartphone.Battery}, {smartphone.CPU}");
                }
            }
        }

    }
}
