using InsightGenerator.Models;

namespace InsightGenerator.IO;

public interface IOutputRenderer
{
    void RenderRaw(string raw);
    void RenderInsights(Insight insights);
} 