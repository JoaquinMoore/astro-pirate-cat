using UnityEngine;

namespace Asteroid
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] Sprite[] _allSprites;
        [SerializeField] Vector3[] _scaleAsteroids;

        void Awake()
        {
            InitialStat();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                InitialStat();
        }

        void InitialStat()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = _allSprites[Random.Range(0, _allSprites.Length)];

            transform.localScale = _scaleAsteroids[Random.Range(0, _scaleAsteroids.Length)];

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 361)));
        }
    }
}