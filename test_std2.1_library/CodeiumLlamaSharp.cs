using LLama.Common;
using LLama;
using LLama.Native;
using LLama.Sampling;
using LLama.Transformers;
using System;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace test_std2._1_library
{
    public class CodeiumLlamaSharp
    {
        private static bool isInitialized = Initialize();

        private static bool Initialize()
        {
            // Configure logging. Change this to `true` to see log messages from llama.cpp
            var showLLamaCppLogs = false;
            NativeLibraryConfig
               .All
               .WithLogCallback((level, message) =>
               {
                   if (showLLamaCppLogs)
                       Console.WriteLine($"[llama {level}]: {message.TrimEnd('\n')}");
               });

            // Configure native library to use. This must be done before any other llama.cpp methods are called!
            NativeLibraryConfig
               .All
               .DryRun(out var loadedllamaLibrary, out var loadedLLavaLibrary);

            // Calling this method forces loading to occur now.
            NativeApi.llama_empty_call();

            return true;
        }

        public static async Task TestConsole(string[] args)
        {
            var modelPath = "C:/Users/vanva/source/Models_LLM/llama-2-7b-chat.Q4_K_M.gguf";
            var parameters = new ModelParams(modelPath)
            {
                Seed = 1337,
                GpuLayerCount = int.MaxValue,
                FlashAttention = true,
                ContextSize = 2048,
            };

            using var model = LLamaWeights.LoadFromFile(parameters);
            using var context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var chatHistoryJson = File.ReadAllText("./Assets/chat-with-bob.json");
            var chatHistory = ChatHistory.FromJson(chatHistoryJson) ?? new ChatHistory();

            ChatSession session = new ChatSession(executor, chatHistory);

            // add the default templator. If llama.cpp doesn't support the template by default, 
            // you'll need to write your own transformer to format the prompt correctly
            session.WithHistoryTransform(new PromptTemplateTransformer(model, withAssistant: true));

            // Add a transformer to eliminate printing the end of turn tokens, llama 3 specifically has an odd LF that gets printed sometimes
            string eos = model.Tokens.EndOfTurnToken ?? model.Tokens.EndOfSpeechToken
                ?? throw new Exception("End of speech or turn token not found");

            session.WithOutputTransform(new LLamaTransforms.KeywordTextOutputStreamTransform(
                new string[] { eos, "�" },
                redundancyLength: 5));

            var inferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f
                },

                MaxTokens = -1, // keep generating tokens until the anti prompt is encountered
                AntiPrompts = new string[] { model.Tokens.EndOfTurnToken! } // model specific end of turn string
            };

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The chat session has started.");

            // show the prompt
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("User> ");
            var userInput = Console.ReadLine() ?? "";

            while (userInput != "exit")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Assistant> ");

                // as each token (partial or whole word is streamed back) print it to the console, stream to web client, etc
                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                var message = new ChatHistory.Message(AuthorRole.User, userInput);
                await foreach (
                    var text
                    in session.ChatAsync(
                        message,
                        inferenceParams,
                        cancellationToken.Token))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (string.IsNullOrEmpty(text))
                    {
                        cancellationToken.Cancel();
                        break;
                    }
                    Console.Write(text);
                }
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("User> ");
                userInput = Console.ReadLine() ?? "";
            }
        }

    }
}
