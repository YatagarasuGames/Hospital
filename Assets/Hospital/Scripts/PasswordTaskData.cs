using UnityEngine;

[CreateAssetMenu(fileName = "NewPasswordData", menuName = "Password Data", order = 53)]
public class PasswordTaskData : ScriptableObject
{
    [field: SerializeField] public string Password { get; private set; }
    [field: SerializeField] public Sprite PasswordSprite { get; private set; }
}
