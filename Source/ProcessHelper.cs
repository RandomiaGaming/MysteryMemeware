//#Approve File 2022/08/04/PM/3/48
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
namespace MysteryMemeware
{
    public enum WindowMode { Default, Hidden, Minimized, Maximized }
    public enum TimeoutAction { Return, Throw, Kill, KillAndThrow }
    public static class ProcessHelper
    {
        public static bool IsAdmin()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new(identity);
                bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                try
                {
                    identity.Dispose();
                }
                catch
                {

                }
                return isAdmin;
            }
            catch
            {
                return false;
            }
        }
        public static Process StartAs(TerminalCommand command, UserRefrence user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            ProcessStartInfo processStartInfo = new()
            {
                Arguments = command.Arguments,
                CreateNoWindow = windowMode is WindowMode.Hidden,
                ErrorDialog = false,
                ErrorDialogParentHandle = IntPtr.Zero,
                FileName = command.FileName,
                LoadUserProfile = true,
                Password = null,
                PasswordInClearText = password,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                StandardErrorEncoding = null,
                StandardOutputEncoding = null,
                UseShellExecute = false,
                Verb = null
            };
            if (user.OnDefaultDomain)
            {
                processStartInfo.Domain = null;
            }
            else
            {
                processStartInfo.Domain = user.Domain;
            }
            processStartInfo.UserName = user.Name;
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
        public static Process Start(TerminalCommand command, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            ProcessStartInfo processStartInfo = new()
            {
                Arguments = command.Arguments,
                CreateNoWindow = windowMode is WindowMode.Hidden,
                Domain = null,
                ErrorDialog = false,
                ErrorDialogParentHandle = IntPtr.Zero,
                FileName = command.FileName,
                LoadUserProfile = false,
                Password = null,
                PasswordInClearText = null,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                StandardErrorEncoding = null,
                StandardOutputEncoding = null,
                UserName = null,
                UseShellExecute = true
            };
            if (elevate)
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
        public static bool AwaitSuccess(Process process, bool throwOnNonzeroExitCode = false)
        {
            while (!process.HasExited)
            {
            }
            if (process.ExitCode is not 0)
            {
                if (throwOnNonzeroExitCode)
                {
                    throw new Exception($"Process has terminated with a exit code of {process.ExitCode}.");
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
        public static bool AwaitSuccess(Process process, long timeoutTicks = 300000000, TimeoutAction timeoutAction = TimeoutAction.Return, bool throwOnNonzeroExitCode = false)
        {
            if (timeoutTicks < 0)
            {
                throw new Exception("timeoutTicks must be greater than or equal to 0.");
            }
            if (timeoutTicks == 0)
            {
                return AwaitSuccess(process, throwOnNonzeroExitCode);
            }
            Stopwatch timeoutStopwatch = new();
            timeoutStopwatch.Restart();
            while (!process.HasExited)
            {
                if (timeoutStopwatch.ElapsedTicks >= timeoutTicks)
                {
                    if (timeoutAction is TimeoutAction.Return)
                    {
                        return false;
                    }
                    if (timeoutAction is TimeoutAction.Kill)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                            throw new Exception("Process timed out and process could not be killed.");
                        }
                        return false;
                    }
                    if (timeoutAction is TimeoutAction.Throw || timeoutAction is TimeoutAction.KillAndThrow)
                    {
                        if (timeoutAction is TimeoutAction.KillAndThrow)
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch
                            {
                                throw new Exception("Process timed out and process could not be killed.");
                            }
                        }
                        throw new Exception("Process timed out.");
                    }
                }
            }
            if (process.ExitCode is not 0)
            {
                if (throwOnNonzeroExitCode)
                {
                    throw new Exception($"Process has terminated with a exit code of {process.ExitCode}.");
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
    }
    public sealed class TerminalCommand
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
                while (arguments.Length > 0 && arguments[0] == ' ')
                {
                    arguments = arguments.Substring(1, arguments.Length - 1);
                }
                while (arguments.Length > 0 && arguments[arguments.Length - 1] == ' ')
                {
                    arguments = arguments.Substring(0, arguments.Length - 1);
                }
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