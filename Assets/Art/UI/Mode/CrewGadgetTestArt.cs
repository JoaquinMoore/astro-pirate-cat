using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewGadgetTestArt : MonoBehaviour
{
    [Header("Crew Icon")]
    [SerializeField] Image _crewIcon;
    [SerializeField] Sprite _crewSprite;

    [Header("Fill")]
    [SerializeField] Image _fill;
    [SerializeField] float _fillAmount;

    [Header("Task Icon")]
    [SerializeField] Image _taskIcon;
    [SerializeField] Sprite _taskSprite;

    [Header("Btn")]
    [SerializeField] Image _btn;
    [SerializeField] Sprite _btnNormalSprite;
    [SerializeField] Sprite _btnSelectedSprite;
    [SerializeField] bool _isSelected = false;

    private void Awake()
    {
        _crewIcon.sprite = _crewSprite;

        if(_isSelected)
            _btn.sprite = _btnSelectedSprite;
        else
            _btn.sprite = _btnNormalSprite;

        if (_taskSprite)
        {
            _taskIcon.sprite = _taskSprite;
            _taskIcon.gameObject.SetActive(true);
        }
        else
            _taskIcon.gameObject.SetActive(false);

        _fill.fillAmount = _fillAmount;
        if(_fillAmount == 0.3f)
            _fill.color = Color.red;
        else if (_fillAmount <= 0.5f)
            _fill.color = Color.yellow;
        else if (_fillAmount <= 1f)
            _fill.color = Color.green;
    }

    private void OnValidate()
    {
        if (_crewIcon.sprite != _crewSprite)
            _crewIcon.sprite = _crewSprite;

        if (_isSelected)
            _btn.sprite = _btnSelectedSprite;
        else
            _btn.sprite = _btnNormalSprite;

        if (_taskSprite)
        {
            if (_taskIcon.sprite != _taskSprite)
                _taskIcon.sprite = _taskSprite;
            _taskIcon.gameObject.SetActive(true);
        }
        else
            _taskIcon.gameObject.SetActive(false);

        if (_fill.fillAmount != _fillAmount)
        {
            _fill.fillAmount = _fillAmount;

            if (_fillAmount == 0.3f)
                _fill.color = Color.red;
            else if (_fillAmount <= 0.5f)
                _fill.color = Color.yellow;
            else if (_fillAmount <= 1f)
                _fill.color = Color.green;
        }
    }
}
