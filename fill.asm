// Runs an infinite loop. When any key is pressed, the screen turns black.
// So, we need to know how to increment memory addresses in ram once a key is pressed because the keyboard and screen are technically already in memory.

We have our A, D, and M chips
We have our d destination control bits
We have our j jump control bits

// First iteration of the code at 10;30 on a Saturday night

@1 // Setting the A register to 1
D = A

(LOOP)

// Read the value at M[24576], which I'm pretty sure get's stored in the A register

@M[24576] // Setting the incoming memory value to A, although I actually don't think this is possible with the hack architecture
D=A-1

JEQ

// Screen starts at 16384

@SCREEN

M=1


@LOOP
0;JMP

(END)

// Second iteration of instructions at 2 PM the following day while asking AI yes or no questions about my thought process and going through my diagram of the CPU.

(LOOP)
@24576
D=M
@LOOP
D;JEQ

// From here will will just make the screen black because we would not have jumped back to the top of the loop

(END)


// First iteration of the second loop in which we make the screen black

(LOOP)
@16384
M = -1


D = D + word length? // Maybe this is how the value of D get's updated so we know when to stop on the screen
M = -1 // We'll need to write 11111... to the word

(END)


// Second and final iteration
(LOOP)
    @KBD
    D=M
    @LOOP
    D;JEQ

    @SCREEN
    D=A
    @POSITION
    M=D

(INNER_LOOP)
    @POSITION
    A=M
    M=-1

    @POSITION
    D=M+1
    M=D

    @24576
    D=D-A
    @LOOP
    D;JGE

    @INNER_LOOP
    0;JMP




