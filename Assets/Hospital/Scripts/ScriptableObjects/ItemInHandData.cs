using UnityEngine;

[CreateAssetMenu(fileName = "New IHItem Data", menuName = "IHItem Data", order = 51)]
public class ItemInHandData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
