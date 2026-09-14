using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AutoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image _autoImage;
    [SerializeField]
    private Sprite[] _image;

    [SerializeField]
    private AudioClip _clickSound;
    [SerializeField]
    private float _pressedScale = 0.9f;

    private Vector3 _originalScale;

    private void Start()
    {
        InitImage();
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        // 단축키 '1'을 누르면 오토 모드 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleAuto();
        }
    }

    private void InitImage()
    {
        if (PlayerStatus.Instance != null)
        {
            _autoImage.sprite = PlayerStatus.Instance.IsAutoMode ? _image[1] : _image[0];
        }
    }

    public void ToggleAuto()
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.ToggleAutoMode();
            InitImage();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = _originalScale * _pressedScale;

        if (_clickSound != null)
        {
            Vector3 playPos = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(_clickSound, playPos);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = _originalScale;
    }
}