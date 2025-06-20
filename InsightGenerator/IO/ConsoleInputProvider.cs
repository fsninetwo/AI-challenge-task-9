using InsightGenerator.IO;

namespace InsightGenerator.IO;

internal sealed class ConsoleInputProvider : IInputProvider
{
    public (string Text, bool IsServiceName) GetInput()
    {
        Console.WriteLine("Select input mode:");
        Console.WriteLine("1) Paste description text (e.g., an 'About Us' section)");
        Console.WriteLine("2) Enter the service's name");
        Console.WriteLine("3) Exit");
        Console.Write("Your choice (1, 2 or 3): ");

        string? choice;
        while (true)
        {
            choice = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (choice is "1" or "2")
                break;
            if (choice is "3" or "q" or "quit" or "exit")
                return (string.Empty, false); // signal exit

            Console.Write("Invalid choice. Please enter 1, 2 or 3: ");
        }

        if (choice == "1")
        {
            Console.WriteLine("\nPaste the service description below. Finish with a single blank line:");
            var text = ReadMultiLineInput();
            return (text, false);
        }
        else
        {
            Console.Write("\nEnter the service name: ");
            var name = Console.ReadLine() ?? string.Empty;
            return (name, true);
        }
    }

    private static string ReadMultiLineInput()
    {
        var lines = new List<string>();
        string? line;
        while (true)
        {
            line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                break;
            lines.Add(line);
        }

        return string.Join(' ', lines);
    }
} 