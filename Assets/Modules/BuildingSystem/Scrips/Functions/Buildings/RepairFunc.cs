using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class RepairFunc
    {
        private int _repaitAmount;
        private float _repairIntervals;
        private GameObject _repairParticles;
        private Vector2 _particleOffset;
        private BuildingControler _model;

        private GameObject _currentParticle;
        //private AudioDataSO _reapirSound;

        [System.Serializable]
        public class Settings
        {
            public int RepairAmounts;
            public float ReapirIntervals;
            public GameObject RepairParticles;
            public Vector2 ParticleOffset;
            //public AudioDataSO ReapirSound;
        }

        public RepairFunc(Settings settings, BuildingControler model)
        {
            _repaitAmount = settings.RepairAmounts;
            _repairIntervals = settings.ReapirIntervals;
            _repairParticles = settings.RepairParticles;
            _particleOffset = settings.ParticleOffset;
            _model = model;
            //_reapirSound = settings.ReapirSound;
        }


        public void StartRepairs(GameObject npc)
        {
            if (npc != null)
                npc.transform.position = (_model._base.transform.position + (Vector3)(Random.insideUnitCircle * 2));
            if (_repairParticles != null)
                _currentParticle = Object.Instantiate(_repairParticles, _model._base.transform.position + (Vector3)_particleOffset, _model._base.transform.rotation);
            Debug.Log(_currentParticle);
            _model._base.StartCoroutine(Repair());
            //_model._visual.PlayAudio(_reapirSound);
            //task.Finish();
        }

        IEnumerator Repair()
        {
            if (_model._model._hpFuncion.PublicCurrentHealth >= _model._model._hpFuncion.PublicMaxHealth)
            {
                //AudioManager.Instance.Stop(Model.Data.RepairsSoundName.ToString());
                FinishRepairs();
            }
            else
            {
                //AudioManager.Instance.Play(Model.Data.RepairsSoundName.ToString());
                _model._model._hpFuncion.Heal(_repaitAmount);
                yield return new WaitForSeconds(_repairIntervals);
                _model._base.StartCoroutine(Repair());
            }

        }


        public void FinishRepairs()
        {
            if (_model._base._RtsHitbox) _model._base._RtsHitbox.SetActive(false);
            _model._visual.Destroy(false);
            _model._visual.FullHp();
            _model._base._Col.enabled = true;
            if (_currentParticle) Object.Destroy(_currentParticle);
        }

    }
}