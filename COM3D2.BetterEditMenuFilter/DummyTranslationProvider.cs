using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.BetterEditMenuFilter
{
    class DummyTranslationResult : ITranslationResult, ITranslationAsyncResult
    {
        public string OriginalText { get; set; }

        public string TranslatedText { get; set; }

        public bool IsTranslationSuccessful { get; set; }

        public bool IsReady {
            get; set;
        }
    }

    class DummyTranslationProvider : ITranslationProvider
    {
        public ITranslationResult Translate(string text)
        {
            return new DummyTranslationResult
            {
                OriginalText = text,
                IsTranslationSuccessful = false,
                IsReady = true,
            };
        }

        public ITranslationAsyncResult TranslateAsync(string text)
        {
            return new DummyTranslationResult
            {
                OriginalText = text,
                IsTranslationSuccessful = false,
                IsReady = true,
            };
        }
    }
}
