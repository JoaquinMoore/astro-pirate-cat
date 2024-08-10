using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class SchoolFunc
    {
        float _graduatingTime;
        List<TeachingSlots> slots = new();

        BuildingControler _model;
        public class Settings
        {
            public float GraduatingTime;
            public GameObject particle;
        }


        public SchoolFunc(Settings settings, BuildingControler model)
        {
            _graduatingTime = settings.GraduatingTime;

            _model = model;
        }


        public void GiveStuddent(MountingFunc.Slots slot)
        {
            TeachingSlots teachslot = new()
            {
                slot = slot,
                Particle = null,
                _maxTimer = _graduatingTime
            };

            slots.Add(teachslot);
        }




        public void CheckSlots()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].slot.Member == null)
                {
                    slots.Remove(slots[i]);
                }
            }


            foreach (var item in slots)
            {
                if (item.InternalTimer())
                {
                    Graduation(item);
                }
            }

        }

        public void Graduation(TeachingSlots slot)
        {
            //se gradua aqui

            Object.Destroy(slot.Particle);
            slots.Remove(slot);
        }


        public class TeachingSlots
        {
            public MountingFunc.Slots slot;
            public GameObject Particle;

            public float _maxTimer = 0;
            float _timer = 0;

            public bool InternalTimer()
            {
                _timer += Time.deltaTime;

                if (_timer >= _maxTimer)
                    return true;
                else
                    return false;
            }
        }
    }
}