using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealthSystem;

public class LaserBullet : BaseBullet
{
    protected List<IHurtable> _targets = new();
    [SerializeField] private int _damage;
    [SerializeField] private int _maxTargets;

    protected List<string> _targetsTag;
    public override void SetUp(BaseBulletData data)
    {
        LaserBulletData _data = data as LaserBulletData;

        _maxTargets = _data.MaxTargets;
        _damage = _data.Damage;
        Debug.Log(_data.Damage);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        _targets.Clear();
    }
    public override void ExternalInput()
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] == null)
                continue;
            _targets[i].Hurt(_damage);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hold = collision.GetComponent<IHurtable>();

        //if (_targets.Count ==_maxTargets)
        //{
        //    return;
        //}
        if (hold != null && !_targets.Contains(hold))
        {
            _targets.Add(hold);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var hold = collision.GetComponent<IHurtable>();

        if (hold != null && _targets.Contains(hold))
        {
            _targets.Remove(hold);
        }
    }
}
