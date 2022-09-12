using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using LifeSim;


// Initialization
//---------------------------------------------------------
const int screenWidth = 1000;
const int screenHeight = 1000;
const int borderSize = 0;

InitWindow(screenWidth, screenHeight, "Atom Simulation");

bool pause = false;
int framesCounter = 0;

SetTargetFPS(60);


Random rand = new Random();

List<List<Atom>> allAtoms = new List<List<Atom>>();

List<Atom> yellowAtoms = Atom.Create(200, 5, YELLOW);
List<Atom> redAtoms = Atom.Create(200, 5, RED);
List<Atom> greenAtoms = Atom.Create(200, 5, GREEN);
//redAtoms.Add(new Atom(screenHeight / 2, screenWidth / 2, 5, RED));

// A negative g means they attract eachother, and a positive g means they repell eachother
static void Interact(ref List<Atom> group1, ref List<Atom> group2, double g, double maxForce = 80, double velocityMultiplier = 0.5)
{
    foreach (var atom1 in group1)
    {
        double fx = 0;
        double fy = 0;
        double F = 0;

        foreach (var atom2 in group2)
        {
            int dx = atom1.X - atom2.X;
            int dy = atom1.Y - atom2.Y;
            double distance = Math.Sqrt(dx*dx + dy*dy);

            // Set mass to 1 (don't want to calculate when 2 atoms combine) and d^2 to
            F = (g * 1) / distance;
            
            if (distance > 0 && distance < 100)
            {
                fx += F * dx;
                fy += F * dy;
            }
        }        

        // Newtons second law
        // F = ma, since m = 1 => a = F, thus you must increase the velocity by the force
        atom1.VelX = (atom1.VelX + fx) * velocityMultiplier;
        atom1.VelY = (atom1.VelY + fy) * velocityMultiplier;

        // Round because sometimes velX and velY can be rounded to 0 making the atoms not move
        atom1.X += (int)Math.Round(atom1.VelX, MidpointRounding.AwayFromZero);
        atom1.Y += (int)Math.Round(atom1.VelY, MidpointRounding.AwayFromZero);

        //Reverse direction and set pos to border if they hit the wall
        if (atom1.X <= borderSize || atom1.X >= screenWidth + borderSize)
        {
            atom1.VelX *= -1;
            if (atom1.X <= borderSize)
            {
                atom1.X = borderSize;
            }
            else
            {
                atom1.X = screenWidth + borderSize - atom1.Size;
            }
        }
        if (atom1.Y <= borderSize || atom1.Y >= screenHeight + borderSize)
        {
            atom1.VelY *= -1;

            if (atom1.Y <= borderSize)
            {
                atom1.Y = borderSize;
            }
            else
            {
                atom1.Y = screenHeight + borderSize - atom1.Size;
            }
        }

        //Console.WriteLine($"Atom1: (x: {atom1.X}, y: {atom1.Y})");
        //Console.WriteLine($"Distance: {distance}, F: {F}, fx: {fx}, fy: {fy}, dx: {dx}, dy: {dy}, F * dx: {F * dx}");
    }
}

// Main game loop
while (!WindowShouldClose())  // Detect window close button or ESC key
{
    // Update
    if (IsKeyPressed(KEY_SPACE))
    {
        pause = !pause;
    }

    if (!pause)
    {
        Interact(ref redAtoms, ref redAtoms, -1);

        Interact(ref redAtoms, ref yellowAtoms, 0.3);
        Interact(ref yellowAtoms, ref redAtoms, 0.3);
    }
    else
    {
        framesCounter += 1;
    }

    // Draw
    BeginDrawing();
    ClearBackground(BLACK);

    // Update List after modified by Interact()
    allAtoms = new List<List<Atom>>();
    allAtoms.Add(yellowAtoms);
    allAtoms.Add(redAtoms);
    allAtoms.Add(greenAtoms);

    //DrawRectangle(borderSize, borderSize, screenWidth - borderSize*2, screenHeight - borderSize*2, BLACK);

    foreach (var atomList in allAtoms)
    {
        foreach (var atom in atomList)
        {
            DrawRectangle(atom.X, atom.Y, atom.Size, atom.Size, atom.Color);
        }
    }

    DrawText("PRESS SPACE to PAUSE MOVEMENT", 10, GetScreenHeight() - 25, 20, LIGHTGRAY);

    // On pause, draw a blinking message
    if (pause && ((framesCounter / 30) % 2) == 0)
    {
        DrawText("PAUSED", GetScreenWidth() - 135, 10, 30, GRAY);
    }
    DrawFPS(10, 10);

    EndDrawing();
    //-----------------------------------------------------
}

// De-Initialization
//---------------------------------------------------------
CloseWindow();        // Close window and OpenGL context
                      //----------------------------------------------------------