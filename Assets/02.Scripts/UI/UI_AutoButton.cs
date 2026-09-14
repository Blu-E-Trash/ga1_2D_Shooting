using UnityEngine;
using UnityEngine.UI;

public class UI_AutoButton : MonoBehaviour
{
    [SerializeField]
    private Image _autoImage;
    [SerializeField]
    private Sprite[] _image;
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
    }
    private void InitImage()
    {
        _autoImage.sprite = _autoOn ? _image[1] : _image[0];
    }
    public void ToggleAuto()
    {
        _autoOn = !_autoOn;
        InitImage();
        _playerFire.SetAuto(_autoOn);
    }
}
