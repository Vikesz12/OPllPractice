using Ble;

using RubikVisualizers;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

[RequireComponent(typeof(Button))]
public class SwitchToLiveCube : MonoBehaviour
{

    [Inject] private IBle _ble;

    [SerializeField] private GameObject _jsonRubik;
    [SerializeField] private RubikHolder _liveRubik;
    private Button _switchButton;
    private void Start()
    {
        _switchButton = GetComponent<Button>();
        _switchButton.onClick.AddListener(() => SwitchCubes());
    }
    private void SwitchCubes()
    {
        _jsonRubik.SetActive(!_jsonRubik.activeInHierarchy);
        _liveRubik.gameObject.SetActive(!_liveRubik.gameObject.activeInHierarchy);
        if (_liveRubik.gameObject.activeInHierarchy)
        {
            _ble.Write("3", ConnectedDeviceData.ConnectedDeviceId);
        }
    }
}
