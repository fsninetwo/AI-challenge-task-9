using InsightGenerator.IO;

namespace InsightGenerator.IO;

internal sealed class ConsoleInputProvider : IInputProvider
{
    public (string Text, bool IsServiceName) GetInput()
    {
        Console.WriteLine("Select input mode:");
        Console.WriteLine("1) Paste description text (e.g., an 'About Us' section)");
        Console.WriteLine("2) Enter the service's name");
        Console.Write("Your choice (1 or 2): ");

        string? choice;
        do
        {
            choice = Console.ReadLine();
        } while (choice != "1" && choice != "2");

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