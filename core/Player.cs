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

    private RayCollision CursorPicker = new();

    public Block? PickedBlock { get; set; }
    public Node? PickedNode { get; set; }
    public bool HoldingWire { get; set; }

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
        // Blocks
        foreach (var Block in World.Blocks) {
            Block.Draw();
        }

        // Wires
        foreach (var Wire in World.Wires) {
            DrawCapsule(Wire.NodeA.Position, Wire.NodeB.Position, 0.1f, 1, 1, Wire.Color);
        }

        if (HoldingWire) {
            var EndPos = PickedBlock is Node ? PickedBlock.Position : Camera.Target;
            DrawCapsule(PickedNode!.Position, EndPos, 0.1f, 1, 1, new Color(255, 0, 0, CursorAlpha + 50));
        }

        // Cursor
        CursorAlpha += 4 * AlphaDir;
        if (CursorAlpha == 0) { AlphaDir = 1; }
        if (CursorAlpha == 100) { AlphaDir = -1; }

        if (PickedBlock is not null) {
            DrawCube(PickedBlock.Position, 1.01f, 1.01f, 1.01f, new Color(255, 255, 255, CursorAlpha));
            DrawCubeWires(PickedBlock.Position, 1.01f, 1.01f, 1.01f, Color.WHITE);
        }
    }
}