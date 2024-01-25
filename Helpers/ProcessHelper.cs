namespace MysteryMemeware
{
    public enum WindowMode { Default, Hidden, Minimized, Maximized }
    public static class ProcessHelper
    {
        public static readonly System.Diagnostics.Process CurrentProcess = System.Diagnostics.Process.GetCurrentProcess();
        public static System.Diagnostics.Process StartAs(string fileName, string arguments, string username, string domain, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(fileName, arguments), new UsernameDomainPair(username, domain), password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(string command, string username, string domain, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(command), new UsernameDomainPair(username, domain), password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(TerminalCommand command, string username, string domain, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(command, new UsernameDomainPair(username, domain), password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(string command, string username, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(command), new UsernameDomainPair(username), password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(TerminalCommand command, string username, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(command, new UsernameDomainPair(username), password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(string fileName, string arguments, UsernameDomainPair user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(fileName, arguments), user, password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(string command, UsernameDomainPair user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            return StartAs(new TerminalCommand(command), user, password, windowMode, workingDirectory);
        }
        public static System.Diagnostics.Process StartAs(TerminalCommand command, UsernameDomainPair user, string password, WindowMode windowMode = WindowMode.Default, string workingDirectory = null)
        {
            if (command is null)
            {
                throw new System.Exception("command cannot be null.");
            }
            if (user is null)
            {
                throw new System.Exception("user cannot be null.");
            }
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                Arguments = command.Arguments,
                CreateNoWindow = windowMode is WindowMode.Hidden,
                ErrorDialog = false,
                ErrorDialogParentHandle = System.IntPtr.Zero,
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
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            }
            else if (windowMode is WindowMode.Maximized)
            {
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            }
            else if (windowMode is WindowMode.Minimized)
            {
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
            }
            else
            {
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            }
            if (workingDirectory is "")
            {
                processStartInfo.WorkingDirectory = null;
            }
            else
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            return System.Diagnostics.Process.Start(processStartInfo);
        }
        public static System.Diagnostics.Process Start(string fileName, string arguments, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            return Start(new TerminalCommand(fileName, arguments), windowMode, elevate, workingDirectory);
        }
        public static System.Diagnostics.Process Start(string command, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            return Start(new TerminalCommand(command), windowMode, elevate, workingDirectory);
        }
        public static System.Diagnostics.Process Start(TerminalCommand command, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            if (command is null)
            {
                throw new System.Exception("command cannot be null.");
            }
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                Arguments = command.Arguments,
                CreateNoWindow = windowMode is WindowMode.Hidden,
                Domain = null,
                ErrorDialog = false,
                ErrorDialogParentHandle = System.IntPtr.Zero,
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
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            }
            else if (windowMode is WindowMode.Maximized)
            {
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            }
            else if (windowMode is WindowMode.Minimized)
            {
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
            }
            else
            {
                processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
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
                return System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception win32Exception)
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
        public static bool AwaitSuccess(System.Diagnostics.Process process, bool throwOnNonzeroExitCode = false)
        {
            while (!process.HasExited)
            {
            }
            if (!(process.ExitCode is 0))
            {
                if (throwOnNonzeroExitCode)
                {
                    throw new System.Exception($"Process has terminated with a exit code of {process.ExitCode}.");
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
        public static bool AwaitSuccess(System.Diagnostics.Process process, System.TimeSpan timeout, TimeoutAction timeoutAction = TimeoutAction.Return, bool throwOnNonzeroExitCode = false)
        {
            if (timeout.Ticks < 0)
            {
                throw new System.Exception("timeout.Ticks must be greater than or equal to 0.");
            }
            if (timeout.Ticks == 0)
            {
                return AwaitSuccess(process, throwOnNonzeroExitCode);
            }
            System.Diagnostics.Stopwatch timeoutStopwatch = new System.Diagnostics.Stopwatch();
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
                    throw new System.Exception($"Process has terminated with a exit code of {process.ExitCode}.");
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
    public sealed class UnexpectedExitCodeException : System.Exception
    {
        public readonly System.Diagnostics.Process targetProcess;
        public readonly int exitCode;
        public UnexpectedExitCodeException(System.Diagnostics.Process targetProcess) : base("Process timed out.", null)
        {
            if (targetProcess is null)
            {
                throw new System.Exception("targetProcess cannot be null.");
            }
            this.targetProcess = targetProcess;
            exitCode = targetProcess.ExitCode;
            HelpLink = "";
            HResult = 0;
            Source = "System.Process.Start while attempting to invoke ShellExecuteExA within shellapi.h. For more info on ShellExecuteExA see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa.";
        }
    }
    public sealed class ProcessTimedOutException : System.Exception
    {
        public readonly System.Diagnostics.Process targetProcess;
        public ProcessTimedOutException(System.Diagnostics.Process targetProcess) : base("Process timed out.", null)
        {
            if (targetProcess is null)
            {
                throw new System.Exception("targetProcess cannot be null.");
            }
            this.targetProcess = targetProcess;
            HelpLink = "";
            HResult = 0;
            Source = "System.Process.Start while attempting to invoke ShellExecuteExA within shellapi.h. For more info on ShellExecuteExA see https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shellexecuteexa.";
        }
    }
    public sealed class UserDeclinedUACException : System.Exception
    {
        public readonly System.ComponentModel.Win32Exception baseWin32Exception;
        public UserDeclinedUACException(System.ComponentModel.Win32Exception baseWin32Exception) : base("Could not start System.Diagnostics.Process because user declined administrator access prompt (UAC).", baseWin32Exception)
        {
            if (baseWin32Exception is null)
            {
                throw new System.Exception("baseWin32Exception cannot be null.");
            }
            if (!(baseWin32Exception.NativeErrorCode is 123456789))
            {
                throw new System.Exception("baseWin32Exception must have native error code 123456789.");
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
                throw new System.Exception("Invalid command due to unbalanced quotes.");
            }
            else
            {
                while (splitIndex < command.Length)
                {
                    if (command[splitIndex] == '"')
                    {
                        throw new System.Exception("Invalid command due to unexpected quote.");
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