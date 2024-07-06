using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BaseBullet
{

    public override IEnumerator OnInpact() {yield return null; Destroy(gameObject); StopAllCoroutines(); }

    protected int _damage;
    protected float _speed;
    protected float _timer;
    protected List<string> _targetsTag;

    public override void SetUp(BaseBulletData data)
    {
        StandardBulletData _data = data as StandardBulletData;

        _damage = _data._damage;
        _speed = _data._speed;
        _timer = _data._timer;
        StartCoroutine(DespawnTimer());
    }
    public override void Move()
    {
        _rigidBody.position += (Vector2)_rigidBody.transform.right * Time.fixedDeltaTime * _speed;
    }



    public override IEnumerator DespawnTimer() { yield return new WaitForSeconds(_timer); Destroy(gameObject); }

}
