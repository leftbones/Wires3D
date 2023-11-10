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

        var ReticlePos = Global.WindowSize / 2;

        // World
        var RNG = new RNG();

        var Blocks = new List<Block>();
        var StartPos = new Vector3(-16, 0, -16);
        for (int x = 0; x < 32; x++) {
            for (int z = 0; z < 32; z++) {
                var B = new Block(new Vector3(StartPos.X + x, StartPos.Y + RNG.Range(0, 0), StartPos.Z + z), new Color(0, RNG.Range(150, 200), 0, 255));
                Blocks.Add(B);
            }
        }

        // Cursor
        int CursorAlpha = 0;
        int AlphaDir = 1;

        Block? PickedBlock;
        RayCollision CursorPicker = new();
        float PickDistance = 6;

        // Main Loop
        while (!WindowShouldClose()) {
            //
            // Update
            UpdateCamera(ref Camera, CameraMode.CAMERA_FIRST_PERSON);

            if (IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
                if (IsCursorHidden()) {
                    EnableCursor();
                } else {
                    break;
                }
            }

            if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) {
                if (IsCursorHidden()) {
                    // Break block
                } else {
                    DisableCursor();
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

            // Cursor
            CursorAlpha += 4 * AlphaDir;
            if (CursorAlpha == 0) { AlphaDir = 1; }
            if (CursorAlpha == 100) { AlphaDir = -1; }

            PickedBlock = null;
            CursorPicker.distance = Global.FloatMax;
            var CursorRay = GetMouseRay(ReticlePos, Camera);

            foreach (var Block in Blocks) {
                var BlockHitInfo = GetRayCollisionBox(CursorRay, Block.BoundingBox);
                if (BlockHitInfo.hit && BlockHitInfo.distance < PickDistance && BlockHitInfo.distance < CursorPicker.distance) {
                    CursorPicker = BlockHitInfo;
                    PickedBlock = Block;
                }
            }

            //
            // Draw
            BeginDrawing();
            ClearBackground(Color.SKYBLUE);

            BeginMode3D(Camera);

            // Ground
            foreach (var Block in Blocks) {
                Block.Draw();
            }

            // Cursor
            if (PickedBlock is not null) {
                DrawCube(PickedBlock.Position, 1.01f, 1.01f, 1.01f, new Color(255, 255, 255, CursorAlpha));
                DrawCubeWires(PickedBlock.Position, 1.01f, 1.01f, 1.01f, Color.WHITE);
            }

            EndMode3D();

            // Reticle
            DrawCircleLines((int)ReticlePos.X, (int)ReticlePos.Y, 4.0f, Color.WHITE);

            // HUD Text
            var CamPos = $"X: {(MathF.Round(Camera.position.X * 2) / 2).ToString("0.0")}, Y: {(MathF.Round(Camera.position.Y * 2) / 2).ToString("0.0")}, Z: {(MathF.Round(Camera.position.Z * 2) / 2).ToString("0.0")}";

            DrawText($"{CamPos}", 11, 11, 20, Color.BLACK);
            DrawText($"{CamPos}", 10, 10, 20, Color.WHITE);

            EndDrawing();
        }

        CloseWindow();
    }
}
