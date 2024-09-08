using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assets.Modules.NPCs.Golden.Movements;

public class MovementTest
{
    private MovementContext _goldenMovementContext;
    private MockInput _mockInput;

    private struct Case
    {
        public Vector2 direction;
        public Vector2 expectedPosition;
        public float seconds;
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        EditorSceneManager.LoadSceneInPlayMode(
            "Assets/Modules/NPCs/Golden/Tests/PlayMode/Rebuilds/MovementBasic.unity",
            new LoadSceneParameters(LoadSceneMode.Single)
        );
        yield return null;
        _goldenMovementContext = Object.FindObjectOfType<MovementContext>();
        _mockInput = _goldenMovementContext.GetComponent<MockInput>();
    }

    [UnityTest]
    public IEnumerator TestIdleToWalkStateChange()
    {
        var seconds = 1;
        var directions = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(-1, 1)
        };
        foreach (var direction in directions)
        {
            yield return Move(direction, seconds);
            var finalXPosition = direction.normalized.x * _goldenMovementContext.Speed * seconds;
            var finalYPosition = direction.normalized.y * _goldenMovementContext.Speed * seconds;
            Assert.AreApproximatelyEqual(_goldenMovementContext.transform.position.x, finalXPosition, 0.1f);
            Assert.AreApproximatelyEqual(_goldenMovementContext.transform.position.y, finalYPosition, 0.1f);
            yield return Move(-direction, seconds);
            Assert.AreApproximatelyEqual(_goldenMovementContext.transform.position.x, 0, 0.1f);
            Assert.AreApproximatelyEqual(_goldenMovementContext.transform.position.y, 0, 0.1f);
        }
    }

    private IEnumerator Move(Vector2 direction, float seconds)
    {
        _mockInput.HDirection = direction.x;
        _mockInput.VDirection = direction.y;
        yield return new WaitForSeconds(seconds);
        _mockInput.HDirection = 0;
        _mockInput.VDirection = 0;
    }
}
