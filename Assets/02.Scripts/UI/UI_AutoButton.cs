using UnityEngine;
using UnityEngine.EventSystems; // 터치 이벤트를 사용하기 위해 추가
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

    private bool _autoOn = false;
    private GameObject _player;
    private PlayerFire _playerFire;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player != null)
        {
            _playerFire = _player.GetComponent<PlayerFire>();
        }
        InitImage();

        _originalScale = transform.localScale;
    }

    private void InitImage()
    {
        _autoImage.sprite = _autoOn ? _image[1] : _image[0];
    }

    public void ToggleAuto()
    {
        _autoOn = !_autoOn;
        InitImage();

        if (_playerFire != null)
        {
            _playerFire.SetAuto(_autoOn);
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