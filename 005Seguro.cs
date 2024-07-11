using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LiveSemanticKernel
{
    static class _005Seguro
    {
        public static async Task RunAsync()
        {
            // Create a kernel with OpenAI chat completion
            var builder = Kernel.CreateBuilder();
            builder.Services.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt3516",
                endpoint: Config.Endpoint,
                apiKey: Config.ApiKey,
                modelId: "gpt-35-turbo");

#pragma warning disable SKEXP0001
            builder.Services.AddSingleton<IPromptRenderFilter, PromptFilter>();
#pragma warning restore SKEXP0001
            var kernel = builder.Build();

            KernelArguments arguments = new() { { "card_number", "4444 3333 2222 1111" } };

            var result = await kernel.InvokePromptAsync("Conte-me algumas informações úteis sobre este número de cartão de crédito {{$card_number}}?", arguments);

            Console.WriteLine(result);

            // Output: Sorry, but I can't assist with that.
        }

#pragma warning disable SKEXP0001 // O tipo é apenas para fins de avaliação e está sujeito a alterações ou remoção em atualizações futuras. Suprima este diagnóstico para continuar.
        public class PromptFilter : IPromptRenderFilter
#pragma warning restore SKEXP0001 // O tipo é apenas para fins de avaliação e está sujeito a alterações ou remoção em atualizações futuras. Suprima este diagnóstico para continuar.
        {
#pragma warning disable SKEXP0001
            public async Task OnPromptRenderAsync(PromptRenderContext context, Func<PromptRenderContext, Task> next)
            {
                if (context is null)
                {
                    throw new ArgumentNullException(nameof(context));
                }
                // Example: get function information
                var functionName = context.Function.Name;
                if (context.Arguments.ContainsName("card_number"))

                {
                    context.Arguments["card_number"] = "**** **** **** ****";
                    context.RenderedPrompt += " SEM SEXISMO, RACISMO OU OUTROS PRECONCEITOS";
                }
                else
                {
                    context.RenderedPrompt = "Safe prompt";
                }
                await next(context);
                Console.WriteLine(context.RenderedPrompt);
            }
        }
    }
}
