using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

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
            uiController = gameManagerObject.AddComponent<UIController>();

            GameObject buildObjectbtn = new GameObject();
            GameObject cancelObjectbtn = new GameObject();
            GameObject cancelPnl = new GameObject();
            GameObject closeBuildMenuBtn = new GameObject();

            uiController.cancelActionBtn = cancelObjectbtn.AddComponent<Button>();
            var buttonBuildComponent = buildObjectbtn.AddComponent<Button>();
            uiController.buildResidentialAreaBtn = buttonBuildComponent;
            uiController.cancelActionPnl = cancelObjectbtn;
            uiController.buildingMenuPnl = cancelPnl;
            uiController.openBuildMenuBtn = uiController.cancelActionBtn;
            uiController.demolishBtn = uiController.cancelActionBtn;
            closeBuildMenuBtn.AddComponent<Button>();
            uiController.closeBuildMenuBtn = closeBuildMenuBtn.GetComponent<Button>();

            gameManager = gameManagerObject.AddComponent<GameManager>();
            gameManager.cameraMovement = cameraMovementComponent;
            gameManager.uiController = uiController;

            GameObject goldAmountTxt = new GameObject();
            goldAmountTxt.AddComponent<TextMeshPro>();
            uiController.goldAmountTxt = goldAmountTxt.GetComponent<TMP_Text>();
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
