using UnityEngine;
using System.Collections;

public class ConstructionAxisXYZ : MonoBehaviour
{
    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        float minPosX = ConstructionController.Instance.min_pos_x;
        float minPosY = ConstructionController.Instance.min_pos_y;
        float minPosZ = ConstructionController.Instance.min_pos_z;

        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
        //Draw X axis
        GL.Color(Color.red);
        GL.Vertex3(minPosX, minPosY, minPosZ);
        GL.Vertex3(2.5f + minPosX, minPosY, minPosZ);
        GameObject.Find("X Axis Text").transform.position = new Vector3(3f + minPosX, minPosY, minPosZ);

        //Draw Y axis
        GL.Color(Color.green);
        GL.Vertex3(minPosX, minPosY, minPosZ);
        GL.Vertex3(minPosX, 1.5f + minPosY, minPosZ);
        GameObject.Find("Y Axis Text").transform.position = new Vector3(minPosX, 2f + minPosY, minPosZ);

        //Draw Z axis
        GL.Color(Color.blue);
        GL.Vertex3(minPosX, minPosY, minPosZ);
        GL.Vertex3(minPosX, minPosY, 2.5f + minPosZ);
        GameObject.Find("Z Axis Text").transform.position = new Vector3(minPosX, minPosY, 3f + minPosZ);

        GL.End();
        GL.PopMatrix();
        

    }

}