using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OsuTaskCircle : MonoBehaviour, IPointerClickHandler
{
    public int _pressIndex { get; private set; }
    private OsuTaskController _controller;
    [SerializeField] private TMP_Text _pressIndexText;
    public void Init(int pressIndex, OsuTaskController controller)
    {
        _pressIndex = pressIndex;
        _controller = controller;
        _pressIndexText.text = pressIndex.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_controller.HandleCirclePressed(_pressIndex))
        {
            Destroy(gameObject);
        }
    }
}
