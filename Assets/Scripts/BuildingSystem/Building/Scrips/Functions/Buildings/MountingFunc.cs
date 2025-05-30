using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class MountingFunc
    {
        List<Slots> Seats = new();
        BuildingControler _model;
        float _bonusFireRate;
        bool _changeSortingLayer;

        int _currentGunOrderDraw;
        int _currentOrderDraw;
        int _selectedBodyLayer;
        int _selectedGunLayer;
        [System.Serializable]
        public class Settings
        {
            public List<Transform> Pos = new();
            public float BonusFireRate;
            public bool ChangeSortingLayer;

            public int _selectedBodyLayer;
            public int _selectedGunLayer;
        }


        public MountingFunc(Settings settings, BuildingControler model)
        {
            foreach (var item in settings.Pos)
            {
                Seats.Add(new() { Transform = item, });
            }
            _model = model;
            _bonusFireRate = settings.BonusFireRate;
            _changeSortingLayer = settings.ChangeSortingLayer;

            _selectedBodyLayer = settings._selectedBodyLayer;
            _selectedGunLayer = settings._selectedGunLayer;
        }




        public Slots SetCrew(GameObject members)
        {
            Slots seatholder = new();
            foreach (var item in Seats)
            {
                if (item.Member == null)
                {
                    item.Member = members;
                    seatholder = item;
                    break;
                }
                seatholder = null;
            }

            if (seatholder == null || !_model._base._Col.enabled)
                return null;

            if (members == null)
                return null;



            seatholder.Member.transform.position = seatholder.Transform.position;
            //seatholder.Member.StopMovement();
            //seatholder.Member.transform.position = holder.Transform.position;

            if (_bonusFireRate != 0)
            {
                //seatholder.Member.AddAttackSpeed(_BonusFireRate);

            }


            if (_changeSortingLayer)
            {
                var spriteCrew = seatholder.Member.GetComponent<SpriteRenderer>();
                //var gunsprite = holder.Member.GetComponentInChildren<FireWeapon>().GetComponent<SpriteRenderer>();
                //_currentGunOrderDraw = gunsprite.sortingOrder;
                _currentOrderDraw = spriteCrew.sortingOrder;
                //
                spriteCrew.sortingOrder = _selectedBodyLayer;
                //gunsprite.sortingOrder = _selectedGunLayer;
            }

            return seatholder;
            //members.Interactable = false;

        }




        public void RemoveMounted()
        {


            foreach (var item in Seats)
            {
                if (item.Member == null)
                    continue;


                if (_changeSortingLayer)
                {
                    //item.Member.GetComponentInChildren<SpriteRenderer>().sortingOrder = _currentOrderDraw;
                    //item.Member.GetComponentInChildren<FireWeapon>().GetComponent<SpriteRenderer>().sortingOrder = _currentGunOrderDraw;
                }


                //item.Member.CurrentPosition = transform.position + (Vector3)(Random.insideUnitCircle * _DeployRaduis).normalized * _DeployRaduis;
                //item.Member.Interactable = true;
                item.Member = null;

            }
        }

        public void CheckMounted()
        {
            foreach (var item in Seats)
            {
                if (item.Member == null)
                    continue;


                if (CheckDistance(2, item.Transform, item.Member.transform))
                {
                    //item.Member.StopMovement();
                    //item.Member.transform.position = item.Transform.position;
                    Object.Destroy(item.Member);
                }

                //if (item.Member.Tasks.Count > 0)
                //{
                //    if (_changeSortingLayer)
                //    {
                //        item.Member.GetComponentInChildren<SpriteRenderer>().sortingOrder = _currentOrderDraw;
                //        item.Member.GetComponentInChildren<FireWeapon>().GetComponent<SpriteRenderer>().sortingOrder = _currentGunOrderDraw;
                //    }
                //    if (_bonusFireRate != 0)
                //        item.Member.RemoveAttackSpeed(_BonusFireRate);
                //    item.Member.Interactable = true;
                //    item.Member = null;
                //}
            }
        }

        public bool CheckDistance(float distance, Transform StartingPos, Transform npc)
        {
            if (distance < Vector2.Distance(StartingPos.position, npc.position))
                return true;

            return false;
        }

        [System.Serializable]
        public class Slots
        {
            public GameObject Member;
            public Transform Transform;
        }



    }
}