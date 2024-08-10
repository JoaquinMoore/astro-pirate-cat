using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BuildSystem
{
    public class BuildingBase : MonoBehaviour//, IDamageable
    {

        public BuildingControler _controler { get; private set; }
        [field: SerializeField] public BoxCollider2D _Col { get; private set; }
        [field: SerializeField] public GameObject _RtsHitbox { get; private set; }


        private void Awake()
        {
            _controler = new BuildingControler();
        }

        protected virtual void Update()
        {
            _controler.VirtualUpdate();
        }


        public void SetUp(BuildingVisual visual, BuildingModel based)
        {
            _controler.SetUp(visual, based, this);
        }

        public void Damage()
        {
            if (!_Col.enabled)
                return;
            _controler.Damage();
        }


        public virtual void AnimDeathAction() { }
    }
}