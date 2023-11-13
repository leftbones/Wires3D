using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;

namespace Wires3D;

unsafe class Camera {
    public Player Player { get; private set; }

    public Vector3 Position { get { return Cam.position; } }
    public Vector3 Target { get { return Cam.target; } }
    public Vector2 ViewAngle { get; private set; }

    public bool MouseLookEnabled { get; set; } = true;

    public float FieldOfView = 80.0f;
    public float Sensitivity = 800.0f;

    private Camera3D Cam;

    private Shader Shader = LoadShader("assets/shaders/lighting.vs", "assets/shaders/lighting.fs");

    public Camera(Player player) {
        Player = player;

        Cam = new() {
            position = Player.Position,
            target = new Vector3(Player.Position.X, Player.Position.Y, Player.Position.Z - 4.0f),
            up = new Vector3(0.0f, 1.0f, 0.0f),
            fovy = FieldOfView,
            projection = CameraProjection.CAMERA_PERSPECTIVE
        };

        Shader.locs[(int)ShaderLocationIndex.SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(Shader, "viewPos");
        int AmbientLoc = GetShaderLocation(Shader, "ambient");
        float[] Ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
        SetShaderValue(Shader, AmbientLoc, Ambient, ShaderUniformDataType.SHADER_UNIFORM_VEC4);
    }

    public void Update() {
        // Camera Position + Target
        if (MouseLookEnabled) {
            var MouseDelta = GetMouseDelta();

            var ViewX = ViewAngle.X;
            var ViewY = ViewAngle.Y;

            ViewX += MouseDelta.X / -Sensitivity;
            ViewY += MouseDelta.Y / -Sensitivity;

            if (ViewY < -89.0f * (MathF.PI / 180.0f)) {
                ViewY = -89.0f * (MathF.PI / 180.0f);
            } else if (ViewY > 89.0f * (MathF.PI / 180.0f)) {
                ViewY = 89.0f * (MathF.PI / 180.0f);
            }

            ViewAngle = new Vector2(ViewX, ViewY);

        }

        Cam.position = Player.Position;
        Cam.target = Cam.position + Vector3Transform(new Vector3(0, 0, 1), MatrixRotateZYX(new Vector3(-ViewAngle.Y, ViewAngle.X, 0)));
    }

    // Get a raycast from the center of the screen in the camera's direction
    public Ray GetRay() {
        return GetMouseRay(Global.WindowSize / 2, Cam);
    }

    // Begin 3D drawing mode
    public void BeginDraw() {
        BeginMode3D(Cam);
    }

    // End 3D drawing mode
    public void EndDraw() {
        EndMode3D();
    }
}