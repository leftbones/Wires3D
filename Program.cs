using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Program {
    static void Main(string[] args) {
        // Window Setup
        SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
        InitWindow((int)Global.WindowSize.X, (int)Global.WindowSize.Y, "Wires3D");
        SetTargetFPS(60);

        SetExitKey(KeyboardKey.KEY_NULL);
        DisableCursor();

        // Server Setup
        var Server = new Server();


        //
        // Main Loop
        while (!WindowShouldClose()) {
            //
            // Update
            Server.Update();

            if (Server.ShouldExit) {
                break;
            }

            //
            // Draw
            BeginDrawing();
            ClearBackground(Color.SKYBLUE);

            Server.Draw();

            EndDrawing();
        }

        CloseWindow();
    }
}
