//#Approve File 08/03/2022 11:35am.
using System;
using System.Diagnostics;
using System.IO;

namespace MysteryMemeware
{
    public static class ProcessHelper
    {
        public enum AdminMode { SameAsParent, AlwaysAdmin }
        public enum WindowMode { Default, Hidden, Minimized, Maximized }
        public static bool CurrentProcessIsAdmin()
        {
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                bool isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                try
                {
                    identity.Dispose();
                }
                catch (Exception ex)
                {

                }
                return isAdmin;
            }
            catch
            {
                return false;
            }
        }
        public static Process RunAs(string command, string username, string password)
        {
            return RunAs(new TerminalCommand(command), username, password);
        }
        public static Process RunAs(string command, string username, string password, WindowMode windowMode)
        {
            return RunAs(new TerminalCommand(command), username, password, windowMode);
        }
        public static Process RunAs(string command, string username, string password, WindowMode windowMode, string workingDirectory)
        {
            return RunAs(new TerminalCommand(command), username, password, windowMode, workingDirectory);
        }
        public static Process RunAs(TerminalCommand command, string username, string password)
        {
            return RunAs(command, username, password, WindowMode.Default);
        }
        public static Process RunAs(TerminalCommand command, string username, string password, WindowMode windowMode)
        {
            return RunAs(command, username, password, windowMode, null);
        }
        public static Process RunAs(TerminalCommand command, string username, string password, WindowMode windowMode, string workingDirectory)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = command.Arguments;
            processStartInfo.CreateNoWindow = windowMode is WindowMode.Hidden;
            processStartInfo.Domain = null;
            processStartInfo.ErrorDialog = false;
            processStartInfo.ErrorDialogParentHandle = IntPtr.Zero;
            processStartInfo.FileName = command.FileName;
            processStartInfo.LoadUserProfile = true;
            processStartInfo.Password = null;
            processStartInfo.PasswordInClearText = password;
            processStartInfo.RedirectStandardError = false;
            processStartInfo.RedirectStandardInput = false;
            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.StandardErrorEncoding = null;
            processStartInfo.StandardOutputEncoding = null;
            processStartInfo.UserName = username;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Verb = null;
            if (windowMode is WindowMode.Hidden)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            else if (windowMode is WindowMode.Maximized)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            }
            else if (windowMode is WindowMode.Minimized)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            }
            else
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            }
            if (workingDirectory is null || workingDirectory is "")
            {
                try
                {
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(command.FileName);
                }
                catch
                {
                    processStartInfo.WorkingDirectory = null;
                }
            }
            else
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            return Process.Start(processStartInfo);
        }
        public static Process Run(string command)
        {
            return Run(new TerminalCommand(command));
        }
        public static Process Run(string command, WindowMode windowMode)
        {
            return Run(new TerminalCommand(command), windowMode);
        }
        public static Process Run(string command, AdminMode adminMode)
        {
            return Run(new TerminalCommand(command), adminMode);
        }
        public static Process Run(string command, WindowMode windowMode, AdminMode adminMode)
        {
            return Run(new TerminalCommand(command), windowMode, adminMode);
        }
        public static Process Run(string command, WindowMode windowMode, AdminMode adminMode, string workingDirectory)
        {
            return Run(new TerminalCommand(command), windowMode, adminMode, workingDirectory);
        }
        public static Process Run(TerminalCommand command)
        {
            return Run(command, WindowMode.Default);
        }
        public static Process Run(TerminalCommand command, WindowMode windowMode)
        {
            return Run(command, windowMode, AdminMode.SameAsParent);
        }
        public static Process Run(TerminalCommand command, AdminMode adminMode)
        {
            return Run(command, WindowMode.Default, adminMode);
        }
        public static Process Run(TerminalCommand command, WindowMode windowMode, AdminMode adminMode)
        {
            return Run(command, windowMode, adminMode, null);
        }
        public static Process Run(TerminalCommand command, WindowMode windowMode, AdminMode adminMode, string workingDirectory)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = command.Arguments;
            processStartInfo.CreateNoWindow = windowMode is WindowMode.Hidden;
            processStartInfo.Domain = null;
            processStartInfo.ErrorDialog = false;
            processStartInfo.ErrorDialogParentHandle = IntPtr.Zero;
            processStartInfo.FileName = command.FileName;
            processStartInfo.LoadUserProfile = false;
            processStartInfo.Password = null;
            processStartInfo.PasswordInClearText = null;
            processStartInfo.RedirectStandardError = false;
            processStartInfo.RedirectStandardInput = false;
            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.StandardErrorEncoding = null;
            processStartInfo.StandardOutputEncoding = null;
            processStartInfo.UserName = null;
            processStartInfo.UseShellExecute = true;
            if (adminMode is AdminMode.AlwaysAdmin || (adminMode is AdminMode.SameAsParent && CurrentProcessIsAdmin()))
            {
                processStartInfo.Verb = "runas";
            }
            else
            {
                processStartInfo.Verb = null;
            }
            if (windowMode is WindowMode.Hidden)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            else if (windowMode is WindowMode.Maximized)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            }
            else if (windowMode is WindowMode.Minimized)
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            }
            else
            {
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
            }
            if (workingDirectory is null || workingDirectory is "")
            {
                try
                {
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(command.FileName);
                }
                catch
                {
                    processStartInfo.WorkingDirectory = null;
                }
            }
            else
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            return Process.Start(processStartInfo);
        }
        public enum TimeoutAction { StopWaiting, ThrowException, KillChild, KillChildAndThrowException }
        public static bool AwaitSuccess(Process process)
        {
            return AwaitSuccess(process, false);
        }
        public static bool AwaitSuccess(Process process, bool throwOnNonzeroExitCode)
        {
            while (!process.HasExited)
            {

            }

            if (process.ExitCode is not 0)
            {
                if (throwOnNonzeroExitCode)
                {
                    throw new Exception($"Child process has terminated with a response code of {process.ExitCode}.");
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public static bool AwaitSuccess(Process process, TimeSpan timeout)
        {
            return AwaitSuccess(process, timeout, TimeoutAction.StopWaiting);
        }
        public static bool AwaitSuccess(Process process, TimeSpan timeout, TimeoutAction timeoutAction)
        {
            return AwaitSuccess(process, timeout, timeoutAction, false);
        }
        public static bool AwaitSuccess(Process process, TimeSpan timeout, bool throwOnNonzeroExitCode)
        {
            return AwaitSuccess(process, timeout, TimeoutAction.StopWaiting, throwOnNonzeroExitCode);
        }
        public static bool AwaitSuccess(Process process, TimeSpan timeout, TimeoutAction timeoutAction, bool throwOnNonzeroExitCode)
        {
            if (timeout.Ticks < 0)
            {
                throw new Exception("timeout cannot be a negative amount of time.");
            }
            if (timeout.Ticks == 0)
            {
                return AwaitSuccess(process, throwOnNonzeroExitCode);
            }

            Stopwatch timeoutStopwatch = new Stopwatch();
            timeoutStopwatch.Restart();

            while (!process.HasExited)
            {
                if (timeoutStopwatch.ElapsedTicks >= timeout.Ticks)
                {
                    if (timeoutAction is TimeoutAction.StopWaiting)
                    {
                        return false;
                    }
                    if (timeoutAction is TimeoutAction.KillChild)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                            throw new Exception("Process timeout expired and child process could not be killed while waiting for process to exit.");
                        }
                        return false;
                    }
                    if (timeoutAction is TimeoutAction.ThrowException || timeoutAction is TimeoutAction.KillChildAndThrowException)
                    {
                        if (timeoutAction is TimeoutAction.KillChildAndThrowException)
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch
                            {
                                throw new Exception("Process timeout expired and child process could not be killed while waiting for process to exit.");
                            }
                        }
                        throw new Exception("Process timeout expired while waiting for process to exit.");
                    }
                }
            }

            if (process.ExitCode is not 0)
            {
                if (throwOnNonzeroExitCode)
                {
                    throw new Exception($"Child process has terminated with a response code of {process.ExitCode}.");
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public struct TerminalCommand
        {
            public readonly string FileName = "";
            public readonly string Arguments = "";
            public readonly string Command = "";
            public TerminalCommand(string command)
            {
                if (command is null || command is "")
                {
                    FileName = "";
                    Arguments = "";
                    Command = "";
                    return;
                }
                while (command.Length > 0 && command[0] == ' ')
                {
                    command = command.Substring(1, command.Length - 1);
                }
                while (command.Length > 0 && command[command.Length - 1] == ' ')
                {
                    command = command.Substring(0, command.Length - 1);
                }
                if (command.Length == 0)
                {
                    FileName = "";
                    Arguments = "";
                    Command = "";
                    return;
                }
                int splitIndex = 0;
                if (command[0] == '"')
                {
                    command = command.Substring(1, command.Length - 1);
                    while (splitIndex < command.Length)
                    {
                        if (command[splitIndex] == '"')
                        {
                            goto QuoteFound;
                        }
                        else
                        {
                            splitIndex++;
                        }
                    }
                    throw new Exception("Invalid command due to unbalanced quotes.");
                }
                else
                {
                    while (splitIndex < command.Length)
                    {
                        if (command[splitIndex] == '"')
                        {
                            throw new Exception("Invalid command due to unexpected quote.");
                        }
                        else if (command[splitIndex] == ' ')
                        {
                            break;
                        }
                        else
                        {
                            splitIndex++;
                        }
                    }
                }
            QuoteFound:
                string fileName = command.Substring(0, splitIndex);
                string arguments = command.Substring(splitIndex + 1, command.Length - splitIndex - 1);
                while (arguments.Length > 0 && arguments[0] == ' ')
                {
                    arguments = arguments.Substring(1, arguments.Length - 1);
                }
                FileName = fileName;
                Arguments = arguments;
                if (fileName.Contains(" "))
                {
                    if (arguments is "")
                    {
                        Command = $"\"{fileName}\"";
                    }
                    else
                    {
                        Command = $"\"{fileName}\" {arguments}";
                    }
                }
                else
                {
                    if (arguments is "")
                    {
                        Command = $"{fileName}";
                    }
                    else
                    {
                        Command = $"{fileName} {arguments}";
                    }
                }
            }
            public TerminalCommand(string fileName, string arguments)
            {
                if (fileName is null)
                {
                    FileName = "";
                }
                else
                {
                    FileName = fileName;
                }
                if (arguments is null)
                {
                    Arguments = "";
                }
                else
                {
                    Arguments = arguments;
                }
                if (fileName.Contains(" "))
                {
                    if (arguments is "")
                    {
                        Command = $"\"{fileName}\"";
                    }
                    else
                    {
                        Command = $"\"{fileName}\" {arguments}";
                    }
                }
                else
                {
                    if (arguments is "")
                    {
                        Command = $"{fileName}";
                    }
                    else
                    {
                        Command = $"{fileName} {arguments}";
                    }
                }
            }
        }
    }
}