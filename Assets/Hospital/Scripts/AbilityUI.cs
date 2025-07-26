using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public ModuleData ModuleData {  get; private set; }
    [SerializeField] private Image _abilityImage;
    [SerializeField] private TMP_Text _abilityUseButton;

    private bool _isOnCooldown = false;
    private float _useTime;
    private float _cooldownTime;
    public bool Inited { get; private set; } = false;

    public void Init(ModuleData data, Sprite abilitySprite, string useText)
    {
        ModuleData = data;
        _abilityImage.sprite = abilitySprite;
        _abilityUseButton.text = useText;
        Inited = true;
        _abilityImage.type = Image.Type.Filled;
    }
    private void Update()
    {
        if (!_isOnCooldown) return;
        if (Time.time >= _useTime+_cooldownTime) _isOnCooldown = false;
        _abilityImage.fillAmount = (Time.time - _useTime) / _cooldownTime;
        //print($"{Time.time} AND {_nextUseTime}");
    }

    public void UpdateCooldown(float useTime, float cooldown)
    {
        _isOnCooldown = true;
        _useTime = useTime;
        _cooldownTime = cooldown;
    }

    public void Clear()
    {
        ModuleData = null;
        _abilityImage.sprite = null;
        _abilityUseButton.text = string.Empty;
        _isOnCooldown = false;
        _useTime = 0;
        _cooldownTime = 0;
        Inited = false;
        _abilityImage.fillAmount = 1;
    }


}
