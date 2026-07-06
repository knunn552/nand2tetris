// =========================================================================
// PROGRAM: Mult.asm (Project 4)
// AUTHOR: You (The Systems Thinker)
// LOGIC: Multiplies R0 and R1, storing the final product in R2.
// =========================================================================

// -------------------------------------------------------------------------
// [MY PERSONAL LOG: WHAT I DIDN'T UNDERSTAND PRIOR TO CODING IT]
// 1. Hardware Limits: I thought we were trapped using only A, D, and M. I didn't 
//    fully realize yet that RAM acts as a massive array of extra registers.
// 2. The @ Myth: I didn't initially realize that '@something' is strictly an 
//    A-instruction whose only job is to change the A register itself.
// 3. Side Effects: I initially wanted to just decrement R1 directly to save memory. 
//    I had to learn that destroying input data is dangerous 'destructive design'.
// -------------------------------------------------------------------------

// -------------------------------------------------------------------------
// [MY COGNITIVE TRAIN OF THOUGHT: STATE INITIALIZATION]
// -------------------------------------------------------------------------

    // Step 1: Initialize the Sum. 
    // I know @R2 targets RAM[2]. I map M=0 to wipe any leftover test data.
    @R2 
    M=0 

    // Step 2: Safely duplicate R1 into a dedicated countdown variable.
    // I learned that I must hop through the D register to transfer memory.
    // I load R1's address into A, pull its data into D, create a nickname 
    // called '@counter' (which hooks a unique address), and write D into it.
    @R1 
    D=M 
    @counter 
    M=D 

// -------------------------------------------------------------------------
// [THE CORE ENGINE: LOOP LOGIC & HARDWARE STRATEGY]
// -------------------------------------------------------------------------

(LOOP)
    // Step 3: Check if we are done.
    // I load the counter value back into D.
    @counter 
    D=M 

    // Step 4: Execute the Zero-Check.
    // I learned that Hack hardware relies on a physical status flag (zr) inside 
    // the ALU. 'D;JEQ' asks if D == 0. If true, the PC registers a jump to (END).
    @END 
    D;JEQ 

    // Step 5: Process the Multiplicand.
    // I realized D is completely free to be overwritten here! I fetch R0.
    @R0 
    D=M 

    // Step 6: Overcome the ALU Bottleneck.
    // ARCHITECTURAL AH-HA! My gut caught that 'M=M+A' or 'M=D+A' were illegal. 
    // The ALU is wired with D on one side and M/A on the other. 
    // 'M=M+D' (or my legal 'M=D+M' discovery) successfully uses the open M window.
    @R2 
    M=M+D 

    // Step 7: Decrement the loop countdown.
    @counter 
    M=M-1 

    // Step 8: Force the unconditional loop back.
    @LOOP 
    0;JMP 

// -------------------------------------------------------------------------
// [THE SAFETIES: INFINITE TRAP]
// -------------------------------------------------------------------------

(END)
    // To prevent the CPU program counter from bleeding into empty space.
    @END 
    0;JMP 

// =========================================================================
// [THE KNOWLEDGE SYSTEM: WHAT I SUCCESSFULLY LEARNED]
// =========================================================================
// * Instruction Formats: I mastered that MSB=0 denotes an A-instruction and 
//   MSB=1 denotes a C-instruction (with its comp, dest, and jump bit segments).
// * ALU Topography: I discovered why we must pass numbers through D because the 
//   ALU cannot simultaneously read from and write to two non-D channels.
// * Jumps: I learned that hardware logic optimization hinges on zero-comparisons.
// =========================================================================

// =========================================================================
// [THE PARKING LOT: WHAT I STILL DON'T FULLY UNDERSTAND RIGHT NOW]
// =========================================================================
// 1. How the hardware physically links the custom label (LOOP) or (END) to a 
//    specific ROM address instruction line when calculating the jump.
// 2. How the Assembler dynamically grabs a random, safe RAM location (like 16+) 
//    the very first time it reads my custom variable name '@counter'.
// -> RESOLUTION: Chapter 6 (The Assembler) will explain this translation layer!
// =========================================================================
