using System.Collections;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UITest {
    [BepInPlugin (PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess ("dyingsun.exe")]
    public class Plugin : BaseUnityPlugin {
        private void Awake () {
            // Plugin startup logic
            Logger.LogInfo ($"Plugin {PluginInfo.PLUGIN_GUID} welcomes you with a lot of logging");
            SceneManager.sceneLoaded += SceneLoadHook;
        }

        private void SceneLoadHook (Scene scene, LoadSceneMode mode) {
            SceneManager.sceneLoaded -= SceneLoadHook;
            Logger.LogInfo ($"Caught scene loading: {scene.name}; starting coroutine");
            StartCoroutine (LoadUI (scene));
        }

        private IEnumerator LoadUI (Scene scene) {
            yield return null;          // Wait one frame for the scene to fully load
            Logger.LogInfo ($"Loading UI elements...");
            
            GameObject goRoot = null;   // Using a master variable instead of multiple variables that get discarded afterward
            UIButton button = null;
            Vector3 buttonSize = new Vector3 (352, 24, 0);

            GameObject uiRoot = scene.GetRootGameObjects ().GetGameObject ("# CUI_2D");
            Transform menuRoot = uiRoot.transform.FindChild ("Camera/ROOT_Menus");

            Transform mainRoot = menuRoot.FindChild ("GATE_MainMenu/ANCHOR_MainMenu/PANEL_MainMenu/ROOT_MainMenu");
            UILabel motd = mainRoot.FindChild ("WIDGET_MOTD/PANEL_ScrollWindow/010_LABEL_BlockText").GetComponent<UILabel> ();
            goRoot = mainRoot.FindChild ("ROOT_Left/010_BUTTONS").gameObject;

            List<EventDelegate> delegates = new List<EventDelegate> ();
            EventDelegate.Add (delegates, HelloWorld);
            
            button = goRoot.CreateButton (buttonSize, "HelloWorld", "HELLO WORLD", delegates, 20, Color.cyan);
            button.name = $"021_{button.name}"; // Places the button below "PLAY"

            mainRoot.GetComponent<CUIMainMenu> ().AddTooltip (motd, button.gameObject, "Prints \"Hello world!\" to the console.");
        }

        private void HelloWorld () {
            Logger.LogInfo ("Hello world!");
        }
    }
}
