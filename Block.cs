using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Block {
    public Vector3 Position { get; set; }
    public Color Color { get; set; }

    public BoundingBox BoundingBox { get; private set; }

    public Block(Vector3 position, Color color) {
        Position = position;
        Color = color;

        BoundingBox = new BoundingBox(Position - new Vector3(0.5f, 0.5f, 0.5f), Position - new Vector3(0.5f, 0.5f, 0.5f) + Vector3.One);
    }

    public virtual void Draw() {
        DrawCubeV(Position, Vector3.One, Color);
    }
}