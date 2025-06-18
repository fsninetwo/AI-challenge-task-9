# Digital Service Insight Generator

A lightweight C# console application that leverages the OpenAI Chat Completion API to deliver structured, actionable insights about a digital service. Ideal for product managers, investors, and prospective users who need quick understanding of a product's core value, target audience, and growth potential.

---

## Features

* Accepts either:
  * A block of descriptive text (e.g., an "About Us" section), **or**
  * Just the service name.
* Sends a carefully-crafted prompt to OpenAI to extract:
  * Service overview
  * Unique value proposition
  * Target audience
  * Monetization model
  * Key features
  * Growth opportunities
  * Risks & challenges
* Prints the insights to the console in a readable format and also shows the raw JSON for debugging.

## Prerequisites

1. **.NET 7 SDK** (Download from https://dotnet.microsoft.com/download)
2. **OpenAI API Key**
   * Sign up at https://platform.openai.com.
   * Create an API key and store it as an environment variable named `OPENAI_API_KEY`.

## Getting Started

```powershell
# 1. Restore (there are no external NuGet dependencies, but this updates the SDK & tools)
dotnet restore

# 2. Build
dotnet build -c Release

# 3. Run
dotnet run --project InsightGenerator
```

> 💡 During startup the app will prompt you to choose between pasting a description or providing a service name. Follow the instructions in the console.

## Configuration

* **Model** – The application defaults to `gpt-3.5-turbo`. Change the constant in `OpenAIClient.cs` if you prefer a different model.
* **Temperature** – Set in `OpenAIClient.GetCompletionAsync()`. A lower temperature (e.g., 0.2) keeps the output focused and deterministic.

## Limitations & Next Steps

* No automatic web-scraping is performed when only a service name is provided. The prompt relies on the model's existing knowledge.
* Error handling is basic. Feel free to extend with retries and rate-limit backoff.
* Consider persisting insights to a database or exporting as CSV/Markdown for further reporting.

---

© 2023 Insight Generator — MIT License 