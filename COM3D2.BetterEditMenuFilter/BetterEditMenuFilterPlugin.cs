using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using BepInEx;
using BepInEx.Logging;

namespace COM3D2.BetterEditMenuFilter
{
    [BepInPlugin("org.bepinex.plugins.com3d2.bettereditmenufilter", "BetterEditMenuFilter", "1.0.0.0")]
    public class BetterEditMenuFilterPlugin: BaseUnityPlugin
    {
        public static BetterEditMenuFilterPlugin Instance { get; private set; }

		public ITranslationProvider TranslationProvider { get; set; }

		public new BetterEditMenuFilterConfig Config { get; private set; }

		void Awake()
        {
            if (Instance != null) throw new Exception("Already instantiated");
            Instance = this;
            DontDestroyOnLoad(this);

			this.Config = new BetterEditMenuFilterConfig(base.Config);

			this.TranslationProvider = new DummyTranslationProvider();

			SceneManager.sceneLoaded += this.OnChangedSceneLevel;
		}

		private void OnChangedSceneLevel(Scene scenename, LoadSceneMode SceneMode)
		{
			if (scenename.name == "SceneEdit")
			{
				this.InstallMenu();
			}
		}

		private void InstallMenu()
        {
			//StopAllCoroutines();
			var itemMenu = InstallMenu<EditMenuPanelFilter>("ScrollPanel-MenuItem");
			itemMenu.Init(this.Config.ItemSearchConfig, new Vector3(-575, 520));

			var setMenu = InstallMenu<EditMenuPanelFilter>("ScrollPanel-SetItem");
			setMenu.Init(this.Config.ItemSetSearchConfig, new Vector3(-575, 520));

			var presetMenu = InstallMenu<PresetPanelFilter>("PresetPanel/PresetViewer");
			presetMenu.Init(this.Config.PresetSearchConfig, new Vector3(-575, 520));
		}

		private T InstallMenu<T>(string panelName)
			where T: MonoBehaviour
		{
			Transform transform = GameObject.Find("UI Root").transform;
			if (transform == null)
			{
				UnityEngine.Debug.Log($"Cannot find UI Root for {panelName}");
				return null;
			}

			Transform transform2 = transform.Find(panelName);
			if (transform2)
			{
				Log.LogVerbose($"Found panel {panelName}");
				GameObject gameObject = NGUITools.AddChild(transform2.gameObject);
				gameObject.name = typeof(T).Name;
				var control = gameObject.AddComponent<T>();
				return control;
			}
			return null;
		}

		internal new ManualLogSource Logger
        {
			get => base.Logger;
        }
	}
}
