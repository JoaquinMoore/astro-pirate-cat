using UnityEngine;

namespace Asteroid
{
    public class AsteroidMaterial : Asteroid
    {
        [SerializeField] VisualMaterialAsteroids _visualResourse;
        [SerializeField] Materials _typeMat;
        [SerializeField] int[] _varCount;
        [SerializeField] Animator _anim;

        private void Awake()
        {
            _typeMat.Amount = _varCount[Random.Range(0, _varCount.Length)];
        }

        public void Death()
        {
            GameManager.Instance.AddMaterialAmount(_typeMat);
            var visualResours = Instantiate(_visualResourse, transform);
            visualResours.Initial(_typeMat.Type, _typeMat.Amount);
            visualResours.transform.SetParent(null);

            _anim.Play("Destroy");
        }

        void desactive() => gameObject.SetActive(false);
    }
}