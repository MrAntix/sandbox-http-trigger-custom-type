using System.Diagnostics;
using Xunit.Abstractions;

namespace Sandbox.HttpTriggerCustomType.Tests;

public static class FunctionsHelper
{
    public static IDisposable StartHost(int port, ITestOutputHelper output)
    {
        var host = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = "..\\..\\..\\..\\Sandbox.HttpTriggerCustomType\\",
                FileName = "func",
                Arguments = $"start -p {port}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            },
            EnableRaisingEvents = true
        };

        var wait = new AutoResetEvent(false);
        host.Exited += (_sender, _e) =>
        {
            wait.Set();
        };
        host.OutputDataReceived += (sender, e) =>
        {
            output.WriteLine(e.Data);

            if (e.Data is null) return;

            if (e.Data.Contains("The file is locked"))
                throw new Exception("File locked");

            if (e.Data.Contains("Worker process started and initialized"))
                wait.Set();
        };

        host.Start();
        host.BeginOutputReadLine();

        wait.WaitOne();

        return new HostDisposable(host);
    }

    class HostDisposable(
        Process host
        ) : IDisposable
    {
        public void Dispose()
        {
            if (host.HasExited) return;

            host.CancelOutputRead();
            host.Kill();
            host.Dispose();
        }
    }
}