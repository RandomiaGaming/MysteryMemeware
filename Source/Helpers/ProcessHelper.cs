using System;
using System.ComponentModel;
using System.Diagnostics;
namespace MysteryMemeware
{
    public static class ProcessHelper
    {
        public static readonly Process CurrentProcess = Process.GetCurrentProcess();

        public static Process StartAs(string fileName, string arguments, string username, string domain, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(fileName, arguments), new UsernameDomainPair(username, domain), password, windowMode, workingDirectory);
        }
        public static Process StartAs(string command, string username, string domain, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(command), new UsernameDomainPair(username, domain), password, windowMode, workingDirectory);
        }
        public static Process StartAs(TerminalCommand command, string username, string domain, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(command, new UsernameDomainPair(username, domain), password, windowMode, workingDirectory);
        }
        public static Process StartAs(string command, string username, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(command), new UsernameDomainPair(username), password, windowMode, workingDirectory);
        }
        public static Process StartAs(TerminalCommand command, string username, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(command, new UsernameDomainPair(username), password, windowMode, workingDirectory);
        }
        public static Process StartAs(string fileName, string arguments, UsernameDomainPair user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(fileName, arguments), user, password, windowMode, workingDirectory);
        }
        public static Process StartAs(string command, UsernameDomainPair user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(command), user, password, windowMode, workingDirectory);
        }
        public static Process StartAs(TerminalCommand command, UsernameDomainPair user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            if (command is null)
            {
                throw new Exception("command cannot be null.");
            }
            if (user is null)
            {
                throw new Exception("user cannot be null.");
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                Arguments = command.Arguments,
                CreateNoWindow = windowMode is WindowMode.Hidden,
                ErrorDialog = false,
                ErrorDialogParentHandle = IntPtr.Zero,
                FileName = command.FileName,
                LoadUserProfile = true,
                Password = null,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                StandardErrorEncoding = null,
                StandardOutputEncoding = null,
                UseShellExecute = false,
                Verb = null
            };
            if (password is null)
            {
                processStartInfo.PasswordInClearText = "";
            }
            else
            {
                processStartInfo.PasswordInClearText = password;
            }
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
            if (workingDirectory is "")
            {
                processStartInfo.WorkingDirectory = null;
            }
            else
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            return Process.Start(processStartInfo);
        }

        public static Process Start(string fileName, string arguments, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            return Start(new TerminalCommand(fileName, arguments), windowMode, elevate, workingDirectory);
        }
        public static Process Start(string command, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            return Start(new TerminalCommand(command), windowMode, elevate, workingDirectory);
        }
        public static Process Start(TerminalCommand command, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            if (command is null)
            {
                throw new Exception("command cannot be null.");
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
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
            if (workingDirectory is "")
            {
                processStartInfo.WorkingDirectory = null;
            }
            else
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            try
            {
                return Process.Start(processStartInfo);
            }
            catch (Win32Exception win32Exception)
            {
                if (elevate && win32Exception.NativeErrorCode is 123456789)
                {
                    throw new UserDeclinedUACException(win32Exception);
                }
                else
                {
                    throw win32Exception;
                }
            }
        }

        public static bool AwaitSuccess(Process process, bool throwOnNonzeroExitCode = false)
        {
            while (!process.HasExited)
            {
            }
            if (!(process.ExitCode is 0))
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
        public static bool AwaitSuccess(Process process, TimeSpan timeout, TimeoutAction timeoutAction = TimeoutAction.Return, bool throwOnNonzeroExitCode = false)
        {
            if (timeout.Ticks < 0)
            {
                throw new Exception("timeout.Ticks must be greater than or equal to 0.");
            }
            if (timeout.Ticks == 0)
            {
                return AwaitSuccess(process, throwOnNonzeroExitCode);
            }
            Stopwatch timeoutStopwatch = new Stopwatch();
            timeoutStopwatch.Start();
            while (!process.HasExited)
            {
                if (timeoutStopwatch.ElapsedTicks >= timeout.Ticks)
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
                            throw new ProcessTimedOutException(process);
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
                                throw new ProcessTimedOutException(process);
                            }
                        }
                        throw new ProcessTimedOutException(process);
                    }
                }
            }
            if (!(process.ExitCode is 0))
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
    public sealed class UnexpectedExitCodeException : Exception
    {
        public readonly Process targetProcess;
        public readonly int exitCode;
        public UnexpectedExitCodeException(Process targetProcess) : base("Process timed out.", null)
        {
            if (targetProcess is null)
            {
                throw new Exception("targetProcess cannot be null.");
            }
            this.targetProcess = targetProcess;
            exitCode = targetProcess.ExitCode;
            HelpLink = "";
            HResult = 0;
            Source = "System.Process.Start while attempting to invoke ShellExecuteExA within shellapi.h. For more info on ShellExecuteExA see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa.";
        }
    }
    public sealed class ProcessTimedOutException : Exception
    {
        public readonly Process targetProcess;
        public ProcessTimedOutException(Process targetProcess) : base("Process timed out.", null)
        {
            if (targetProcess is null)
            {
                throw new Exception("targetProcess cannot be null.");
            }
            this.targetProcess = targetProcess;
            HelpLink = "";
            HResult = 0;
            Source = "System.Process.Start while attempting to invoke ShellExecuteExA within shellapi.h. For more info on ShellExecuteExA see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa.";
        }
    }
    public sealed class UserDeclinedUACException : Exception
    {
        public readonly Win32Exception baseWin32Exception;
        public UserDeclinedUACException(Win32Exception baseWin32Exception) : base("Could not start process because user declined administrator access prompt (UAC).", baseWin32Exception)
        {
            if (baseWin32Exception is null)
            {
                throw new Exception("baseWin32Exception cannot be null.");
            }
            if (!(baseWin32Exception.NativeErrorCode is 123456789))
            {
                throw new Exception("baseWin32Exception must have native error code 123456789.");
            }
            this.baseWin32Exception = baseWin32Exception;
            HelpLink = "https://docs.microsoft.com/en-us/windows/security/identity-protection/user-account-control/how-user-account-control-works";
            HResult = 1123456789;
            Source = "System.Process.Start while attempting to invoke ShellExecuteExA within shellapi.h. For more info on ShellExecuteExA see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa.";
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
            if (fileName is "")
            {
                Command = Arguments;
            }
            else
            {
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
    public enum TimeoutAction { Return, Throw, Kill, KillAndThrow }
}