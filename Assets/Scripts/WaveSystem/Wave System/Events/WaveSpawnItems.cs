using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Events/SpawnItems")]
public class WaveSpawnItems : WaveEvents
{
    [Tooltip("Lista de elementos que uno quiere spawnear")]
    public List<Items> _Items = new();

    [System.Serializable]
    public class Items
    {
        [Header("Data")]
        [Tooltip("Refecencia al objeto que uni quiere spawnear(Nota: requiere si o si un ciclecolider para funcionar)")]
        public GameObject Entety;
        [Tooltip("El color de la zona que uno quiere que tome")]
        public Color MapColor;
        [Tooltip("el radio que uno quiere spawne un objeto independientemente de otros")]
        public float MaxAreaOfSpawn;
        [Tooltip("la cantidad de objetos que uno quiere que spawne")]
        public int Amount;
        [Tooltip("El tamaño del espacio que va a tomar entre cada objeto")]
        public float Size;

        [Header("Force SpawnPoint")]
        [Tooltip("si queres que el objeto spawne en un punto especifico")]
        public bool PlaceOnID;
        [Tooltip("La ID del transform que uno quiere que valla"),Range(0,100)]
        public int PlaceID;
    }

    public override void Event()
    {
        //Spawner.StartItemWave?.Invoke(this);
        //UIManager.Instance?.game.ShowAlert(true);
    }

}
