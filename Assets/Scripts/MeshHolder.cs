using UnityEngine;

public class MeshHolder : MonoBehaviour
{
    public static MeshHolder Instance;
    public Mesh[] borders;

    private void Awake()
    {
        Instance = this;
    }

    public Mesh GetMeshByName(string name)
    {
        foreach(Mesh mesh in borders)
        {
            if (mesh.name == name) return mesh;
        }
        return null;
    }
}
