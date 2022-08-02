using System;
using System.Diagnostics;
namespace MysteryMemeware
{
    public static class ProcessHelper
    {
        public enum AdminMode { AlwaysAdmin, DefaultUser, SameAsParent }
        public enum WindowMode { Hidden, Default, Minimized, Maximized }
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
            return RunAs(ParseCommand(command), username, password);
        }
        public static Process RunAs(string command, string username, string password, WindowMode windowMode)
        {
            return RunAs(ParseCommand(command), username, password, windowMode);
        }
        public static Process RunAs(string command, string username, string password, WindowMode windowMode, string workingDirectory)
        {
            return RunAs(ParseCommand(command), username, password, windowMode, workingDirectory);
        }
        public static Process RunAs(Command command, string username, string password)
        {
            return RunAs(command, username, password, WindowMode.Default);
        }
        public static Process RunAs(Command command, string username, string password, WindowMode windowMode)
        {
            return RunAs(command, username, password, windowMode, null);
        }
        public static Process RunAs(Command command, string username, string password, WindowMode windowMode, string workingDirectory)
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
            processStartInfo.WorkingDirectory = workingDirectory;
            return Process.Start(processStartInfo);
        }
        public static Process Run(string command)
        {
            return Run(ParseCommand(command));
        }
        public static Process Run(string command, WindowMode windowMode)
        {
            return Run(ParseCommand(command), windowMode);
        }
        public static Process Run(string command, AdminMode adminMode)
        {
            return Run(ParseCommand(command), adminMode);
        }
        public static Process Run(string command, WindowMode windowMode, AdminMode adminMode)
        {
            return Run(ParseCommand(command), windowMode, adminMode);
        }
        public static Process Run(string command, WindowMode windowMode, AdminMode adminMode, string workingDirectory)
        {
            return Run(ParseCommand(command), windowMode, adminMode, workingDirectory);
        }
        public static Process Run(Command command)
        {
            return Run(command, WindowMode.Default, AdminMode.DefaultUser);
        }
        public static Process Run(Command command, WindowMode windowMode)
        {
            return Run(command, windowMode, AdminMode.DefaultUser);
        }
        public static Process Run(Command command, AdminMode adminMode)
        {
            return Run(command, WindowMode.Default, adminMode);
        }
        public static Process Run(Command command, WindowMode windowMode, AdminMode adminMode)
        {
            return Run(command, windowMode, adminMode, null);
        }
        public static Process Run(Command command, WindowMode windowMode, AdminMode adminMode, string workingDirectory)
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
            processStartInfo.WorkingDirectory = workingDirectory;
            return Process.Start(processStartInfo);
        }
        public enum TimeoutAction { StopWaiting, ThrowException, KillChild, KillChildAndThrowException }
        public static void AwaitSuccess(Process process)
        {
            AwaitSuccess(process, false);
        }
        public static void AwaitSuccess(Process process, bool throwOnNonzeroExitCode)
        {
            while (!process.HasExited)
            {

            }

            if (process.ExitCode is not 0 && throwOnNonzeroExitCode)
            {
                throw new Exception($"Child process has terminated with a response code of {process.ExitCode}.");
            }
        }
        public static void AwaitSuccess(Process process, TimeSpan timeout)
        {
            AwaitSuccess(process, timeout, TimeoutAction.StopWaiting);
        }
        public static void AwaitSuccess(Process process, TimeSpan timeout, bool throwOnNonzeroExitCode)
        {
            AwaitSuccess(process, timeout, TimeoutAction.StopWaiting, throwOnNonzeroExitCode);
        }
        public static void AwaitSuccess(Process process, TimeSpan timeout, TimeoutAction timeoutAction)
        {
            AwaitSuccess(process, timeout, timeoutAction);
        }
        public static void AwaitSuccess(Process process, TimeSpan timeout, TimeoutAction timeoutAction, bool throwOnNonzeroExitCode)
        {
            if (timeout.Ticks < 0)
            {
                throw new Exception("timeout cannot be a negative amount of time.");
            }
            if (timeout.Ticks == 0)
            {
                AwaitSuccess(process);
                return;
            }

            Stopwatch timeoutStopwatch = new Stopwatch();
            timeoutStopwatch.Restart();

            while (!process.HasExited)
            {
                if (timeoutStopwatch.ElapsedTicks >= timeout.Ticks)
                {
                    if (timeoutAction is TimeoutAction.StopWaiting)
                    {
                        return;
                    }
                    if (timeoutAction is TimeoutAction.KillChild)
                    {
                        process.Kill();
                        return;
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

            if (process.ExitCode is not 0 && throwOnNonzeroExitCode)
            {
                throw new Exception($"Child process has terminated with a response code of {process.ExitCode}.");
            }
        }
        public static Command ParseCommand(string command)
        {
            if (command is null)
            {
                return new Command("", "");
            }
            while (command.Length > 0 && command[0] == ' ')
            {
                command = command.Substring(1, command.Length - 1);
            }
            while (command.Length > 0 && command[command.Length - 1] == ' ')
            {
                command = command.Substring(0, command.Length - 1);
            }
            if (command.Length <= 0)
            {
                return new Command("", "");
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
                    splitIndex++;
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
                    if (command[splitIndex] == ' ')
                    {
                        break;
                    }
                    splitIndex++;
                }
            }
        QuoteFound:
            string fileName = command.Substring(0, splitIndex);
            string arguments = command.Substring(splitIndex + 1, command.Length - splitIndex - 1);
            while (arguments.Length > 0 && arguments[0] == ' ')
            {
                arguments = arguments.Substring(1, arguments.Length - 1);
            }
            return new Command(fileName, arguments);
        }
        public struct Command
        {
            public string FileName;
            public string Arguments;
            public Command(string fileName, string arguments)
            {
                FileName = fileName;
                Arguments = arguments;
            }
        }
    }
}