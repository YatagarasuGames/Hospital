using TMPro;
using UnityEngine;

public class EnterPasswordTaskController : UITask
{
    [SerializeField] private TMP_Text _passwordText;
    [SerializeField] private int _passwordLength = 4;
    private string _correctPassword;

    public override void Init(GameObject uiCreator)
    {
        base.Init(uiCreator);
        _correctPassword = _uiCreator.GetComponent<EnterPasswordTaskGiver>().Password;
        _passwordLength = _correctPassword.Length;
    }
    public void HandleKeyPressed(int key)
    {
        _passwordText.text += key.ToString();
        if(_passwordText.text.Length >= _passwordLength) CheckPassword();
    }

    private void CheckPassword()
    {
        if (_passwordText.text == _correctPassword) CompleteTask();
        else _passwordText.text = string.Empty;
    }
}
