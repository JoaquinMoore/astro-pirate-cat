using UnityEngine;
using TMPro;

public class WaveScreen : MenuScreen
{
    [SerializeField] private TextMeshProUGUI _waveText;

    public override void OpenScreen()
    {
        _waveText.text = "Oleada " + WaveManager.IndexWave;
        base.OpenScreen();
    }

}
