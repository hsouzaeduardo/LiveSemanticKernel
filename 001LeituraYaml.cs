using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

using Microsoft.SemanticKernel.PromptTemplates.Handlebars;


namespace LiveSemanticKernel
{
    public static class _001LeituraYaml
    {
        public static async Task RunAsync()
        {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt3516",
            endpoint: Config.Endpoint,
            apiKey: Config.ApiKey,
            modelId: "gpt-35-turbo");

            var kernel = builder.Build();

            using var reader = new StreamReader($"{Directory.GetCurrentDirectory()}\\Files\\estoria.yaml");
            var generateStoryYaml = reader.ReadToEnd();
            var yamlChat = kernel.CreateFunctionFromPrompt(generateStoryYaml);

            Console.WriteLine(await kernel.InvokeAsync(yamlChat, arguments: new()
            {
                { "topic", "Cachorro" },
                { "length", "3" },
            }));

        }
    }
}
