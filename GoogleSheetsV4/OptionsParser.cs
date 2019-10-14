using System;
using System.Collections.Generic;
using System.Linq;
using GoogleSheetsV4.Models;

namespace GoogleSheetsV4
{
    /// <summary>
    /// Parser for command line arguments and parameters.
    /// </summary>
    class OptionsParser
    {
        private static readonly string[] Commands = { "-s", "-ci", "-cs" };

        private string _parsedCommand;

        /// <summary>
        /// Parse all command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns></returns>
        public ParsedOptions Parse(string[] args)
        {
            var values = new Dictionary<string, List<string>>();

            foreach (var arg in args)
            {
                if (_parsedCommand == null)
                    ParseCommand(arg);
                else
                {
                    var parameter = ParseParameter(arg);

                    if (!values.ContainsKey(_parsedCommand) || values[_parsedCommand] == null)
                        values[_parsedCommand] = new List<string>();
                    values[_parsedCommand].Add(parameter);
                }
            }

            return new ParsedOptions(values["-s"].FirstOrDefault(),
                values[".ci"].FirstOrDefault(),
                values["-cs"].FirstOrDefault());
        }

        #region Private methods

        /// <summary>
        /// Parse a command argument.
        /// </summary>
        /// <param name="arg">Argument to parse.</param>
        private void ParseCommand(string arg)
        {
            EnsureArgumentCommand(arg);

            _parsedCommand = arg;
        }

        /// <summary>
        /// Parse a command parameter.
        /// </summary>
        /// <param name="arg">Argument to parse.</param>
        /// <returns>The parsed command parameter.</returns>
        private string ParseParameter(string arg)
        {
            switch (_parsedCommand)
            {
                case "-s":
                    EnsureArgumentNoCommand(arg);
                    _parsedCommand = null;
                    return arg;
                case "-ci":
                    EnsureArgumentNoCommand(arg);
                    _parsedCommand = null;
                    return arg;
                case "-cs":
                    EnsureArgumentNoCommand(arg);
                    _parsedCommand = null;
                    return arg;
                default:
                    throw new InvalidOperationException($"Unknown option '{_parsedCommand}'.");
            }
        }

        /// <summary>
        /// Ensures that the argument is no known command.
        /// </summary>
        /// <param name="arg">The value to ensure.</param>
        private void EnsureArgumentNoCommand(string arg)
        {
            if (arg == null || Commands.Contains(arg))
                throw new InvalidOperationException($"'{arg}' is no valid parameter.");
        }

        /// <summary>
        /// Ensures that the argument is a known command.
        /// </summary>
        /// <param name="arg">The value to ensure.</param>
        private void EnsureArgumentCommand(string arg)
        {
            if (arg == null || !Commands.Contains(arg))
                throw new InvalidOperationException($"'{arg}' is no valid command.");
        }

        #endregion
    }
}
