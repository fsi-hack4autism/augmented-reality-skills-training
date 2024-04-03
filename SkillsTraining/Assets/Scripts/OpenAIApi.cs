using Azure;
using Azure.AI.OpenAI;
using static System.Environment;


string endpoint = "https://usecase5.openai.azure.com/";
string key = "f89e9e38f93448d8a99312bd95f5fd7e";

var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
  "GPT35Turbo",
  new ChatCompletionsOptions()
  {
      Messages =
    {
      new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information."),
      new ChatMessage(ChatRole.User, @"What is the highest mountain?"),
    },
      Temperature = (float)0.7,
      MaxTokens = 800,


      NucleusSamplingFactor = (float)0.95,
      FrequencyPenalty = 0,
      PresencePenalty = 0,
  });


ChatCompletions response = responseWithoutStream.Value;
Console.WriteLine($"Chatbot: {response.Choices[0].Message.Content}");



