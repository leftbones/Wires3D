using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;

namespace Wires3D;


class Player {
    public World World { get; private set; }
    public Vector3 Position { get; set; }

    public float FlySpeed { get; set; }                 = 0.01f;
    public float PickDistance { get; set; }             = 6.0f;

    public float CameraSensitivity { get; set; }        = 0.01f;

    private Camera3D Camera;

    private Vector3 Velocity;

    private Block? PickedBlock;
    private RayCollision CursorPicker = new();

    private int CursorAlpha = 0;
    private int AlphaDir = 1;

    public Player(World world, Vector3 position) {
        World = world;
        Position = position;

        // Camera Setup
        Camera = new() {
            position = Position,
            target = new Vector3(Position.X, Position.Y, Position.Z - 4.0f),
            up = new Vector3(0.0f, 1.0f, 0.0f),
            fovy = 80.0f,
            projection = CameraProjection.CAMERA_PERSPECTIVE
        };
    }

    public void Move(Vector3 direction) {
        Velocity += direction * FlySpeed;
    }

    public void Update() {
        // Camera
        Velocity = Vector3.Clamp(Velocity, new Vector3(-0.2f, -0.2f, -0.2f), new Vector3(0.2f, 0.2f, 0.2f));
        Velocity = Vector3.Lerp(Velocity, Vector3.Zero, FlySpeed * 10);

        Position += Velocity;
        Camera.position += Velocity;
        Camera.target += Velocity;

        // Input
        Input();

        // Cursor
        PickedBlock = null;
        CursorPicker.distance = Global.FloatMax;
        var CursorRay = GetMouseRay(Global.WindowSize / 2, Camera);

        foreach (var Block in World.Blocks) {
            var BlockHitInfo = GetRayCollisionBox(CursorRay, Block.BoundingBox);
            if (BlockHitInfo.hit && BlockHitInfo.distance < PickDistance && BlockHitInfo.distance < CursorPicker.distance) {
                CursorPicker = BlockHitInfo;
                PickedBlock = Block;
            }
        }
    }

    public void Draw() {
        BeginMode3D(Camera);

        // Blocks
        foreach (var Block in World.Blocks) {
            Block.Draw();
        }

        // Cursor
        CursorAlpha += 4 * AlphaDir;
        if (CursorAlpha == 0) { AlphaDir = 1; }
        if (CursorAlpha == 100) { AlphaDir = -1; }

        if (PickedBlock is not null) {
            DrawCube(PickedBlock.Position, 1.01f, 1.01f, 1.01f, new Color(255, 255, 255, CursorAlpha));
            DrawCubeWires(PickedBlock.Position, 1.01f, 1.01f, 1.01f, Color.WHITE);
        }

        EndMode3D();

        // Reticle
        DrawCircleLines((int)Global.WindowSize.X / 2, (int)Global.WindowSize.Y / 2, 4.0f, Color.WHITE);

        // HUD Text
        var CamPos = $"X: {(MathF.Round(Camera.position.X * 2) / 2).ToString("0.0")}, Y: {(MathF.Round(Camera.position.Y * 2) / 2).ToString("0.0")}, Z: {(MathF.Round(Camera.position.Z * 2) / 2).ToString("0.0")}";

        DrawText($"{CamPos}", 11, 11, 20, Color.BLACK);
        DrawText($"{CamPos}", 10, 10, 20, Color.WHITE);

    }

    public void Input() {
        // Exit
        if (IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
            if (IsCursorHidden()) {
                EnableCursor();
            } else {
                World.Exit();
            }
        }

        // Destroy Block / Focus Window
        if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) {
            if (IsCursorHidden()) {
                PickedBlock?.Destroy();
            } else {
                DisableCursor();
            }
        }

        // Move
        if (IsKeyDown(KeyboardKey.KEY_W)) { Move(new Vector3(0, 0, -1)); }
        if (IsKeyDown(KeyboardKey.KEY_S)) { Move(new Vector3(0, 0, 1)); }
        if (IsKeyDown(KeyboardKey.KEY_A)) { Move(new Vector3(-1, 0, 0)); }
        if (IsKeyDown(KeyboardKey.KEY_D)) { Move(new Vector3(1, 0, 0)); }

        // Ascend
        if (IsKeyDown(KeyboardKey.KEY_SPACE)) {
            Move(new Vector3(0, 1.0f, 0));
        }

        // Descend
        if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT)) {
            Move(new Vector3(0, -1.0f, 0));
        }
    }
}