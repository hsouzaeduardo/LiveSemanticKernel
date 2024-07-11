using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace LiveSemanticKernel
{
    internal class _003PluginKernel
    {

        /// <summary>
        /// Shows different ways to load a <see cref="KernelPlugin"/> instances.
        /// </summary>

        public static async Task RunAsync()
        {
            // Create a kernel with OpenAI chat completion
            var builder = Kernel.CreateBuilder();
            builder.Services.AddAzureOpenAIChatCompletion(
            deploymentName: Config.DeploymentName,
            endpoint: Config.Endpoint,
            apiKey: Config.ApiKey,
            modelId: Config.ModelId);

            builder.Plugins.AddFromType<TimeInformation>();
            builder.Plugins.AddFromType<WidgetFactory>();
            Kernel kernel = builder.Build();

            // Example 1. Invoke the kernel with a prompt that asks the AI for information it cannot provide and may hallucinate
            Console.WriteLine(await kernel.InvokePromptAsync("Quantos dias até o Natal?"));

            // Example 2. Invoke the kernel with a templated prompt that invokes a plugin and display the result
            Console.WriteLine(await kernel.InvokePromptAsync("A hora atual é {{TimeInformation.GetCurrentUtcTime}}. Quantos dias até o Natal?"));

            // Example 3. Invoke the kernel with a prompt and allow the AI to automatically invoke functions
            OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
            Console.WriteLine(await kernel.InvokePromptAsync("Quantos dias até o Natal? Explique seu pensamento.", new(settings)));

            // Example 4. Invoke the kernel with a prompt and allow the AI to automatically invoke functions that use enumerations
            Console.WriteLine(await kernel.InvokePromptAsync("Crie um widget prático de cor limão para mim.", new(settings)));
            Console.WriteLine(await kernel.InvokePromptAsync("Crie um lindo widget de cor escarlate para mim.", new(settings)));
            Console.WriteLine(await kernel.InvokePromptAsync("Crie um widget atraente em marrom e marinho para mim.", new(settings)));
        }

        /// <summary>
        /// A plugin that returns the current time.
        /// </summary>
        public class TimeInformation
        {
            [KernelFunction]
            [Description("Retrieves the current time in UTC.")]
            public string GetCurrentUtcTime() => DateTime.UtcNow.ToString("R");
        }

        /// <summary>
        /// A plugin that creates widgets.
        /// </summary>
        public class WidgetFactory
        {
            [KernelFunction]
            [Description("Cria um novo widget do tipo e cores especificados")]
            public WidgetDetails CreateWidget([Description("O tipo de widget a ser criado")] WidgetType widgetType, [Description("As cores do widget a ser criado")] WidgetColor[] widgetColors)
            {
                var colors = string.Join('-', widgetColors.Select(c => c.GetDisplayName()).ToArray());
                return new()
                {
                    SerialNumber = $"{widgetType}-{colors}-{Guid.NewGuid()}",
                    Type = widgetType,
                    Colors = widgetColors
                };
            }
        }

        /// <summary>
        /// A <see cref="JsonConverter"/> is required to correctly convert enum values.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum WidgetType
        {
            [Description("Um widget que é útil.")]
            Useful,

            [Description("Um widget decorativo.")]
            Decorative
        }

        /// <summary>
        /// A <see cref="JsonConverter"/> is required to correctly convert enum values.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum WidgetColor
        {
            [Description("Use ao criar um item vermelho")]
            Red,

            [Description("Use ao criar um item Verde.")]
            Green,

            [Description("Use ao criar um item Azul")]
            Blue
        }

        public class WidgetDetails
        {
            public string SerialNumber { get; init; }
            public WidgetType Type { get; init; }
            public WidgetColor[] Colors { get; init; }
        }
    }
}
