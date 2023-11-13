using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Block {
    public Vector3 Position { get; set; }
    public Color Color { get; set; }

    public Model Model { get; set; }
    public Texture2D Texture { get; set; }
    public BoundingBox BoundingBox { get; private set; }

    public Block(Vector3 position, Color color) {
        Position = position;
        Color = color;

        Model = LoadModelFromMesh(GenMeshCube(1.0f, 1.0f, 1.0f));
        BoundingBox = new BoundingBox(Position - new Vector3(0.5f, 0.5f, 0.5f), Position - new Vector3(0.5f, 0.5f, 0.5f) + Vector3.One);
    }

    // Called when the player interacts with the block
    public virtual void OnInteract() {

    }

    // Called after the block is added to the world
    public virtual void OnCreate() {

    }

    // Called before the block is removed from the world
    public virtual void OnDestroy() {

    }

    public virtual void Draw() {
        Drawing.DrawCubeTexture(Texture, Position, 1.0f, 1.0f, 1.0f, Color.WHITE);
    }
}