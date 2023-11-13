using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Client {
    public Server Server { get; private set; }
    public World World { get { return Server.World; } }

    public Player Player { get; private set; }

    public Dictionary<string, Texture2D> TextureCache { get; private set; }

    public Client(Server server) {
        Server = server;

        TextureCache = new Dictionary<string, Texture2D>();

        Player = new Player(World, World.SpawnPoint);
        World.Players.Add(Player);
    }

    public void GetInput() {
        // Exit
        if (IsKeyPressed(KeyboardKey.KEY_ESCAPE)) {
            if (IsCursorHidden()) {
                EnableCursor();
                Player.Camera.MouseLookEnabled = false;
            } else {
                Server.ShouldExit = true;
            }
        }

        // Place Block
        if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT)) {
            if (IsCursorHidden()) {
                if (Player.PickedBlock is not null) {
                    if (Player.PickedBlock is Node) {
                        if (Player.HoldingWire) {
                            var NewNode = Player.PickedBlock as Node;

                            if (NewNode != Player.PickedNode) {
                                if (!NewNode!.ConnectedNodes.Contains(Player.PickedNode!)) {
                                    NewNode!.ConnectedNodes.Add(Player.PickedNode!);
                                    Player.PickedNode!.ConnectedNodes.Add(NewNode!);
                                    World.Wires.Add(new Wire(Player.PickedNode!, NewNode!, Color.RED));
                                }
                            }

                            Player.HoldingWire = false;
                            Player.PickedNode = null;
                        } else {
                            Player.PickedNode = Player.PickedBlock as Node;
                            Player.HoldingWire = true;
                        }
                    } else {
                        var Block = new Node(Player.PickedBlock.Position + new Vector3(0, 1, 0));
                        Block.Texture = TextureCache["node"];
                        World.SetBlock(Block);
                    }
                } else {
                    if (Player.HoldingWire) {
                        Player.HoldingWire = false;
                        Player.PickedNode = null;
                    }
                }
            }
        }

        // Destroy Block / Focus Window
        if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT)) {
            if (IsCursorHidden()) {
                // Interact/destroy block
            } else {
                DisableCursor();
                Player.Camera.MouseLookEnabled = true;
            }
        }

        // Move
        if (IsKeyDown(KeyboardKey.KEY_W)) { Player.Move(new Vector3(0, 0, -1)); }
        if (IsKeyDown(KeyboardKey.KEY_S)) { Player.Move(new Vector3(0, 0, 1)); }
        if (IsKeyDown(KeyboardKey.KEY_A)) { Player.Move(new Vector3(-1, 0, 0)); }
        if (IsKeyDown(KeyboardKey.KEY_D)) { Player.Move(new Vector3(1, 0, 0)); }

        // Ascend
        if (IsKeyDown(KeyboardKey.KEY_SPACE)) {
            Player.Move(new Vector3(0, 1.0f, 0));
        }

        // Descend
        if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT)) {
            Player.Move(new Vector3(0, -1.0f, 0));
        }
    }

    public void Tick() {
        Player.World.Tick();
    }

    public void Update() {
        Player.Update();
    }

    public void Draw() {
        // Player + World
        Player.Camera.BeginDraw();
        Player.World.Draw();
        Player.Draw();
        Player.Camera.EndDraw();

        // Reticle
        DrawCircleLines((int)Global.WindowSize.X / 2, (int)Global.WindowSize.Y / 2, 4.0f, Color.WHITE);

        // HUD Text
        var ClientFPS = $"FPS: {GetFPS()}";
        var FPSPos= (int)Global.WindowSize.X - MeasureText(ClientFPS, 20) - 10;
        Drawing.DrawTextShadow(ClientFPS, FPSPos, 10, 20);


        var CameraPos = $"X: {(MathF.Round(Player.Position.X * 2) / 2).ToString("0.0")}, Y: {(MathF.Round(Player.Position.Y * 2) / 2).ToString("0.0")}, Z: {(MathF.Round(Player.Position.Z * 2) / 2).ToString("0.0")}";
        Drawing.DrawTextShadow(CameraPos, 10, 10, 20);

        var CurrentTick = $"Tick: {Server.Tick}";
        Drawing.DrawTextShadow(CurrentTick, 10, 35, 20);
    }
}
