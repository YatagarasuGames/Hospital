using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitiesHint : UI
{
    [SerializeField] private List<Module> modules = new List<Module>();

    [SerializeField] private GameObject _noModulesWarning;
    [SerializeField] private GameObject _modulesHintHolder;
    [SerializeField] private GameObject _moduleHint;
    public override void Init(GameObject uiCreator)
    {
        _noModulesWarning.SetActive(false);
        base.Init(uiCreator);
        modules = NetworkClient.localPlayer.gameObject.GetComponentsInChildren<Module>().ToList();
        InitModulesHint();
    }

    private void InitModulesHint()
    {
        if (modules.Count == 0) { _noModulesWarning.SetActive(true); return; }

        foreach(var module in modules)
        {
            GameObject moduleHint = Instantiate(_moduleHint, _modulesHintHolder.transform);
            moduleHint.GetComponent<ModuleHint>().Init(module.ModuleData);
        }
    }
}
