using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class NetworkOutline : NetworkBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            outlineWidth = value;
        }
    }

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }

    [SerializeField]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    [SyncVar] private float outlineWidth = 2f;

    [Header("Optional")]

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();

    private Renderer[] renderers;
    [SerializeField] private Material outlineMaskMaterial;
    [SerializeField] private Material outlineFillMaterial;

    [SerializeField] private bool _isOutlineApplied = false;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        // Retrieve or generate smooth normals
        LoadSmoothNormals();
    }

    public void SetOutlineFormatLocal(bool isOutlined)
    {
        if (_isOutlineApplied == isOutlined) return;
        _isOutlineApplied = isOutlined;
        if (_isOutlineApplied) AddMaterials();
        else { RemoveMaterials(); print("d"); }
    }

    #region Set outline for player
    [Server]
    public void SetOutlineFormatForPlayer(NetworkConnectionToClient connection, bool isOutlined)
    {
        TargetSetOutlineFormatOnPlayer(connection, isOutlined);
    }

    [Server]
    public async UniTask SetOutlineFormatForPlayer(NetworkConnectionToClient connection, bool isOutlined, float duration)
    {
        TargetSetOutlineFormatOnPlayer(connection, isOutlined);
        await UniTask.Delay((int)duration * 1000);
        TargetSetOutlineFormatOnPlayer(connection, !isOutlined);
    }

    [Command]
    public void CmdSetOutlineFormatForPlayer(NetworkConnectionToClient connection, bool isOutlined)
    {
        SetOutlineFormatForPlayer(connection, isOutlined);
    }

    [TargetRpc]
    private void TargetSetOutlineFormatOnPlayer(NetworkConnectionToClient connection, bool isOutlined)
    {
        SetOutlineFormatLocal(isOutlined);
    }

    #endregion

    [ServerCallback]
    public void SetOutlineFormatForAllPlayers(bool isOutlined)
    {
        RpcSetOutlineFormat(isOutlined);
    }

    [ServerCallback]
    public async UniTask SetOutlineFormatForAllPlayers(bool isOutlined, float duration)
    {
        RpcSetOutlineFormat(isOutlined);
        await UniTask.Delay((int)duration * 1000);
        RpcSetOutlineFormat(!isOutlined);
    }

    [Command]
    public void CmdSetOutlineFormat(bool isOutlined)
    {
        SetOutlineFormatForAllPlayers(isOutlined);
    }

    [ClientRpc]
    private void RpcSetOutlineFormat(bool isOutlined)
    {
        SetOutlineFormatLocal(isOutlined);
    }

    private void AddMaterials()
    {
        foreach (var renderer in renderers)
        {

            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    private void RemoveMaterials()
    {
        foreach (var renderer in renderers)
        {

            // Remove outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }



    void LoadSmoothNormals()
    {

        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            var renderer = meshFilter.GetComponent<Renderer>();

            if (renderer != null)
            {
                CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
            }
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {

            // Skip if UV3 has already been reset
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }

            // Clear UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

            // Combine submeshes
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh)
    {

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    void CombineSubmeshes(Mesh mesh, Material[] materials)
    {

        // Skip meshes with a single submesh
        if (mesh.subMeshCount == 1)
        {
            return;
        }

        // Skip if submesh count exceeds material count
        if (mesh.subMeshCount > materials.Length)
        {
            return;
        }

        // Append combined submesh
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

}
