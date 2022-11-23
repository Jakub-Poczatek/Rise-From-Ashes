using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    [TestFixture]
    public class PlayerStateTest
    {
        UIController uiController;
        GameManager gameManager;

        [SetUp]
        public void Init()
        {
            GameObject gameManagerObject = new GameObject();
            var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();
            gameManagerObject.AddComponent<InputManager>();
            uiController = gameManagerObject.AddComponent<UIController>();

            GameObject buildObjectbtn = new GameObject();
            GameObject cancelObjectbtn = new GameObject();
            GameObject cancelPnl = new GameObject();

            uiController.cancelActionBtn = cancelObjectbtn.AddComponent<Button>();
            var buttonBuildComponent = buildObjectbtn.AddComponent<Button>();
            uiController.buildResidentialAreaBtn = buttonBuildComponent;
            uiController.cancelActionPnl = cancelObjectbtn;
            gameManager = gameManagerObject.AddComponent<GameManager>();
            gameManager.cameraMovement = cameraMovementComponent;
            gameManager.uiController = uiController;
        }

        [UnityTest]
        public IEnumerator PlayerBuildingSingleStructureStateTestWithEnumeratorPasses()
        {
            yield return new WaitForEndOfFrame(); // awake
            yield return new WaitForEndOfFrame(); // start
            uiController.buildResidentialAreaBtn.onClick.Invoke();
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(gameManager.PlayerState is PlayerBuildingSingleStructureState);
        }

        [UnityTest]
        public IEnumerator PlayerSelectionStateTestWithEnumeratorPasses()
        {
            yield return new WaitForEndOfFrame(); // awake
            yield return new WaitForEndOfFrame(); // start
            Assert.IsTrue(gameManager.PlayerState is PlayerSelectionState);
        }
    }
}
