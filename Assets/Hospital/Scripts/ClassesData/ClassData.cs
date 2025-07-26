using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "New Class Data", menuName = "Class Data", order = 52)]
public class ClassData : ScriptableObject
{
    public List<Material> HeadMaterials;
    public List<Material> BodyMaterials;
    public List<GameObject> Modules;
    public AssetReference HeadMesh;
    public AssetReference BodyMesh;
    public int InentorySlots;
    public string Tag;
}
