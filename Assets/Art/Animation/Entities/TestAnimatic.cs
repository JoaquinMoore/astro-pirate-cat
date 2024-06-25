using UnityEngine;
using TMPro;

enum Job
{
    Bucanero,
    Ingeniero,
    Minero,
    Normal,
}

public class TestAnimatic : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] TMP_Text _tittle;
    [SerializeField] Job _currentJob;
    [SerializeField] GameObject[] _head;
    [SerializeField] GameObject[] _addBody;
    [SerializeField] GameObject[] _tails;

    private void Start()
    {
        _tittle.text = _currentJob.ToString();

        CustomCat();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CustomCat();
    }

    void CustomCat()
    {
        // Cambia la cabeza del gato
        foreach (var head in _head)
            head.SetActive(false);
        var headRandom = Random.Range(0, _head.Length);
        _head[headRandom].SetActive(true);

        // Cambia la cola del gato
        foreach (var tail in _tails)
            tail.SetActive(false);
        var tailRandom = Random.Range(0, _tails.Length);
        _tails[tailRandom].SetActive(true);

        // Cambia la decoración del cuerpo
        foreach (var decoBody in _addBody)
            decoBody.SetActive(false);
        var bodyRandom = Random.Range(0, _addBody.Length);
        _addBody[bodyRandom].SetActive(true);

        // Cambia el trabajo del gato
        var job = new Job[] { Job.Bucanero, Job.Ingeniero, Job.Minero, Job.Normal };
        _currentJob = job[Random.Range(0, job.Length)];
        _tittle.text = _currentJob.ToString();
        _anim.Play(_currentJob.ToString());
    }
}