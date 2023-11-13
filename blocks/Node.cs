using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Node : Block {
    public List<Node> ConnectedNodes { get; set; }

    public Node(Vector3 position) : base(position, Color.GRAY) {
        ConnectedNodes = new List<Node>();
    }

    public override void Draw() {
        base.Draw();
        // foreach (var Node in ConnectedNodes) {
        //     DrawCapsule(Position, Node.Position, 0.1f, 4, 4, Color.RED);
        // }
    }
}