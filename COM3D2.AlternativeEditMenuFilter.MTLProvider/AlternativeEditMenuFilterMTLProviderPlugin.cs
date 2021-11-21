using System;
using System.Collections;
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
        readonly Queue<AsyncTResult> queue = new Queue<AsyncTResult>();

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
            this.translator = AutoTranslator.Default;
            AlternateEditMenuFilterPlugin.Instance.TranslationProvider = this;
            //StartCoroutine(this.StartTranslationCoroutine());
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

            public override string ToString()
            {
                return $"succeeded:{IsTranslationSuccessful}\n\ttext:{OriginalText}\n\ttranslated:{TranslatedText}";
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
#if DEBUG
                LogVerbose($"AsyntTranslationResolved: {this}");
#endif
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
            translator.TranslateAsync(result.OriginalText, result.Resolve);
            //this.queue.Enqueue(result);
            return result;
        }

        public void ResetAsyncQueue()
        {
            //this.StopAllCoroutines();
            //this.queue.Clear();
            //this.StartCoroutine(this.StartTranslationCoroutine());
        }

        IEnumerator StartTranslationCoroutine()
        {
            while(true)
            {
                yield return new WaitUntil(() => this.queue.Count > 0);

                var r = this.queue.Dequeue();
                var complete = false;
#if DEBUG
                LogVerbose("Translating: {r.OriginalText}");
#endif
                translator.TranslateAsync(r.OriginalText, (t) => {
                    complete = true;
                    r.Resolve(t);
                });

                yield return new WaitUntil(() => complete);
            }
        }

        static void LogVerbose(object obj)
        {
#if DEBUG
            Instance.Logger.LogInfo(obj);
#endif
        }

    }

}
