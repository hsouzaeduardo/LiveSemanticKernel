using System.ComponentModel;
using System.Text.Json.Serialization;
using LiveSemanticKernel.Model;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins;
using Microsoft.SemanticKernel.Plugins.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LiveSemanticKernel
{
    internal static class _004HttpPlugin
    {
        public static async Task Run()
        {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddAzureOpenAIChatCompletion(
            deploymentName: Config.DeploymentName,
            endpoint: Config.Endpoint,
            apiKey: Config.ApiKey,
            modelId: Config.ModelId);

            builder.Plugins.AddFromType<HttpPlugin>();
            Kernel kernel = builder.Build();
            
            string input = @"Vou fazer uma entrega no CEP 06192000 qual é o nome da rua ";
            KernelArguments settings = new() { { "cep", "06192000" } };
            var result = await kernel.InvokePromptAsync(input, settings);
            Console.WriteLine(result);

            //string inputPraia = @"Vou a Santos, SP no qual será a temperatura.";
            //KernelArguments settingsPraia = new() { { "city", "Santos, SP" } };
            //var resultPraia = await kernel.InvokePromptAsync(inputPraia, settingsPraia);
            //Console.WriteLine(resultPraia);
        }

    }

    public class HttpPlugin
    {

        private static readonly HttpClient client = new HttpClient();

        [KernelFunction("GetAddress")]
        [Description("Retorna o endereço pelo número do CEP")]
        public async Task<string> GetAddress(KernelArguments arguments)
        {
            string cep = arguments["cep"] as string;
            string url = $"https://viacep.com.br/ws/{cep}/json/";

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            JObject addressData = JObject.Parse(responseBody);

            return $"A rua correspondente ao CEP {cep} é a {addressData["logradouro"].ToString()}";
        }

        [KernelFunction("GetWeatherAsync")]
        [Description("Retorna a temperatura pela Cidade")]
        public static async Task<string> GetWeatherAsync(KernelArguments arguments)
        {
            string city = arguments["city"] as string;
            string url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={Config.APIWeather}&units=metric";

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            WeatherClass weatherForecast = JsonConvert.DeserializeObject<WeatherClass>(responseBody);
            return  $"Retorna a Temperatura: {weatherForecast.list.FirstOrDefault().main.temp}°C";
        }
    }
}



