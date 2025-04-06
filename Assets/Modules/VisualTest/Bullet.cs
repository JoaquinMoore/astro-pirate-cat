using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Material _enemyMat;
    [SerializeField] GameObject _enemyParticle;
    [SerializeField] Material _alliedMat;
    [SerializeField] GameObject _alliedParticle;
    [SerializeField] float _speed;
    [SerializeField] int _timeDestroy = 10;

    float _time;
    GameObject _currentParticle;

    public void Initial(posInWar pos)
    {
        switch (pos)
        {
            case posInWar.player:
                _sprite.material = _alliedMat;
                _currentParticle = _alliedParticle;
                break;
            case posInWar.enemy:
                _sprite.material = _enemyMat;
                _currentParticle = _enemyParticle;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().linearVelocity = - transform.right * _speed * Time.deltaTime;
        _time += Time.deltaTime;

        if (_time >= _timeDestroy)
        {
            Instantiate(_currentParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(_currentParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
