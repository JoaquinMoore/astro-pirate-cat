using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalStateScreen : MenuScreen
{
    [SerializeField] private TextMeshProUGUI _stateTextDisplay;
    [SerializeField] private GameObject _RestartButton;
    [SerializeField] private GameObject _ContinueButton;


    public override void OpenScreen()
    {
        if (UIManager.MenuManager.GetFinalState())
        {
            _RestartButton.SetActive(true);
            _ContinueButton.SetActive(false);
            _stateTextDisplay.text = "PERDISTE";
        }



        base.OpenScreen();

    }
}
