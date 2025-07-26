using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleHint : MonoBehaviour
{
    [SerializeField] private TMP_Text _moduleName;
    [SerializeField] private TMP_Text _moduleDescription;
    [SerializeField] private Image _moduleIcon;
    public void Init(ModuleData moduleData)
    {
        _moduleName.text = moduleData.Name;
        _moduleDescription.text = moduleData.Description;
        _moduleIcon.sprite = moduleData.Sprite;
    }
}
