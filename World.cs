using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class World {
    public Vector3 Size { get; set; }

    public List<Block> Blocks { get; private set; }
    public List<Player> Players { get; private set; }

    public Vector3 SpawnPoint { get; private set; }

    public bool ShouldExit { get; private set; } = false;

    private readonly RNG RNG = new RNG();

    public World(float size, float height) {
        Size = new Vector3(size, height, size);

        SpawnPoint = new Vector3(Size.X / 2, 1.0f, Size.Z / 2);

        // Blocks
        Blocks = new List<Block>();
        for (int x = -(int)Size.X / 2; x < Size.X * 2; x++) {
            for (int z = -(int)Size.Z / 2; z < Size.Z * 2; z++) {
                var Block = new Block(new Vector3(x, 0, z), new Color(0, RNG.Range(150, 200), 0, 255));
                Blocks.Add(Block);
            }
        }

        // Players
        Players = new List<Player>();
    }

    public void AddPlayer() {
        var NewPlayer = new Player(this, SpawnPoint);
        Players.Add(NewPlayer);
    }

    public void Update() {
        // Blocks
        for (int i = Blocks.Count - 1; i >= 0; i--) {
            var Block = Blocks[i];
            if (Block.ShouldBeRemoved) {
                Blocks.Remove(Block);
            }
        }

        // Players
        foreach (var Player in Players) {
            Player.Update();
        }
    }

    public void Draw() {
        foreach (var Player in Players) {
            Player.Draw();
        }
    }

    public void Exit() {
        ShouldExit = true;
    }
}