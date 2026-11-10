using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    internal class Parser
    {
        private readonly string[] _lines; // This will hold all of the lines in the .vm file which is actually represpentative of a c# .ilasm file
        private int _currentLineIndex = -1; // We set at -1 to start so the program thinks there's another command to go fetch from the very start. 
        private string[] _currentCommandParts = Array.Empty<string>(); // This is the staging area for breaking down current command instructions (push local 2) would become ["push", "local", "2"]


        public Parser(string filePath)
        {
            _lines = File.ReadLines(filePath) // The constructor of the Parser class will strip out any comment or white space from each line in the .vm file
                .Select(line => line.Split("//")[0].Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
        }

        public bool HasMoreCommands()
        {
            return _currentLineIndex + 1 < _lines.Length;
        }

        public void Advance() // Not only do we increment the currentLineIndex here, but we also get the individual command parts within the line and store that in the currentCommandParts string array

        {
            _currentLineIndex++;

            //  We are simply spliting the lines where there are spaces are tabs which indicate spaces
            _currentCommandParts = _lines[_currentLineIndex].Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries)
        }


        public CommandType GetCommandType()
        { 
            // This method is getting the command type, which will be either an addition, push or pop
            string baseCommand = _currentCommandParts[0];

            // List of all arithmetic operations in chapter 7
            string[] arithmeticCommands = { "add", "sub", "neg", "eq", "gt", "lt", "and", "or", "not" };

            if (arithmeticCommands.Contains(baseCommand))
            {
                return CommandType.C_ARITHMETIC;
            }

            return baseCommand switch
            {
                "push" => CommandType.C_PUSH,
                "pop" => CommandType.C_POP,
                _ => throw new InvalidOperationException($"Unknown command type")
            };

        }


        // Now, we take into account the two arguments in the command
        public string Arg1()
        {
            // If it's arithmetic, the command itself (like "add") is treated as Arg1
            if(GetCommandType() == CommandType.C_ARITHMETIC)
            {
                return _currentCommandParts[0];
            }

            return _currentCommandParts[1];
        }

        public int Arg2() {
            return int.Parse( _currentCommandParts[2]);
        }

    }
}
