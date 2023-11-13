using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace Wires3D;

static class Drawing {
    // Draw text with a drop shadow
    public static void DrawTextShadow(string text, int x, int y, int size) {
        DrawText(text, x + 1, y + 1, size, Color.BLACK);
        DrawText(text, x, y, size, Color.WHITE);
    }

    // Draw a textured cube
    public static void DrawCubeTexture(Texture2D texture, Vector3 position, float width, float height, float length, Color color) {
        float x = position.X;
        float y = position.Y;
        float z = position.Z;

        // Set desired texture to be enabled while drawing following vertex data
        rlSetTexture(texture.id);

        rlBegin(DrawMode.QUADS);
        rlColor4ub(color.r, color.g, color.b, color.a);

        // Front Face
        // Normal Pointing Towards Viewer
        rlNormal3f(0.0f, 0.0f, 1.0f);
        rlTexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
        rlTexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
        rlTexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
        rlTexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        rlVertex3f(x - width / 2, y + height / 2, z + length / 2);

        // Back Face
        // Normal Pointing Away From Viewer
        rlNormal3f(0.0f, 0.0f, -1.0f);
        rlTexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
        rlTexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
        rlTexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
        rlTexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        rlVertex3f(x + width / 2, y - height / 2, z - length / 2);

        // Top Face
        // Normal Pointing Up
        rlNormal3f(0.0f, 1.0f, 0.0f);
        rlTexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
        rlTexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
        rlTexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
        rlTexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        rlVertex3f(x + width / 2, y + height / 2, z - length / 2);

        // Bottom Face
        // Normal Pointing Down
        rlNormal3f(0.0f, -1.0f, 0.0f);
        rlTexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
        rlTexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
        rlTexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
        rlTexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        rlVertex3f(x - width / 2, y - height / 2, z + length / 2);

        // Right face
        // Normal Pointing Right
        rlNormal3f(1.0f, 0.0f, 0.0f);
        rlTexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
        rlTexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
        rlTexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
        rlTexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        rlVertex3f(x + width / 2, y - height / 2, z + length / 2);

        // Left Face
        // Normal Pointing Left
        rlNormal3f(-1.0f, 0.0f, 0.0f);
        rlTexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
        rlTexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
        rlTexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
        rlTexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
        rlEnd();

        rlSetTexture(0);
    }
}