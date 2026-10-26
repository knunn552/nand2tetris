using HackAssembler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");

// I want to see exactly what's happening when I run this program as far as how the file is loaded, how to Parser and Program classes are loaded into memory during runtime and compile time.


string currentProgram = "Max";

// BELOW IS THE PARSER WITHOUT SYMBOLS - USED FOR THE ADD.ASM file in project 6.
//string? outputFile = $"C:\\Users\\knunn\\Documents\\Projects\\nand2tetris\\files\\06\\{currentProgram}.hack";
//string? inputFile = $"C:\\Users\\knunn\\Documents\\Projects\\nand2tetris\\files\\06\\{currentProgram}.asm";

//Parser parser = new Parser(inputFile); // We have the streamreader, now we need to use the methods within the parser and code classes to get that into a binary file
//using StreamWriter writer = new StreamWriter(outputFile);

//while (parser.hasMoreCommands())
//{
//    parser.advance();

//    if (string.IsNullOrEmpty(parser.CurrentInstruction))
//    {
//        continue;
//    }

//    parser.commandType(parser.CurrentInstruction);

//    if (parser.CommandType == "A_COMMAND")
//    {
//        string addressString = parser.symbol();
//        int address = int.Parse(addressString);

//        string binaryInstruction = Convert.ToString(address, 2).PadLeft(16, '0');
//        writer.WriteLine(binaryInstruction);
//    }
//    else 
//    {
//        string prefix = "111";
//        string compBits = Code.Comp(parser.comp());
//        string destBits = Code.Dest(parser.dest());
//        string jumpBits = Code.Jump(parser.jump());

//        string binaryInstruction = prefix + compBits + destBits + jumpBits;
//        writer.WriteLine(binaryInstruction);
//    }
//}

//Console.WriteLine("Assembly completed successfully!");




// BELOW IS THE PARSER WITH SYMBOLS - USED FOR THE ALL FILES BESIDES ADD.ASM in project 6.

string? inputFile = $"C:\\Users\\knunn\\Documents\\Projects\\nand2tetris\\files\\06\\{currentProgram}.asm";

// This variable is mainly used in the first pass of the assembler. It's used to give encountered labels a ROM instruction.
int currentRomInstructions = 0;
int nextVariableAddress = 16;

SymbolTable symbolTable = new SymbolTable();

// Moved the below two parser instantiations into the for loop below.
// Parser parser = new Parser(inputFile, 1); // We have the streamreader, now we need to use the methods within the parser and code classes to get that into a binary file
// Parser parser2 = new Parser(inputFile, 2); // Now we will iterate again only this time collect the A and C commands as those are the one's that matter
int loops = 2;
for(int i = 0; i < loops; i++)
{
    Parser parser = new Parser(inputFile, i + 1);
    parseTheAssemblyFile(parser, symbolTable);
}



// Need to make a method that uses to parser to were not rewriting logic for the while loop twice
void parseTheAssemblyFile(Parser parser, SymbolTable symbolTable)
{
    
    string? outputFile = $"C:\\Users\\knunn\\Documents\\Projects\\nand2tetris\\files\\06\\{currentProgram}.hack";
    StreamWriter? writer = null;

    if(parser.loopCount == 2)
    {
        // I want to NOW give outputFile a value and start the streamWriter globally within the scrope of this method
        // We don't have the use a using keyword here because it messes with variable scope - we can just call writer.Dispose at the end of the while loop
        writer = new StreamWriter(outputFile);
    }
    
    

    while (parser.hasMoreCommands())
    {
        parser.advance();

        if (string.IsNullOrEmpty(parser.CurrentInstruction))
        {
            continue;
        }

        parser.commandType(parser.CurrentInstruction);

        if (parser.CommandType == "A_COMMAND")
        {
            if (parser.loopCount == 1)
            {
                currentRomInstructions++;
                continue;
            }

            // Get the clean text string token from the parser
            string addressString = parser.symbol();
            int address;

            // 1. Check if the string exists in the Symbol Table (Pre-defined, Labels, or existing variables)
            if (symbolTable.Contains(addressString))
            {
                address = symbolTable.GetAddress(addressString);
            }
            // 2. See if it's a raw numeric constant (checks ASCII/int parsing automatically)
            else if (int.TryParse(addressString, out int numericAddress))
            {
                address = numericAddress;
            }
            // 3. If it's not a known symbol and not a number, it's a brand-new variable!
            else
            {
                address = nextVariableAddress;
                symbolTable.AddEntry(addressString, address);
                nextVariableAddress++; // Move slot marker up for the next unique variable
            }

            string binaryInstruction = Convert.ToString(address, 2).PadLeft(16, '0');
            writer.WriteLine(binaryInstruction);
        }

        else if (parser.CommandType == "L_COMMAND" && parser.loopCount == 1)
        {
            // Now, we are working with labels. The way we know were working with a label (or symbol - correct my terminology if needed) is by the "(" character. 

            // Don't we just ignore this shit if it's an L_COMMAND? Yes, we do, on the second iteration of the loop, so we need to keep track of the iteration count with an integer variable called "loopCount". I'll add this in the Parser.cs file.
            // So, since we will not ignore L_COMMAND on the first iteration, we will need to add the label (LOOP, END, etc) to the symbol table, making it a symbol.

            // Now we need to put the current label in the symbol table
            string currentLabel = parser.symbol();
            symbolTable.AddEntry(currentLabel, currentRomInstructions);

        }
        else if(parser.CommandType == "L_COMMAND" && parser.loopCount == 2)
        {
            continue;
        }
        else
        {
            if (parser.loopCount == 1)
            {
                currentRomInstructions++;
                continue;
            }
            string prefix = "111";
            string compBits = Code.Comp(parser.comp());
            string destBits = Code.Dest(parser.dest());
            string jumpBits = Code.Jump(parser.jump());

            string binaryInstruction = prefix + compBits + destBits + jumpBits;
            writer.WriteLine(binaryInstruction);
        }
    }

    if(writer != null)
    {
        writer.Close();
    }
}

Console.WriteLine("Assembly completed successfully!");
