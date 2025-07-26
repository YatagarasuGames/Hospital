using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NicknameChangeMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _nicknamePreview;
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private GameObject _nicknameChangeMenu;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Nickname"))
        {
            _nicknamePreview.text = PlayerPrefs.GetString("Nickname");
        }
        else
        {
            PlayerPrefs.SetString("Nickname", string.Format("User{0}", UnityEngine.Random.Range(1000000, 1000000000)));
            _nicknamePreview.text = PlayerPrefs.GetString("Nickname");
        }
    }

    public void Submit()
    {
        string newNickname = _nicknameInput.text;
        if (newNickname == "") return;
        PlayerPrefs.SetString("Nickname", newNickname);
        _nicknameChangeMenu.SetActive(false);
        _nicknamePreview.text = newNickname;
    }
}
