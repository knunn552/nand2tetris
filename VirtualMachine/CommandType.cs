using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine
{
    public enum CommandType
    {
        C_ARITHMETIC,
        C_PUSH,
        C_POP,
        C_LABEL,  // Chapter 8
        C_GOTO, // Chapter 8
        C_IF, // Chapter 8
        C_FUNCTION, // Chapter 8
        C_RETURN, // Chapter 8,
        C_CALL, // Chapter 8
    }
}
