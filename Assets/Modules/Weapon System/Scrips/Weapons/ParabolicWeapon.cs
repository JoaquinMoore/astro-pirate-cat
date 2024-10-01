using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{

    public class ParabolicWeapon : RangeWeapon
    {
        [Header("Parabolic Settings")]
        [SerializeField] LineRenderer line;
        [SerializeField] Transform _startPos;
        [SerializeField] Transform _endPos;
        [SerializeField] Transform _height;
        [SerializeField] float _vertexCount = 12;


        [Header("Debug")]
        List<Vector3> _linePoints = new();
        [SerializeField] bool _mouseaim;
        [SerializeField] bool _visualdebugline;
        Vector3 mousepos;
        void Start()
        {
            LineSmoothVisual();
            _visualdebugline = false;
        }


        void Update()
        {
            LineSmoothBullets();
            //LineSmoothVisual();
            if (_mouseaim)
                MouseAim();
        }


        void LineSmoothVisual()
        {
            //height.position = new Vector3((startPos.position.x + endPos.position.x), point2YPos, (startPos.position.z + endPos.position.z) / 2);
            var pointList = new List<Vector3>();

            for (float ratio = 0; ratio <= 1; ratio += 1 / _vertexCount)
            {
                var tangent1 = Vector3.Lerp(_startPos.position, _height.position, ratio);
                var tangent2 = Vector3.Lerp(_height.position, _endPos.position, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve - transform.position);
            }

            line.positionCount = pointList.Count;
            line.SetPositions(pointList.ToArray());
            _linePoints = pointList;
        }



        void LineSmoothBullets()
        {
            //height.position = new Vector3((startPos.position.x + endPos.position.x), point2YPos, (startPos.position.z + endPos.position.z) / 2);
            var pointList = new List<Vector3>();

            for (float ratio = 0; ratio <= 1; ratio += 1 / _vertexCount)
            {
                var tangent1 = Vector3.Lerp(_startPos.position, _height.position, ratio);
                var tangent2 = Vector3.Lerp(_height.position, _endPos.position, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve - transform.position);
            }

            _linePoints = pointList;
        }


        void MouseAim()
        {
            _endPos.position = mousepos;
            //_height.position = transform.right * (Vector3.Distance(transform.position, _endPos.position)/2);
        }

        public override void MousePos(Vector3 pos)
        {
            mousepos = pos;
        }

        public List<Vector3> GivePoints()
        {
            List<Vector3> newpoints = new();

            foreach (var item in _linePoints)
            {
                newpoints.Add(item + transform.position);
            }

            return newpoints;
        }


        private void OnDrawGizmos()
        {
            if (_visualdebugline)
            {
                LineSmoothVisual();
            }
        }

    }

}