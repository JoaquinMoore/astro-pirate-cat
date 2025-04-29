using UnityEngine;
using UnityEngine.UI;
public class GameScreen : MenuScreen
{
    [SerializeField] private Slider _slider;
    void Start()
    {
        
    }


    void Update()
    {
        VisualStuff();
    }

    public void VisualStuff()
    {

        _slider.value = GameManager._wave.WaveProgress();
    }

}
