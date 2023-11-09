using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Block {
    public Vector3 Position { get; set; }
    public Color Color { get; set; }

    public Block(Vector3 position, Color color) {
        Position = position;
        Color = color;
    }

    public virtual void Draw() {
        DrawCubeV(Position, Vector3.One, Color);
    }
}