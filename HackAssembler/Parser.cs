using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    public class Parser
    {

        // Need to go get the asm file sitting somewhere on disk
        // 

        // These files are sitting on disk, which have an read only address that corresponds to the start of that file
        // Well, in modern computers, this actually isn't the case. The windows operating system correlates the address where the file sits with NFTS file system and then loads the file into RAM where I can interact with it


        // Create a stream that takes in a path and reads the .asm and puts the conents of that file into memory where the stream variable is going to be the actually memory location where the file contents are stored on memory



        // Routines/Modules

        // Need a constructor/initializer with arguments for the input file/stream. This opens the input file/stream and gets ready to parse it.
        // Need a method called 


        // Need class members that hold state

        public string? CurrentInstruction { get; private set; } = null;
        public string? CommandType { get; private set; } = null;
        private StreamReader _stream;
        public int loopCount { get; private set; }

        // Need constructor to initialize the class
        public Parser(string filePath, int loopCount)
        {
            _stream = new StreamReader(filePath);
            this.loopCount = loopCount;
        }



        public bool hasMoreCommands() // Are there more commands in the input?
        {

            // Get the stream reader in memory's starting address
            // Loop through it until we either find a /n orrrr what? How do we know numerically where the end of a file is?
            // Actually, the way this is modernly handled is that the OS gives keeps track of the byte size of the file so the StreamReader object has the ability to know exactly where the file ends
            // However, C# has a sexy method called Peak which we can essentialy use to see if there's a new line
            return _stream.Peek() != -1;
        }

            
        // Should be called only if hasMoreCommands() is true. Initiaally there will be no commands
        // Reads the next command from tne input and stores it in the current command variable in this class. Before storing, it must be cleaned.
        public void advance()
        {
            // First attempt
            // We use Peek again to actual get the line? No, we don't.
            // We can use _stream.Readline. Under the hood, this is actually a method that is looping byte by byte untl it hits a /n newline character which is ASCII 10 value
            //// CurrentInstruction = _stream.ReadLine();
            //// CurrentInstruction = CleanLine(CurrentInstruction);
            // Now that we have the actual line, we need to abide by Hack Assembly specs and ignore this line if it's a comment
            // If not a comment, clear the white space

            // Second attempt
            // First we need to get the rawline
            string? rawLine;
            string cleanedLine = "";

            // Keep reading lines until we find one that has actual instruction code
            while (hasMoreCommands())
            {
                rawLine = _stream.ReadLine();
                cleanedLine = CleanLine(rawLine);

                // If we found a legitimate instruction, break out of the loop
                if (!string.IsNullOrEmpty(cleanedLine))
                {
                    break;
                }
            }

            CurrentInstruction = cleanedLine;
        }

        public string CleanLine(string? line)
        {
            if (line == null) return "";

            int commentIndex = line.IndexOf("//");

            if (commentIndex != -1)
            {
                line = line.Substring(0, commentIndex);
            }

            // Remove all whitespace, tabs, and carriage returns
            return line.Replace(" ", "").Replace("\t", "").Replace("\r", "");
        }

        public void commandType(string command)
        {
            // A_COMMAND , C_COMMAND , L_COMMAND
            // A_COMMAND for @Xxx where Xxx is either a symbol or decimal, representing an assigned variable? or memory location
            // C_COMMAND for dest=comp;jump, represeting computation commands
            // L_COMMAND for (actually pseudo command) where @Xxx is a symbol, these are like (LOOP) and (END)

            if (command.StartsWith("@"))
            {
                CommandType = "A_COMMAND";
            }
            else if (command.StartsWith("("))
            {
                CommandType = "L_COMMAND";
            }
            else
            {
                CommandType = "C_COMMAND";
            }
            return;
        }






        // This method should only be called when commandType is an A_COMMAND or L_COMMAND
        // This returns that symbol or decimal of the current command
        public string symbol()
        {
            if (string.IsNullOrEmpty(CurrentInstruction)) return "";

            
            if (CurrentInstruction.StartsWith("@"))
            {
                return CurrentInstruction[1..];
            }
            if (CurrentInstruction.StartsWith("(") && CurrentInstruction.EndsWith(")"))
            {
                return CurrentInstruction[1..^1];
            }

            return CurrentInstruction;
        }


        public string dest()
        {
            // As described in the course, the purpose of this method confused me a little bit, but all were doing is string manipulation and getting the destination part of the assembly instruction
            int characterOfInterestLocation = CurrentInstruction.IndexOf("=");

            if (characterOfInterestLocation != -1)
            {
                return CurrentInstruction.Substring(0, characterOfInterestLocation);
            }
            return "";

        }

        public string jump()
        {
            // Jump, like dest, is an easy function as we know all jump commands come after the semi colon 
            int semiColonIndex = CurrentInstruction.IndexOf(';');
            if (semiColonIndex != -1)
            {
                return CurrentInstruction.Substring(semiColonIndex + 1);
            }
            return "";
        }

        // Before even starting, am guessing this is going to be a little tougher than the previous two
        public string comp()
        {
            // There are 3 different possibilites for computation instructions in Hack assembly language:
            // Something like D = M + 1. In this case, only destination, no jump.
            // Something like 0;JMP. In this case, only a jump, no math.
            // Then, something like D = M + 1; JGT. In this case, a complete instruction with dest and jump

            // Look for ; and =
            int equalsIndex = CurrentInstruction.IndexOf("=");
            int semiColonIndex = CurrentInstruction.IndexOf(";");

            int start;
            int end;

            start = (equalsIndex != -1) ? equalsIndex + 1 : 0;
            end = (semiColonIndex != -1) ? semiColonIndex : CurrentInstruction.Length;

            return CurrentInstruction.Substring(start, end - start);
        }

       



    }
}
