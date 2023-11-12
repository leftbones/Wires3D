using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;

namespace Wires3D;


class Player {
    public World World { get; private set; }
    public Vector3 Position { get; set; }
    public Camera Camera { get; private set; }

    public float FlySpeed { get; set; }                 = 2.0f;
    public float PickDistance { get; set; }             = 6.0f;

    private Vector3 Velocity;
    private float Acceleration = 0.02f;
    private float Friction = 0.1f;

    private Block? PickedBlock;
    private RayCollision CursorPicker = new();

    private int CursorAlpha = 0;
    private int AlphaDir = 1;

    public Player(World world, Vector3 position) {
        World = world;
        Position = position;
        Camera = new Camera(this);
    }

    public void Move(Vector3 dir) {
        var Rotated = Vector3Transform(new Vector3(-dir.X, dir.Y, -dir.Z), MatrixRotateZYX(new Vector3(0, Camera.ViewAngle.X, 0)));
        Velocity += Rotated * Acceleration;
    }

    public void Update() {
        // Camera
        Camera.Update();

        Velocity = Vector3.Clamp(Velocity, new Vector3(-FlySpeed, -FlySpeed, -FlySpeed), new Vector3(FlySpeed, FlySpeed, FlySpeed));
        Velocity = Vector3.Lerp(Velocity, Vector3.Zero, Friction);

        Position += Velocity;

        // Input
        Input();

        // Cursor
        PickedBlock = null;
        CursorPicker.distance = Global.FloatMax;
        var CursorRay = Camera.GetRay();

        foreach (var Block in World.Blocks) {
            var BlockHitInfo = GetRayCollisionBox(CursorRay, Block.BoundingBox);
            if (BlockHitInfo.hit && BlockHitInfo.distance < PickDistance && BlockHitInfo.distance < CursorPicker.distance) {
                CursorPicker = BlockHitInfo;
                PickedBlock = Block;
            }
        }
    }

    public void Draw() {
        Camera.BeginDraw();

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

        Camera.EndDraw();

        // Reticle
        DrawCircleLines((int)Global.WindowSize.X / 2, (int)Global.WindowSize.Y / 2, 4.0f, Color.WHITE);

        // HUD Text
        var CamPos = $"X: {(MathF.Round(Camera.Position.X * 2) / 2).ToString("0.0")}, Y: {(MathF.Round(Camera.Position.Y * 2) / 2).ToString("0.0")}, Z: {(MathF.Round(Camera.Position.Z * 2) / 2).ToString("0.0")}";

        DrawText($"{CamPos}", 11, 11, 20, Color.BLACK);
        DrawText($"{CamPos}", 10, 10, 20, Color.WHITE);

    }

    public void Input() {
        // Exit
        if (IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
            if (IsCursorHidden()) {
                EnableCursor();
                Camera.MouseLookEnabled = false;
            } else {
                World.Exit();
            }
        }

        // Place Block
        if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT)) {
            if (IsCursorHidden()) {
                if (PickedBlock is not null) {
                    var Block = new Block(PickedBlock.Position + new Vector3(0, 1, 0), new Color(World.RNG.Range(0, 255), World.RNG.Range(0, 255), World.RNG.Range(0, 255), 255));
                    World.SetBlock(Block);
                }
            }
        }

        // Destroy Block / Focus Window
        if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT)) {
            if (IsCursorHidden()) {
                PickedBlock?.Destroy();
            } else {
                DisableCursor();
                Camera.MouseLookEnabled = true;
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