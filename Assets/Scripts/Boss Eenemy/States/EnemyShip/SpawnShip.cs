using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.FSM;
using Npc;


namespace NPC.Boss.Ship
{
    public class SpawnShip : State<ShipEnemy>
    {
        private SpawnSettings _settings;

        private bool Spawning;
        

        public SpawnShip(ShipEnemy boss, FiniteStateMachine fsm) : base(boss, fsm)
        {
            if (boss == null)
                return;


            _settings = boss.Spawnsettings;
            foreach (var item in _settings.SpawnPoints)
            {
                item.Anims.SetBool("Show", false);
            }
        }

        public override void Enter()
        {
            context.Mov.StopMov();

            Debug.Log("spawn");
            //fsm.ChangeState<BossBase>(typeof(IdleBoss), context);
            foreach (var item in _settings.SpawnPoints)
            {
                item.Anims.SetBool("Show", true);
            }

        }

        public override void LogicUpdate()
        {
            if (!Spawning && context.Mov.Stoped())
                CheckIfReady();
        }


        public void CheckIfReady()
        {
            foreach (var item in _settings.SpawnPoints)
            {
                if (!item.SpawnPointRef.gameObject.activeSelf)
                    return;
            }

            Spawning = true;
            context.StartCoroutine(SpawnFunc());
        }


        IEnumerator SpawnFunc()
        {
            List<SpawnEntsAmount> spawns = new();
            bool Donespawn = false;

            foreach (var item in _settings.SpawnAmountSets)
            {
                Debug.Log(item.Pref);
                SpawnEntsAmount hold = new()
                {
                    Pref = item.Pref,
                    Amount = Random.Range(item.MinAmount, item.MaxAmount)
                };
                spawns.Add(hold);
            }


            while (!Donespawn)
            {
                yield return new WaitForSeconds(1f);
                SpawnEntsAmount ent = spawns[Random.Range(0, spawns.Count)];

                if (ent.Amount > ent.CurrentAmount)
                {
                    SpawnPoints point = _settings.SpawnPoints[Random.Range(0, spawns.Count)];

                    var holder = EnemyManager.Instance.RequestEnemy(ent.Pref);
                    holder.transform.position = point.SpawnPointRef.position;
                    //Object.Instantiate(ent.Pref, point.SpawnPointRef.position, point.SpawnPointRef.rotation);
                    ent.CurrentAmount++;
                    //Debug.Log(ent.Pref + "//" + ent.Amount + "/" + ent.CurrentAmount);

                }
                foreach (var item in spawns)
                {
                    if (item.Amount != item.CurrentAmount)
                    {
                        Debug.Log("not");
                        Donespawn = false;
                        break;
                    }

                    else if (item.Amount == item.CurrentAmount)
                    {
                        Debug.Log("ready");
                        Donespawn = true;
                    }

                }
            }

            foreach (var item in _settings.SpawnPoints)
            {
                item.Anims.SetBool("Show", false);
            }

            fsm.ChangeState<BossBase>(typeof(IdleBoss), context);
        }


        public override void Exit()
        {
            if (context._stop != true)
                context.Anim.SetBool("IsMove", true);
            context.Mov.ResumeMov();
            Debug.Log("finished");
            Spawning = false;
        }


        [System.Serializable]
        public class SpawnSettings : StateSettings
        {
            public List<SpawnEnts> SpawnAmountSets = new();
            public List<SpawnPoints> SpawnPoints = new();

        }

        [System.Serializable]
        public class SpawnEnts
        {
            public NPCController Pref;
            public int MaxAmount;
            public int MinAmount;
        }

        public class SpawnEntsAmount
        {
            public NPCController Pref;
            public int Amount;
            public int CurrentAmount;
        }

        [System.Serializable]
        public class SpawnPoints
        {
            public GameObject GameObjRef;
            public Animator Anims;
            public Transform SpawnPointRef;
        }

    }

}