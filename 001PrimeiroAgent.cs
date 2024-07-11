using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Agents.Chat;


namespace LiveSemanticKernel
{
    internal class _001PrimeiroAgent
    {
        private const string ParrotName = "LoroJose";
        private const string ParrotInstructions = "Repita a mensagem do usuário na voz de um pirata e termine com o som de um papagaio.";

        public static async Task Run()
        {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt3516",
                endpoint: Config.Endpoint,
                apiKey: Config.ApiKey,
                modelId: "gpt-35-turbo");

            var kernel = builder.Build();

#pragma warning disable SKEXP0001
            ChatCompletionAgent agent =
             new()
             {
                 Name = ParrotName,
                 Instructions = ParrotInstructions,
                 Kernel = kernel,
             };
            #pragma warning disable SKEXP0001

            ChatHistory chat = [];

        // Respond to user input
        await InvokeAgentAsync("A sorte favorece os que tem coragem.");
        await InvokeAgentAsync("Eu vim eu vi eu conquistei.");
        await InvokeAgentAsync("A prática leva à perfeição.");

        // Local function to invoke agent and display the conversation messages.
        async Task InvokeAgentAsync(string input)
        {
            chat.Add(new ChatMessageContent(AuthorRole.User, input));

            Console.WriteLine($"# {AuthorRole.User}: '{input}'");

            await foreach (ChatMessageContent content in agent.InvokeAsync(chat))
            {
                Console.WriteLine($"# {content.Role} - {content.AuthorName ?? "*"}: '{content.Content}'");
            }
        }
    }

}
}
