using System;
using AvaloniaSandbox.Pages.AsyncTest;
using AvaloniaSandbox.Pages.Welcome;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace AvaloniaSandbox.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        SetView(typeof(WelcomeViewModel));
    }
    
    public ViewModelBase SelectedViewModel
    {
        get => _selectedViewModel;
        set => this.RaiseAndSetIfChanged(ref _selectedViewModel, value);
    }

    private ViewModelBase _selectedViewModel;
    public void SetView(object t)
    {
        if (t is Type vmType && vmType.Name.EndsWith("ViewModel"))
        {
            if(vmType == SelectedViewModel?.GetType())
                return;
            
            using (var scope = _serviceProvider.CreateScope())
            {
                SelectedViewModel = (ViewModelBase)scope.ServiceProvider.GetRequiredService(t as Type);    
            }            
        }
        else
        {
            throw new InvalidOperationException();
        }


    }
}