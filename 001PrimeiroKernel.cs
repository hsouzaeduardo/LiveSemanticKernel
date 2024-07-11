using Microsoft.SemanticKernel;

public static class PrimeiroKernel { 

public static async Task Run(){
    var builder = Kernel.CreateBuilder();
    builder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt3516",
    endpoint: Config.Endpoint,
    apiKey: Config.ApiKey,
    modelId: "gpt-35-turbo");
    
    var kernel = builder.Build();

    string chatPrompt = """
        <message role="user">O que é Seattle?</message>
        <message role="system">Responsa em um JSON.</message>
        """;

        var result = await kernel.InvokePromptAsync(chatPrompt);

    Console.WriteLine(result);
}
}