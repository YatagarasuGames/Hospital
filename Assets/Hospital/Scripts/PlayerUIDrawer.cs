using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIDrawer : NetworkBehaviour
{
    [field: SerializeField] public Transform TaskPostition { get; private set; }
    private GameObject _currentUI;
    [SerializeField] private ItemInHandUI _itemInHandUI;
    [SerializeField] private List<InputComponent> _playerControllers;

    [SerializeField] private List<AbilityUI> _abilitiesUI;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Module.OnModuleCreated += HandleModuleUICreated;
        Module.OnModuleDestroyed += HandleModuleUIDestroyed;
        ActiveModule.OnAbilityUsed += HandleActiveAbilityUsed;
    }


    public void CreateUI(GameObject uiToCreate, GameObject uiCreator)
    {
        _currentUI = Instantiate(uiToCreate, TaskPostition);
        _currentUI.GetComponent<UI>().Init(uiCreator);
        DisableInput();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseUI()
    {
        if (_currentUI) Destroy(_currentUI);
        EnableInput();
        Cursor.lockState = CursorLockMode.Locked;
    }

    [Command]
    private void DisableInput()
    {
        foreach (var controller in _playerControllers) controller.SetActiveState(false);

    }

    [Command]
    private void EnableInput()
    {
        foreach (var controller in _playerControllers) controller.SetActiveState(true);

    }

    public void HandleModuleUICreated(uint caller, ModuleData moduleData, string inputKey)
    {
        if (caller != NetworkClient.localPlayer.netId) return;
        foreach (var abilityUI in _abilitiesUI)
        {
            print(abilityUI);
            if (!abilityUI.Inited)
            {
                print(moduleData);
                abilityUI.gameObject.SetActive(true);
                abilityUI.Init(moduleData, moduleData.Sprite, inputKey);
                return;
            }
        }
    }

    private void HandleModuleUIDestroyed(uint caller, ModuleData moduleData)
    {
        if (caller != NetworkClient.localPlayer.netId) return;
        foreach (var abilityUI in _abilitiesUI)
        {
            if (abilityUI.ModuleData == moduleData)
            {
                abilityUI.gameObject.SetActive(false);
                abilityUI.Clear();
                return;
            }
        }
    }

    private void HandleActiveAbilityUsed(ModuleData moduleData, float cooldown)
    {
        foreach (var abilityUI in _abilitiesUI)
        {
            if (abilityUI.ModuleData == moduleData)
            {
                abilityUI.UpdateCooldown(Time.time, cooldown);
                return;
            }
        }
    }

    public void UpdateItemInHandUI(GameObject itemDataGameObject)
    {
        if (!itemDataGameObject) _itemInHandUI.gameObject.SetActive(false);
        _itemInHandUI.gameObject.SetActive(true);
        var itemData = itemDataGameObject.GetComponent<InHandItem>().Data;
        _itemInHandUI.Name.text = itemData.Name;
        _itemInHandUI.Description.text = itemData.Description;
        _itemInHandUI.Icon.sprite = itemData.Icon;
    }

    public void ResetItemInHandUI() => _itemInHandUI.gameObject.SetActive(false);

    public void OnDisable()
    {
        Module.OnModuleCreated -= HandleModuleUICreated;
        Module.OnModuleDestroyed -= HandleModuleUIDestroyed;
        ActiveModule.OnAbilityUsed -= HandleActiveAbilityUsed;
    }
}
