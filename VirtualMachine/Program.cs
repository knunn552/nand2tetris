// See https://aka.ms/new-console-template for more information
using VirtualMachine;

Console.WriteLine("Hello, World!");

string inputFile = @"C:\path\to\your\SimpleAdd.vm"; // Update this path
Parser parser = new Parser(inputFile);

while (parser.HasMoreCommands())
{
    parser.Advance();
    CommandType type = parser.GetCommandType();

    if (type == CommandType.C_ARITHMETIC)
    {
        Console.WriteLine($"Arithmetic Command: {parser.Arg1()}");
    }
    else if (type == CommandType.C_PUSH || type == CommandType.C_POP)
    {
        Console.WriteLine($"{type}: Segment = {parser.Arg1()}, Index = {parser.Arg2()}");
    }
}
