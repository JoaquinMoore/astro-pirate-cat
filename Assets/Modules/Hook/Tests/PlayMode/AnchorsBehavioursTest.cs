using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AnchorsBehavioursTest
{
    private static HookTool _hookTool;
    private static GameObject _hook;
    private static HookAnchor _swingAnchor, _approachAnchor;
    private static Vector2 _swingAnchorStartPosition = new(10, 0);

    private class Rebuild
    {
        public Rebuild()
        {
            CreateHookTool();
            CreateHook();
            CreateSwingAnchor();
        }

        private void CreateHookTool()
        {
            var character = new GameObject();
            _hookTool = character.AddComponent<HookTool>();
        }

        private static void CreateHook()
        {
            _hook = new GameObject();
            _hook.transform.position = Vector2.zero;
        }

        private static void CreateSwingAnchor()
        {
            _swingAnchor = new GameObject().AddComponent<HookAnchor>();
            _swingAnchor.typeOfAnchor = HookAnchor.AnchorType.Swing;
            _swingAnchor.gameObject.AddComponent<BoxCollider2D>();
            _swingAnchor.transform.position = _swingAnchorStartPosition;
        }
    }

    private float HookToolDistanceAnchor => Vector2.Distance(_hookTool.transform.position, _swingAnchor.transform.position);

    [UnityTest]
    public IEnumerator SwingAnchor()
    {
        new Rebuild();

        // Obtener la distancia inicial del hooktool al anchor.
        var initialDistance = HookToolDistanceAnchor;

        // Validation: Asegurar que una vez que se enganchó al anchor, el hook se posiciona sobre el anchor.
        _hookTool.Grab(_swingAnchor.GetComponent<Collider2D>(), _hook);
        Assert.AreEqual(_hook.transform.position, _swingAnchor.transform.position);

        // Validation: Asegurar que la distancia entre el hooktool y el anchor es la misma luego de aplicar una fuerza al character.
        _hookTool.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2);
        Assert.AreApproximatelyEqual(initialDistance, HookToolDistanceAnchor);
    }
}
