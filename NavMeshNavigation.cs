using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class NavMeshNavigation : MonoBehaviour
{
    [Header("NavMesh Visualization")]
    public Color navMeshColor = new Color(0.2f, 0.6f, 1f, 0.45f);
    public float yOffset = 0.02f;
    public float refreshInterval = 0.25f;

    private Mesh navMesh;
    private float nextRefresh;

    private void Start()
    {
        var go = new GameObject("NavMeshVisual");
        go.transform.position = Vector3.zero;

        navMesh = new Mesh { indexFormat = IndexFormat.UInt32 };
        go.AddComponent<MeshFilter>().sharedMesh = navMesh;

        var mr = go.AddComponent<MeshRenderer>();
        var mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = navMeshColor;
        mr.sharedMaterial = mat;
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;

        Rebuild();
    }

    private void Update()
    {
        if (Time.time >= nextRefresh)
        {
            nextRefresh = Time.time + refreshInterval;
            Rebuild();
        }
    }

    private void Rebuild()
    {
        var tri = NavMesh.CalculateTriangulation();
        var verts = tri.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].y += yOffset;
        }

        navMesh.Clear();
        navMesh.vertices = verts;
        navMesh.triangles = tri.indices;
    }
}