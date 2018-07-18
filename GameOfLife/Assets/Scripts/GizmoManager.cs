using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    static List<GizmoLine> lines = new List<GizmoLine>();
    void OnPostRender()
    {
        GL.Begin(GL.LINES);
        for (int i = 0; i < lines.Count; i++)
        {
            GL.Color(lines[i].color);
            GL.Vertex(lines[i].a);
            GL.Vertex(lines[i].b);
        }
        GL.End();
        lines.Clear();

		GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(Color.red);
        GL.Vertex3(0, 0.5F, 0);
        GL.Vertex3(0.5F, 1, 0);
        GL.Vertex3(1, 0.5F, 0);
        GL.Vertex3(0.5F, 0, 0);
        GL.Color(Color.cyan);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0.25F, 0);
        GL.Vertex3(0.25F, 0.25F, 0);
        GL.Vertex3(0.25F, 0, 0);
        GL.End();
        GL.PopMatrix();
    }

    public struct GizmoLine
    {
        public Vector3 a;
        public Vector3 b;
        public Color color;
        public GizmoLine(Vector3 a, Vector3 b, Color color)
        {
            this.a = a;
            this.b = b;
            this.color = color;
        }
    }

    public static void DrawLine(Vector3 a, Vector3 b, Color color)
    {
        lines.Add(new GizmoLine(a, b, color));
    }
}

