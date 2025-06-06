using HealthSystem;
using UnityEngine;
using UnityEngine.UI;
public class Ship : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _Distance;
    [SerializeField] private Health _hp;
    [SerializeField] private Slider _slider;
    [SerializeField] private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.player;
        Ondamage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && DistanceCheck())
        {
            OpenShipMenu();
        }
    }


    public bool DistanceCheck()
    {
        return (Vector2.Distance(this.transform.position, _player.transform.position) <= _Distance);
    }

    public void OpenShipMenu()
    {
        Debug.Log("ew");
    }

    public void Ondamage()
    {
        Debug.Log("ow");
        _slider.value = (float)_hp.CurrentHealth / _hp.PublicMaxHealth;
        _anim.SetFloat("Life", (float)_hp.CurrentHealth / _hp.PublicMaxHealth);
    }

    public void Ondeath()
    {

        GameManager.Instance.FailState();
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, _Distance);
    }
}
