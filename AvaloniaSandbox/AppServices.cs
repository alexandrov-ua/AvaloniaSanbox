using AvaloniaSandbox.Pages.AsyncTest;
using AvaloniaSandbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaSandbox;

public static class AppServices
{
    public static void AddCommonServices(this IServiceCollection collection) {
        collection.AddTransient<MainViewModel>();
        collection.AddTransient<AsyncTestViewModel>();
    }
}