using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class AnimationFunc
    {
        [System.Serializable]
        public class Settings
        {
            public Animator controller;
        }

        protected Animator animator;

        public AnimationFunc(Settings settings)
        {
            animator = settings.controller;
        }

        public void FullHp()
        {
            animator.SetTrigger("FullHp");

        }
        public void TriggerHit() => animator.SetTrigger("hit");
        public void Destroy(bool state)
        {
            animator.SetBool("destroy", state);
            animator.ResetTrigger("FullHp");
        }
    }
}