using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using AvaloniaSandbox.ViewModels;
using ReactiveUI;

namespace AvaloniaSandbox.Pages.AsyncTest;

public class AsyncTestViewModel : ViewModelBase
{
    public AsyncTestViewModel()
    {
        ReactiveCommandAsync2 = ReactiveCommand.CreateFromTask(CallAsync2);
        var canExecute = new BehaviorSubject<bool>(true);
        CallAsync1Command = ReactiveCommand.CreateFromTask(CallAsync, canExecute);
        CallAsync2Command = ReactiveCommand.CreateFromTask(CallAsync2, canExecute);
        this.WhenAnyObservable(
                t => t.CallAsync1Command.IsExecuting,
                t => t.CallAsync2Command.IsExecuting
            ).Select(t => !t)
            .Subscribe(canExecute);
        CanExecute = canExecute.Select(t=>t);
    }

    public IObservable<bool> CanExecute { get; set; }

    public string Name
    {
        set => this.RaiseAndSetIfChanged(ref _name, value);
        get => _name;
    }

    private string _name = "something";

    public ReactiveCommand<Unit, Unit> ReactiveCommandAsync2 { get; private set; }
    public ReactiveCommand<Unit, Unit> CallAsync1Command { get; private set; }
    public ReactiveCommand<Unit, Unit> CallAsync2Command { get; private set; }

    public async Task CallAsync()
    {
        await Task.Delay(1000);
        Name = "CallAsync";
    }

    public async Task CallAsync2()
    {
        await Task.Delay(1000);
        Name = "CallAsync2";
    }

    public void CallAsyncDotWait()
    {
        Task.Delay(2000).ConfigureAwait(true).GetAwaiter().GetResult();
        Name = "CallAsyncDotWait";
    }
}