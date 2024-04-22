using System.Collections;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

using Nebula;
using Nebula.UI;
using Nebula.Utils;

namespace UITest {
    [BepInPlugin (PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency ("Nebula")]
    [BepInProcess ("dyingsun.exe")]
    public class Plugin : BaseUnityPlugin {
        private void Awake () {
            // Plugin startup logic
            NebulaPlugin.SceneReady += LoadUI;
        }

        private void LoadUI (Scene scene) {
            NebulaPlugin.SceneReady -= LoadUI;

            Logger.LogInfo ($"Loading UI elements...");
            
            Vector3 buttonSize = new Vector3 (352, 24, 0);
            int fontSize = 20;

            string menuRoot = "GATE_MainMenu/ANCHOR_MainMenu/PANEL_MainMenu/ROOT_MainMenu";
            string buttonsPath = "ROOT_Left/010_BUTTONS";

            List<EventDelegate> delegates = new List<EventDelegate> ();
            EventDelegate.Add (delegates, HelloWorld);

            ButtonDetailDatum detail = new ButtonDetailDatum (Color.cyan);
            ButtonFactoryDatum factoryDatum = new ButtonFactoryDatum ("HelloWorld", "HELLO WORLD", delegates, fontSize, buttonSize, menuRoot, buttonsPath, detail);

            UIButton button = ButtonFactory.Create (factoryDatum);
            MOTDDatum motdDatum = new MOTDDatum ("Prints \"Hello world!\" to the console.");
            ButtonDatum buttonDatum = new ButtonDatum (button, menuRoot, 21, motdDatum);

            ButtonSpawner.buttonQueue.Enqueue (buttonDatum);
        }

        private void HelloWorld () {
            Logger.LogInfo ("Hello world!");
        }
    }
}
