using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildBtnVisualTest : MonoBehaviour
{
    [SerializeField] bool _currentBin;
    [SerializeField] string _amountBuild;
    [SerializeField] Image _infiniteIcon;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] TMP_Text _text;

    private void Awake()
    {
        _canvas.alpha = 0;
    }

    private void OnValidate()
    {
        if (!_currentBin)
        {
            _text.text = _amountBuild;

            if (_amountBuild.Length == 0)
            {
                _infiniteIcon.gameObject.SetActive(true);
                _text.gameObject.SetActive(false);
            }
            else
            {
                _infiniteIcon.gameObject.SetActive(false);
                _text.gameObject.SetActive(true);
            }
        }
    }
}
