using HealthSystem;
using UnityEngine;
using UnityEngine.UI;
using _UTILITY;
public class Ship : SingletonMono<Ship>
{
    [SerializeField] private MaincharacterController _player;
    [SerializeField] private float _Distance;
    [SerializeField] private Health _hp;
    [SerializeField] private Slider _slider;
    [SerializeField] private Animator _anim;
    [SerializeField] private bool _waiting;

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
        if (_waiting)
        {
            WaitForCamArrival();
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
        _anim.SetTrigger("Hit");
        _slider.value = (float)_hp.CurrentHealth / _hp.PublicMaxHealth;
        _anim.SetFloat("Life", (float)_hp.CurrentHealth / _hp.PublicMaxHealth);
    }

    public void Ondeath()
    {
        GameManager.Instance.player.StopMovement();
        CameraManager.Instance.MoveCamToPlace(transform);
        _waiting = true;
    }

    public void WaitForCamArrival()
    {
        if (!CameraManager.Instance._camMoving)
            return;

        _anim.SetTrigger("Hit");
        _anim.SetFloat("Life", 0);
    }


    public void OnDeathAnim()
    {
        GameManager.Instance.FailState();
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, _Distance);
    }
}
