using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Program {
    static void Main(string[] args) {
        // Window Setup
        InitWindow((int)Global.WindowSize.X, (int)Global.WindowSize.Y, "Wires3D");
        SetTargetFPS(60);

        SetExitKey(KeyboardKey.KEY_NULL);
        DisableCursor();

        // World
        var World = new World(64, 64);
        World.AddPlayer();

        // Main Loop
        while (!WindowShouldClose()) {
            //
            // Update
            World.Update();

            if (World.ShouldExit) {
                break;
            }

            //
            // Draw
            BeginDrawing();
            ClearBackground(Color.SKYBLUE);

            World.Draw();

            EndDrawing();
        }

        CloseWindow();
    }
}
