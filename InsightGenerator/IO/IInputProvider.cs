namespace InsightGenerator.IO;

public interface IInputProvider
{
    /// <summary>
    /// Obtains user input either as service name or raw description.
    /// Returns tuple: (inputText, isServiceName)
    /// </summary>
    (string Text, bool IsServiceName) GetInput();
} 