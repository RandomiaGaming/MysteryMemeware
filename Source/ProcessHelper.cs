//#approve 08/05/2022 12:51pm
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
namespace MysteryMemeware
{
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
            ProcessStartInfo processStartInfo = new()
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
        public static Process Start(TerminalCommand command, WindowMode windowMode = WindowMode.Default, bool elevate = false, string workingDirectory = null)
        {
            if (command is null)
            {
                throw new Exception("command cannot be null.");
            }
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
}