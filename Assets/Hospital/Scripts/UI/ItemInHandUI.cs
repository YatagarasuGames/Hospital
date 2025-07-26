using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInHandUI : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Name { get; set; }
    [field: SerializeField] public TMP_Text Description { get; set; }
    [field: SerializeField] public Image Icon { get; set; }

    [SerializeField] private GameObject _descriptionText;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    _descriptionText.SetActive(true);
        //}
        //if(Input.GetKeyUp(KeyCode.F1)) { _descriptionText.SetActive(false); }
    }
}
