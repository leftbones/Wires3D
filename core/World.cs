using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class World {
    public Vector3 Size { get; set; }

    public List<Block> Blocks { get; private set; }
    public List<Wire> Wires { get; private set; }
    public List<Player> Players { get; private set; }

    public Vector3 SpawnPoint { get; private set; }

    public bool ShouldExit { get; private set; } = false;

    public readonly RNG RNG = new RNG();

    private Model Model;

    public unsafe World(float size, float height) {
        Size = new Vector3(size, height, size);

        SpawnPoint = new Vector3(0, 4, 0);

        // Floor Plane
        Model = LoadModelFromMesh(GenMeshPlane(Size.X, Size.Z, 1, 1));
        SetMaterialTexture(ref Model.materials[0], MaterialMapIndex.MATERIAL_MAP_DIFFUSE, LoadTexture("assets/textures/floor.png"));

        // Blocks
        Blocks = new List<Block>();
        for (int x = -(int)Size.X / 2; x < Size.X * 2; x++) {
            for (int z = -(int)Size.Z / 2; z < Size.Z * 2; z++) {
                var FloorBlock = new Floor(new Vector3(x, 0, z));
                Blocks.Add(FloorBlock);
            }
        }

        // Wires
        Wires = new List<Wire>();

        // Players
        Players = new List<Player>();
    }

    // Add a block to the world
    public void SetBlock(Block block) {
        Blocks.Add(block);
        block.OnCreate();
    }

    public void Tick() {

    }

    public void Update() {

    }

    public void Draw() {
        var Center = new Vector3(SpawnPoint.X - 0.5f, 0.5f, SpawnPoint.Z - 0.5f);
        DrawModel(Model, Center, 1.0f, Color.WHITE);
    }
}