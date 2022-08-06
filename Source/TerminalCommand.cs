//#approve 08/05/2022 12:51pm
using System;
namespace MysteryMemeware
{
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
}