using System;
using UnityEngine;

public class Timer
{
    private float _startTime;
    private float _duration;
    private bool _enabled;

    public event Action OnTimeOut = delegate { };

    public Timer(float duration)
    {
        Restart(duration);
    }

    public void Restart(float newDuration)
    {
        _duration = newDuration;
        _startTime = Time.time;
        _enabled = true;
    }

    public void Restart()
    {
        Restart(_duration);
    }

    public bool Tick()
    {
        if (!_enabled) return false;

        var isTimeOut = Time.time - _startTime >= _duration;

        if (isTimeOut)
        {
            _enabled = false;
            OnTimeOut.Invoke();
        }

        return isTimeOut;
    }
}
