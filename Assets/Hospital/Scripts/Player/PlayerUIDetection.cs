using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDetection : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private TMP_Text _tipText;
    [SerializeField] private Image _tipSprite;

    private void Update()
    {
        DetectUI();
    }

    private void DetectUI()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 3f))
        {
            if (hit.collider.gameObject.TryGetComponent(out IImageTip tipObject))
            {
                //if (tipObject is ITip) _tipText.text = tipObject.GetUI();
                 print(tipObject.GetUI()); _tipSprite.sprite = (tipObject as IImageTip).GetUI();

            }
            else _tipText.text = "";
            _tipSprite.sprite = null;
        }
        else _tipText.text = "";
        _tipSprite.sprite = null;
    }


}
