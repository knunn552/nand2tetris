using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    internal class SymbolTable
    {
        private readonly Dictionary<string, int> symbolTable;
        public SymbolTable() 
        {
            symbolTable = new Dictionary<string, int>()
            {
                { "SP", 0},
                { "LCL", 1},
                { "ARG", 2},
                { "THIS", 3},
                { "THAT", 4},
                { "SCREEN", 16384 },
                { "KBD", 24576}
            };

            for (int i = 0; i <= 15; i++)
            {
                symbolTable.Add($"R{i}", i);
            }
        }

        public void AddEntry(string symbol, int address)
        {
            symbolTable.Add(symbol, address);
        }

        public bool Contains(string symbol)
        {
            return symbolTable.ContainsKey(symbol);
        }

        public int GetAddress(string symbol)
        {
            return symbolTable[symbol];
        }
    }
}
