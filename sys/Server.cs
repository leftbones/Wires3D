using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Wires3D;

class Server {
    public World World { get; private set; }
    public List<Client> Clients { get; private set; }
    public int Tick { get; private set; }

    public bool ShouldExit { get; set; }

    public int TicksPerSecond { get; set; } = 20;           // How many times the game logic is updated per second

    public Server() {
        World = new World(32, 32);

        Clients = new List<Client>();
        AddClient();
    }

    // Add a new client to the server
    public void AddClient() {
        var NewClient = new Client(this);

        NewClient.TextureCache.Add("node", LoadTexture("assets/textures/node.png"));

        Clients.Add(NewClient);
    }

    // Handle input sent from the client
    public void HandleInput() {
        foreach (var Client in Clients) {
            Client.GetInput();
        }
    }

    // Update the server and clients, tick clients based on TPS
    public void Update() {
        Tick++;

        HandleInput();

        foreach (var Client in Clients) {
            Client.Update();

            if (Tick % TicksPerSecond == 0) {
                Client.Tick();
            }
        }
    }

    public void Draw() {
        foreach (var Client in Clients) {
            Client.Draw();
        }
    }
}