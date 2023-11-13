using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Wire {
    public Node NodeA { get; set; }
    public Node NodeB { get; set; }
    public Color Color { get; set; }

    public bool Powered { get; private set; }

    public Wire(Node node_a, Node node_b, Color color) {
        NodeA = node_a;
        NodeB = node_b;
        Color = color;
    }
}