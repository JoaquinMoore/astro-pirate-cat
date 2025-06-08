using System;
using UnityEngine;

namespace Npc
{
    public class Barco : MonoBehaviour
    {
        public static Barco Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}