using GameStore.Services;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class Program
{
    public static async Task Main(string[] args)
    {
        const string secretName = "troll-key";
        var keyVaultName = "gamestore-vault";
        var kvUri = $"https://{keyVaultName}.vault.azure.net";

        // Use DefaultAzureCredential for authentication
        var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
        string secretValue;

        try
        {
            // Get the secret from Azure Key Vault
            var secret = await client.GetSecretAsync(secretName);
            secretValue = secret.Value.Value;
            Debug.WriteLine($"Your secret is: {secret.Value.Value}");

        }
        catch (Exception ex)
        {
            secretValue = "Error retrieving secret";
            Console.WriteLine($"Error retrieving secret: {ex.Message}");
        }

        // Build and run the web application
        var builder = WebApplication.CreateBuilder(args);

        // Add services for MVC and GameService
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<GameService>();

        builder.Configuration["BlobSasToken"] = secretValue;

        var app = builder.Build();

        // Configure HTTP requests
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Games}/{action=Index}/{id?}");

        app.Run();
    }
}
