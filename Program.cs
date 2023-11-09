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

        // Camera Setup
        Camera3D Camera = new() {
            position = new Vector3(0.0f, 8.0f, 0.0f),
            target = new Vector3(0.0f, 0.0f, -4.0f),
            up = new Vector3(0.0f, 1.0f, 0.0f),
            fovy = 80.0f,
            projection = CameraProjection.CAMERA_PERSPECTIVE
        };

        // World
        var RNG = new RNG();

        var Blocks = new List<Block>();
        var StartPos = new Vector3(-16, 0, -16);
        for (int x = 0; x < 32; x++) {
            for (int z = 0; z < 32; z++) {
                var B = new Block(new Vector3(StartPos.X + x, StartPos.Y, StartPos.Z + z), new Color(0, RNG.Range(150, 200), 0, 255));
                Blocks.Add(B);
            }
        }

        // Other
        int CursorAlpha = 0;
        int AlphaDir = 1;

        // Main Loop
        while (!WindowShouldClose()) {
            // Update
            UpdateCamera(ref Camera, CameraMode.CAMERA_FIRST_PERSON);

            if (IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                if (IsCursorHidden()) {
                    EnableCursor();
                } else {
                    break;
                }
            }

            if (IsKeyDown(KeyboardKey.KEY_SPACE)) {
                Camera.position.Y += 0.1f;
                Camera.target.Y += 0.1f;
            }

            if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT)) {
                Camera.position.Y -= 0.1f;
                Camera.target.Y -= 0.1f;
            }

            CursorAlpha += 4 * AlphaDir;
            if (CursorAlpha == 0) { AlphaDir = 1; }
            if (CursorAlpha == 100) { AlphaDir = -1; }

            // Draw
            BeginDrawing();
            ClearBackground(Color.SKYBLUE);

            BeginMode3D(Camera);

            // Ground
            foreach (var Block in Blocks) {
                Block.Draw();
            }

            // Cursor
            var CursorPos = new Vector3(
                MathF.Ceiling(Camera.target.X - 0.5f),
                MathF.Ceiling(Camera.target.Y - 0.5f),
                MathF.Ceiling(Camera.target.Z - 0.5f)
            );
            DrawCube(CursorPos, 1.01f, 1.01f, 1.01f, new Color(255, 255, 255, CursorAlpha));
            DrawCubeWires(CursorPos, 1.01f, 1.01f, 1.01f, Color.WHITE);

            EndMode3D();

            var CamPos = $"X: {(MathF.Round(Camera.position.X * 2) / 2).ToString("0.0")}, Y: {(MathF.Round(Camera.position.Y * 2) / 2).ToString("0.0")}, Z: {(MathF.Round(Camera.position.Z * 2) / 2).ToString("0.0")}";

            DrawText($"{CamPos}", 11, 11, 20, Color.BLACK);
            DrawText($"{CamPos}", 10, 10, 20, Color.WHITE);

            EndDrawing();
        }

        CloseWindow();
    }
}
