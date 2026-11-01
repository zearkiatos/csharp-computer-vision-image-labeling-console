using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using ComputerVisionImageLabelingConsole.Configuration;

class Program
{
    private static readonly string endpoint = AppConfiguration.azureComputerVisionBaseUrl;
    private static readonly string apiKey = AppConfiguration.azureComputerVisionApiKey;

    static async Task Main(string[] args)
    {
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
        {
            Endpoint = endpoint
        };

        // URL de la imagen con SAS
        string imageUrl = AppConfiguration.azureBlobStorageImageUrl;
        try
        {
            var tags = await GetImageTags(client, imageUrl);
            Console.WriteLine("Tags:");
            foreach (var tag in tags)
            {
                Console.WriteLine($"- {tag.Name} (Confidence: {tag.Confidence})");
            }
        }
        catch (ComputerVisionErrorResponseException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    static async Task<IList<ImageTag>> GetImageTags(ComputerVisionClient client, string imageUrl)
    {
        var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Tags };

        var response = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

        return response.Tags;
    }
}

