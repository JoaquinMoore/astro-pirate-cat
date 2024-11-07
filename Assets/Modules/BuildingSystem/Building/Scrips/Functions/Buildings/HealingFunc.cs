using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class HealingFunc
    {
        GameObject _healingParticles;
        List<HealingSlots> hslots = new();
        int _healAmount;
        float _healIntervals;

        [System.Serializable]
        public class Settings
        {
            public int HealAmount;
            public float HealIntervals;
            public GameObject HealingParticles;
        }


        public HealingFunc(Settings settings)
        {
            _healAmount = settings.HealAmount;
            _healIntervals = settings.HealIntervals;
            _healingParticles = settings.HealingParticles;
        }


        public void StartHealing(MountingFunc.Slots obs)
        {
            //slot.Member.StartHealing(HealAmount, HealIntervals);

            HealingSlots holder = new()
            {
                Slot = obs,
                ParicleEffect = Object.Instantiate(_healingParticles, obs.Member.transform.position, obs.Member.transform.rotation),
            };

            hslots.Add(holder);
        }



        void StopHealing(HealingSlots slot)
        {
            //lot.Member.StopAllCoroutines();
            //lot.crewtask.Finish();
            //lot.Member = null;
            //udioManager.Instance.Stop(Model.Data.HealingSoundName.ToString());

            if (slot.ParicleEffect != null)
                Object.Destroy(slot.ParicleEffect);
            hslots.Remove(slot);
        }

        void CheckHealed()
        {

            foreach (var item in hslots)
            {
                if (item.Slot.Member == null)
                    continue;
                //if (item.Member.FullyHealed())
                //{
                //    item.Member.CurrentPosition = model.transform.position + (Vector3)(Random.insideUnitCircle * 5).normalized * 5;
                //    StopHealing(item);
                //}
            }
        }

        public class HealingSlots
        {
            public MountingFunc.Slots Slot;
            public GameObject ParicleEffect;
        }
    }
}