# Digital Service Insight Generator

A lightweight C# console application that leverages the OpenAI Chat Completion API to deliver structured, actionable insights about a digital service. Ideal for product managers, investors, and prospective users who need quick understanding of a product's core value, target audience, and growth potential.

---

## Features

* Interactive **multi-run loop** – analyse multiple services in one session
* Accepts either:
  * A block of descriptive text (multi-line paste supported)
  * A service name only
* Clean exit via menu option **3) Exit** or typing **q / quit / exit**
* Animated spinner while waiting for OpenAI – no more frozen console
* Generates comprehensive analysis including:
  * 📅 Brief History (founding year, milestones)
  * 👥 Target Audience (primary user segments)
  * ⭐ Core Features (top 2-4 key functionalities)
  * 🎯 Unique Selling Points (key differentiators)
  * 💰 Business Model (revenue streams)
  * 🔧 Tech Stack Insights (technologies used)
  * 📊 Strengths & Weaknesses analysis
* Outputs beautifully formatted markdown reports appended to a file

## Prerequisites

1. **.NET 8 SDK** (Download from https://dotnet.microsoft.com/download)
2. **OpenAI API Key**
   * Sign up at https://platform.openai.com
   * Create an API key
   * Set it via environment variable or config:
     ```powershell
     # PowerShell
     $env:OPENAI_API_KEY = 'your-key-here'
     
     # CMD
     set OPENAI_API_KEY=your-key-here
     
     # Bash
     export OPENAI_API_KEY=your-key-here
     ```
   * Or add to `appsettings.json`:
     ```json
     {
       "OpenAI": {
         "ApiKey": "your-key-here"
       }
     }
     ```

## Running the Application

1. **First Time Setup**
   ```powershell
   # Clone the repository (if you haven't already)
   git clone <repository-url>
   cd <repository-directory>

   # Restore dependencies
   dotnet restore
   ```

2. **Build & Run**
   ```powershell
   # Build the project
   dotnet build -c Release

   # Run the application
   dotnet run --project InsightGenerator
   ```

3. **Using the Application**
   
   The app now presents three options and stays running until you choose to exit:

   ```
   Select input mode:
   1) Paste description text (e.g., an 'About Us' section)
   2) Enter the service's name
   3) Exit
   ```

   * **Option 1: Paste Description**
     - Choose `1`
     - Paste your text (multiple lines allowed, press **Enter** on an empty line to finish)

   * **Option 2: Service Name**
     - Choose `2`
     - Type the name (e.g., "Spotify", "Slack")
     - Press Enter

   * **Option 3 / q / quit / exit**
      - Cleanly stops the program

   After each analysis finishes, the menu re-appears automatically so you can process another service without restarting the app.

   During the call to OpenAI you'll see a small spinner (| / – \) so you know the request is in progress.

4. **Output**
   
   The analysis will be saved to `sample_outputs.md` with sections:

   ```markdown
   # ServiceName – Digital Service Analysis
   
   ## 📅 Brief History
   [Founding details and key milestones]

   ## 👥 Target Audience
   - [Primary user segment]
   - [Secondary user segment]

   ## ⭐ Core Features
   1. [First key functionality]
   2. [Second key functionality]
   ...

   [Additional sections with insights]
   ```

   Each run appends to the file with a timestamp, allowing you to compare multiple analyses.

## Configuration

The application can be configured via `appsettings.json`:

```json
{
  "OpenAI": {
    "ApiKey": "your-key-here",
    "Model": "gpt-3.5-turbo",
    "Temperature": 0.2
  },
  "Output": {
    "FilePath": "sample_outputs.md"
  }
}
```

* **Model** – Use `gpt-3.5-turbo` for good balance of speed/quality, or `gpt-4` for deeper analysis
* **Temperature** – Lower values (0.2) for focused analysis, higher (0.8) for creative insights
* **FilePath** – Where to save the markdown output

## Example Usage

1. **Analyzing Spotify:**
   ```powershell
   dotnet run --project InsightGenerator
   # Choose option 2
   # Enter: Spotify
   ```

2. **Analyzing a startup's About page:**
   ```powershell
   dotnet run --project InsightGenerator
   # Choose option 1
   # Paste the About page text
   # Press Enter twice
   ```

## Limitations & Next Steps

* The service relies on OpenAI's model knowledge, which may not include very recent startups
* For service names, accuracy depends on the model's existing knowledge
* Consider adding:
  * Web scraping for richer data gathering
  * Export to other formats (PDF, HTML)
  * Integration with other AI models

---

© 2024 Insight Generator — MIT License 