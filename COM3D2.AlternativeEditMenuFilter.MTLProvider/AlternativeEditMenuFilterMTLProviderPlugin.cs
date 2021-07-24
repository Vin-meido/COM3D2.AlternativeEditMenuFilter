using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BepInEx;
using UnityEngine;
using XUnity.AutoTranslator.Plugin.Core;

namespace COM3D2.AlternativeEditMenuFilter.MTLProvider
{
    [BepInPlugin("org.bepinex.plugins.com3d2.bettereditmenufilter.mtlprovider", "BetterEditMenuFilter.MTLProvider", "1.0.0.0")]
    public class BetterEditMenuFilterMTLProviderPlugin: BaseUnityPlugin, ITranslationProvider
    {
        public static BetterEditMenuFilterMTLProviderPlugin Instance { get; private set; }

        ITranslator translator;

        void Awake()
        {
            if(Instance != null)
            {
                throw new Exception("Already initialized");
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            this.translator = Resources.FindObjectsOfTypeAll<AutoTranslationPlugin>().FirstOrDefault();
            AlternateEditMenuFilterPlugin.Instance.TranslationProvider = this;
        }

        class TResult : ITranslationResult
        {
            public string OriginalText
            {
                get; set;
            }

            public string TranslatedText
            {
                get; set;
            }

            public bool IsTranslationSuccessful
            {
                get;
                set;
            }
        }

        class AsyncTResult : TResult, ITranslationAsyncResult
        {
            public bool IsReady { get; set; }

            public void Resolve(TranslationResult r)
            {
                this.IsTranslationSuccessful = r.Succeeded;
                this.TranslatedText = r.TranslatedText;
                this.IsReady = true;
            }
        }

        public ITranslationResult Translate(string text)
        {
            string translatedText;
            bool translated = translator.TryTranslate(text, out translatedText);
            return new TResult
            {
                OriginalText = text,
                TranslatedText = translatedText,
                IsTranslationSuccessful = translated
            };
        }

        public ITranslationAsyncResult TranslateAsync(string text)
        {
            var result = new AsyncTResult
            {
                OriginalText = text,
                IsReady = false
            };
            translator.TranslateAsync(text, result.Resolve);
            return result;
        }

    }

}
