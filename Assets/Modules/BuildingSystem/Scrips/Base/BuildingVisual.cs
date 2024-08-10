using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuildSystem
{
    public class BuildingVisual
    {
        private SpriteRenderer[] _renderes;
        protected Animator _animatiorControler;

        public BuildingVisual(VisualSettings settings)
        {
            _animatiorControler = settings.AnimatiorControler;
            _renderes = settings._Renderes;
        }



        public void FullHp()
        {
            //_animatiorControler.SetTrigger("FullHp");
        }
        public void TriggerHit() { } //=> _animatiorControler.SetTrigger("hit");
        public void Destroy(bool state)
        {
            //_animatiorControler.SetBool("destroy", state);
            //_animatiorControler.ResetTrigger("FullHp");
        }
        //public void PlayAudio(AudioDataSO data)
        //{
        //    AudioManager.Instance.PlayAudio(data);
        //}

        #region INTERACT

        public void Focus()
        {
            //foreach (var item in _renderes)
            //    item.color = _task.IsBlocked ? Color.yellow : _focusColor.Color;
        }

        public void Unfocus()
        {
            //foreach (var item in _renderes)
            //    item.color = Color.white;
        }

        //public Task Interact(CrewMember crewMember)
        //{
        //    return _task;
        //}


        #endregion


        [System.Serializable]
        public class VisualSettings
        {
            public Animator AnimatiorControler;
            public SpriteRenderer[] _Renderes;
        }
    }
}