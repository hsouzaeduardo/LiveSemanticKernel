using System.ComponentModel;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins;
using Microsoft.SemanticKernel.Plugins.Core;


public static class PlugInKernel
{

    public static async Task Run()
    {
        var builder = Kernel.CreateBuilder();


        builder.Services.AddAzureOpenAIChatCompletion(
          deploymentName: Config.DeploymentName,
          endpoint: Config.Endpoint,
          apiKey: Config.ApiKey,
          modelId: Config.ModelId);

        builder.Plugins.AddFromType<ConversationSummaryPlugin>();
        builder.Plugins.AddFromType<SumarizationPuglin>();

        var kernel = builder.Build();

        string input = @"Sou vegana em busca de novas receitas. Adoro comida picante! Você pode me dar uma lista de receitas de café da manhã veganas?";

        var result = await kernel.InvokePromptAsync(input, new() { { "input", input } });

        Console.WriteLine(result);


        string functionPrompt = @"Histórico do usuário: 
                {{SumarizationPuglin.SummarizeConversation $history}}
        Dado o histórico deste usuário, forneça uma lista de receitas relevantes.";

        var resultFunction = await kernel.InvokePromptAsync(functionPrompt,
        new KernelArguments() { { "history", result } });

        Console.WriteLine(result);


    }


    public class ConversationSummaryPlugin
    {
        // Implementação da função GetConversationActionItems


        [KernelFunction, Description("Retorna uma lista de ações baseadas na entrada do usuário.")]
        public async Task<string> GetConversationActionItems(KernelArguments arguments)
        {
            string input = arguments["input"] as string;
            // Aqui você pode adicionar a lógica para processar a entrada e retornar o resultado.
            return $"Lista de receitasn na veganas de café da manhã baseadas no input: {input}";
        }

    }

    public class SumarizationPuglin
    {
        [KernelFunction, Description("Resume os nomes do pratros na conversa")]
        
        public string SummarizeConversation(KernelArguments arguments)

        {
            string history = arguments["history"] as string;
            // Aqui você pode adicionar a lógica para processar a entrada e retornar o resultado.
            return $"Lista apenas no nome dos pratos do historico: {history}";
        }
    }
}