using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace HookToolSystem
{
    public class AnchorsBehavioursTest
    {
        private HookTool _hookTool;
        private GameObject _hook;
        private HookAnchor _swingAnchor, _approachAnchor;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            EditorSceneManager.LoadSceneInPlayMode(
                "Assets/Modules/HookTool/Tests/PlayMode/Rebuilds/HooktoolWithSwingAndApproachAnchors.unity",
                new LoadSceneParameters(LoadSceneMode.Single)
            );

            yield return null;
            _hookTool = Object.FindObjectOfType<HookTool>();
            _hook = GameObject.Find("Hook");
            _swingAnchor = GameObject.Find("SwingAnchor").GetComponent<HookAnchor>();
            _approachAnchor = GameObject.Find("ApproachAnchor").GetComponent<HookAnchor>();
        }

        [UnityTest]
        public IEnumerator SwingAnchor()
        {
            // Obtener la distancia inicial del hooktool al anchor.
            var initialDistance = HookToolDistanceAnchor(_swingAnchor);

            AssertGrab(_hookTool, _hook, _swingAnchor);

            // Validation: Asegurar que la distancia entre el hooktool y el anchor sea la misma luego de estar en movimiento.
            _hookTool.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            yield return new WaitForSeconds(2);
            Assert.AreApproximatelyEqual(initialDistance, HookToolDistanceAnchor(_swingAnchor));
        }

        [UnityTest]
        public IEnumerator ApproachAnchor()
        {
            // Obtener la distancia inicial del hooktool al achor.
            var initialDistance = HookToolDistanceAnchor(_approachAnchor);

            AssertGrab(_hookTool, _hook, _approachAnchor);

            yield return new WaitForSeconds(2);

            // Validation: Asegurar que la distancia entre el hooktool y el anchor sea la distancia minima.
            Assert.AreApproximatelyEqual(HookToolDistanceAnchor(_approachAnchor), _hookTool.MinDistance, 0.1f);

            // Validation: Asegurar que la distancia entre el hooktool y el anchor no sea igual que la distancia inicial.
            Assert.AreNotApproximatelyEqual(initialDistance, HookToolDistanceAnchor(_approachAnchor));
        }

        private float HookToolDistanceAnchor(HookAnchor anchor) => Vector2.Distance(_hookTool.transform.position, anchor.transform.position);

        private static void AssertGrab(HookTool hookTool, GameObject hook, HookAnchor anchor)
        {
            // Validation: Asegurar que al engancharse, el hook se posiciona sobre el anchor.
            hookTool.Grab(anchor.GetComponent<Collider2D>(), hook);
            Assert.AreEqual(hook.transform.position, anchor.transform.position);
        }
    }
}
