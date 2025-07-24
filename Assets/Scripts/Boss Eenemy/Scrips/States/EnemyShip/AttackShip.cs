using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.FSM;
using WeaponSystem;

namespace NPC.Boss.Ship
{
    public class AttackShip : State<ShipEnemy>
    {
        private WepSettings _settings;
        int AttackId;
        int MaxAttacks;
        MaincharacterController _player;

        public AttackShip(ShipEnemy boss, FiniteStateMachine fsm) : base(boss, fsm)
        {
            if (context == null)
                return;
            _settings = context.Attacksettings;

            foreach (var item in _settings.turrets)
            {
                if (item.attacks.Count > MaxAttacks)
                    MaxAttacks = item.attacks.Count;
            }

            _player = GameManager.Instance.player;

        }

        public override void Enter()
        {
            Debug.Log("attack");
            //fsm.ChangeState<BossBase>(typeof(IdleBoss), context);
            FindAttack();

        }


        public void FindAttack()
        {
            int attackID = 0;
            if (!_settings.RandomiseAttacks)
                attackID = AttackId;
            else
                attackID = RandomAttack();


            foreach (var item in _settings.turrets)
            {
                Attack attack = item.attacks.Find(x => x.AttackId == attackID);
                if (attack == null)
                    continue;

                if (attack.TargetPlayer)
                    context.StartCoroutine(TurretAttackPlayer(item, attack, item.Ref));
                else
                    context.StartCoroutine(TurretAttackRot(item, attack, item.Ref));

            }
        }


        public int RandomAttack()
        {
            return Random.Range(0, MaxAttacks);
        }

        public int SecuentialAttack()
        {
            AttackId++;
            return AttackId = (int)Mathf.Repeat(AttackId, MaxAttacks);
        }




        IEnumerator TurretAttackRot(Turret turretitem, Attack angles, Transform turret)
        {
            float CurrentAngle = 0;
            float AuxAnlge = turret.eulerAngles.z;
            WeaponControler controler = turret.GetComponent<WeaponControler>();

            foreach (var item in angles.angles)
            {
                float Timepased = 0;
                CurrentAngle = turret.eulerAngles.z;
                while (Timepased < item.Time)
                {

                    float evualutae = item.SpeedCurve.Evaluate(Timepased / item.Time);
                    float moving = Mathf.Lerp(0, item.Angle, evualutae);
                    Timepased += Time.deltaTime;
                    turret.rotation = Quaternion.Euler(0, 0, CurrentAngle + moving);
                    if (item.IsFiring)
                        controler.PrimaryFireDown();
                    yield return null;
                }
                controler.PrimaryFireUp();
            }

            turret.rotation = Quaternion.Euler(0, 0, AuxAnlge);
            turretitem.DoneAttacking = true;
        }
        
        IEnumerator TurretAttackPlayer(Turret turretitem, Attack angles, Transform turret)
        {
            WeaponControler controler = turret.GetComponent<WeaponControler>();
            float Timepased = 0;
            float AuxAnlge = turret.eulerAngles.z;





            while (Timepased < angles.angles[0].Time)
            {
                Vector2 pos = (Vector2)_player.transform.position - (Vector2)turretitem.Ref.position;
                turretitem.Ref.transform.right = pos;
                Timepased += Time.deltaTime;
                controler.PrimaryFireDown();
                controler.MouseAim(_player.transform.position);
                yield return null;
            }
            turret.rotation = Quaternion.Euler(0, 0, AuxAnlge);
            controler.PrimaryFireUp();
            controler.MouseAim(controler.transform.position + controler.transform.right * 2);
            turretitem.DoneAttacking = true;
        }



        public override void LogicUpdate()
        {

            foreach (var item in _settings.turrets)
            {
                if (item.DoneAttacking == false)
                {
                    return;
                }
            }

            fsm.ChangeState<BossBase>(typeof(IdleBoss), context);
        }

        public override void Exit()
        {
            Debug.Log("salir");
            foreach (var item in _settings.turrets)
            {
                item.DoneAttacking = false;
            }

            SecuentialAttack();
        }



        [System.Serializable]
        public class WepSettings : StateSettings
        {
            public bool RandomiseAttacks;

            public List<Turret> turrets = new();
        }
        [System.Serializable]
        public class Turret
        {
            public Transform Ref;
            public bool DoneAttacking;
            public List<Attack> attacks = new();
        }
        [System.Serializable]
        public class Attack
        {
            public int AttackId;
            public bool TargetPlayer;
            public List<AngleData> angles = new();
        }
        [System.Serializable]
        public struct AngleData
        {
            public float Angle;
            public float Time;
            public bool IsFiring;
            [Tooltip("la curva que se utilisa para dictar la velosidad durante su movimiento")] public AnimationCurve SpeedCurve;
        }


    }

    //[SerializeField] WeaponHandler.Settings _WeaponData;
    //[SerializeField] List<AttackList> _AttackPattersList = new();
    //
    //private WeaponHandler _WeaponHandler;
    //float CurrentAngle;
    //float Timepased;
    //public int _AttackAmount { get; private set; }
    //float AuxAnlge;
    //
    //private void Awake()
    //{
    //    _WeaponHandler = new(_WeaponData, transform);
    //    _AttackAmount = _AttackPattersList.Count;
    //    _WeaponHandler.CreateInitialWeapons();
    //}
    //
    //void Start()
    //{
    //    AuxAnlge = transform.eulerAngles.z;
    //    _WeaponHandler.AimWeaponTowards(GetVector2(-transform.eulerAngles.z));
    //}
    //
    //IEnumerator Attack(AttackList angles)
    //{
    //    foreach (var item in angles.Anglelist)
    //    {
    //        Timepased = 0;
    //        CurrentAngle = transform.eulerAngles.z;
    //        while (Timepased < item.Time)
    //        {
    //            float evualutae = item.SpeedCurve.Evaluate(Timepased / item.Time);
    //            float moving = Mathf.Lerp(0, item.Angle, evualutae);
    //            Timepased += Time.deltaTime;
    //            transform.rotation = Quaternion.Euler(0, 0, CurrentAngle + moving);
    //            if (item.IsFiring)
    //                _WeaponHandler.Trigger();
    //            yield return null;
    //        }
    //    }
    //    transform.rotation = Quaternion.Euler(0, 0, AuxAnlge);
    //}
    //
    //public void Attack(int attack)
    //{
    //    StopAllCoroutines();
    //    transform.rotation = Quaternion.Euler(0, 0, AuxAnlge);
    //    StartCoroutine(Attack(_AttackPattersList[attack]));
    //}
    //
    //public Vector2 GetVector2(float angle)
    //{
    //    Vector2 V2angle = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    //    return V2angle += (Vector2)transform.position;
    //}
    //
    //[System.Serializable]
    //public struct AngleData
    //{
    //    public float Angle;
    //    public float Time;
    //    public bool IsFiring;
    //    [Tooltip("la curva que se utilisa para dictar la velosidad durante su movimiento")] public AnimationCurve SpeedCurve;
    //
    //
    //}
    //
    //[System.Serializable]
    //public class AttackList
    //{
    //    public List<AngleData> Anglelist = new();
    //}



}