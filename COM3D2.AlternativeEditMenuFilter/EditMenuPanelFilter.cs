using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;

using COM3D2.SimpleUI;
using COM3D2.SimpleUI.Extensions;
using System.Globalization;

namespace COM3D2.AlternativeEditMenuFilter
{
    class PendingTranslation
    {
        public EditMenuPanelItem Item { get; set; }
        public ITranslationAsyncResult Result { get; set; }
    }

    public class EditMenuPanelFilter : MonoBehaviour
    {
        EditMenuPanelController controller;

        string search = "";
        bool updateItemListQueued = false;

        IButton caseSensitivityButton;
        IButton termIncludeButton;
        ITextField searchTextField;
        IDropdown historyDropdown;
        IGenericDropdown itemTypeFilterDropdown;

        readonly List<string> History = new List<string>();

        MenuSearchConfig config;

        bool SearchInName { get => SearchTextMode == SearchTextModeEnum.NAME || SearchTextMode == SearchTextModeEnum.ALL; }
        bool SearchInInfo { get => SearchTextMode == SearchTextModeEnum.DESCRIPTION || SearchTextMode == SearchTextModeEnum.ALL; }

        bool SearchInFilename { get => SearchTextMode == SearchTextModeEnum.FILENAME || SearchTextMode == SearchTextModeEnum.ALL; }
        bool SearchInDirectoryName { get => SearchTextMode == SearchTextModeEnum.DIRECTORYNAME || SearchTextMode == SearchTextModeEnum.ALL; }

        bool SearchLocalized { 
            get => config.SearchLocalized.Value;
            set => config.SearchLocalized.Value = value; }

        bool SearchMTL { 
            get => SearchLocalized && config.SearchMTL.Value;}

        bool SendMTL
        {
            get => SearchMTL && config.SendMTL.Value;
        }

        bool SearchAllTerms {
            get => config.SearchAllTerms.Value;
            set => config.SearchAllTerms.Value = value; }

        bool IncludeMods { 
            get => ItemTypeFilter == ItemTypeFilterEnum.MOD || ItemTypeFilter == ItemTypeFilterEnum.ALL; }

        bool IncludeVanilla {
            get => ItemTypeFilter == ItemTypeFilterEnum.VANILLA || ItemTypeFilter == ItemTypeFilterEnum.ALL;
        }

        bool IncludeCompat { 
            get => ItemTypeFilter == ItemTypeFilterEnum.COMPAT || ItemTypeFilter == ItemTypeFilterEnum.ALL;
        }

        bool IgnoreCase { 
            get => config.IgnoreCase.Value;
            set => config.IgnoreCase.Value = value; }

        ItemTypeFilterEnum ItemTypeFilter {
            get => config.ItemTypeFilter.Value;
            set => config.ItemTypeFilter.Value = value; 
        }

        SearchTextModeEnum SearchTextMode { 
            get => config.SearchTextMode.Value;
            set => config.SearchTextMode.Value = value; }

        ITranslationProvider TranslationProvider
        {
            get => AlternateEditMenuFilterPlugin.Instance.TranslationProvider;
        }

        readonly List<PendingTranslation> pendingTranslations = new List<PendingTranslation>();

        void Awake()
        {
        }

        public void Init(MenuSearchConfig config, Vector3 localPosition)
        {
            this.controller = new EditMenuPanelController(this.gameObject.transform.parent.gameObject);
            this.config = config;
            this.gameObject.transform.localPosition = localPosition;

            this.History.AddRange(
                this.config.History.Value
                .Split('\n')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s)));
        }

        void Start()
        {
            Assert.IsNotNull(this.config, "Not properly initialized");
            this.BuildUI();
        }

        void OnEnable()
        {
            this.QueueUpdateItemList();
        }

        void OnDisable()
        {
            Log.LogVerbose("Saving search history");
            this.config.History.Value = String.Join("\n", this.History.ToArray());
        }

        void Update()
        {
            if(updateItemListQueued)
            {
                updateItemListQueued = false;
                this.TranslationProvider.ResetAsyncQueue();
                UpdateItemList();
            }
        }

        void BuildUI()
        {
            var panelHeight = 40;

            var ui = SimpleUIRoot.Create(this.gameObject, 0, 0);

            var box = ui.Box(new Rect(0, 0, 1050, panelHeight + 16), "");

            var area = ui.Area(new Rect(8, 8, 1000, panelHeight), null);
            area.OnLayout(() =>
            {
                box.size = new Vector2(area.contentWidth + 16, area.contentHeight + 16);
            });

            historyDropdown = area.Dropdown(
                new Vector2(50, panelHeight),
                "Hist", null,
                this.History,
                this.OnHistoryDropdownChange);

            searchTextField = area.TextField(new Vector2(250, panelHeight), "");
            searchTextField.AddSubmitCallback(this.SearchTextfieldSubmit);

            area.Button(new Vector2(70, panelHeight), "Reset", this.ResetButtonClick);

            caseSensitivityButton = area.Button(new Vector2(50, panelHeight), "Aa", this.CaseSensitivityButtonClick);

            termIncludeButton = area.Button(new Vector2(50, panelHeight), "Or", this.TermIncludeButtonClick);

            area.GenericDropdown(new Vector2(50, panelHeight), "ALL")
                .Choice(SearchTextModeEnum.ALL, "[All] text", "All")
                .Choice(SearchTextModeEnum.NAME, "[Name]", "Name")
                .Choice(SearchTextModeEnum.DESCRIPTION, "[Info]", "Info")
                .Choice(SearchTextModeEnum.FILENAME, "[Fn] Filename", "Fn")
//                .Choice(SearchTextModeEnum.DIRECTORYNAME, "[Dir] Full directory path", "Dir")
                .SetValue(this.SearchTextMode)
                .SetUpdateTextOnValuechange(true)
                .AddChangeCallback(o => { 
                    if(o is SearchTextModeEnum mode)
                    {
                        this.SearchTextMode = mode;
                        this.QueueUpdateItemList();
                        Log.LogVerbose("New mode is {0}", mode);
                    }
                });

            itemTypeFilterDropdown = area.GenericDropdown(new Vector2(50, panelHeight), "All")
                .Choice(ItemTypeFilterEnum.ALL, "[All] items", "All")
                .Choice(ItemTypeFilterEnum.VANILLA, "[COM] Vanilla COM3D2", "COM")
                .Choice(ItemTypeFilterEnum.COMPAT, "[CM] Compat/CM", "CM")
                .Choice(ItemTypeFilterEnum.MOD, "[Mod]s", "Mod")
                .SetValue(this.ItemTypeFilter)
                .SetUpdateTextOnValuechange(true);

            itemTypeFilterDropdown.AddChangeCallback(o =>
                {
                    if(o is ItemTypeFilterEnum t)
                    {
                        this.ItemTypeFilter = t;
                        this.QueueUpdateItemList();
                        Log.LogVerbose("New filter type is {0}", t);
                    }
                });

            UpdateToggles();
        }

        private void OnHistoryDropdownChange(string selected)
        {
            searchTextField.Value = selected;
            SearchTextfieldSubmit(selected);
        }

        private void TermIncludeButtonClick()
        {
            this.SearchAllTerms = !this.SearchAllTerms;
            UpdateToggles();
            this.QueueUpdateItemList();
        }

        private void CaseSensitivityButtonClick()
        {
            this.IgnoreCase = !this.IgnoreCase;
            UpdateToggles();
            this.QueueUpdateItemList();
        }

        private void UpdateToggles()
        {
            if (this.SearchAllTerms)
            {
                this.termIncludeButton.text = "AND";
            }
            else
            {
                this.termIncludeButton.text = "OR";
            }

            if (this.IgnoreCase)
            {
                this.caseSensitivityButton.defaultColor = this.caseSensitivityButton.hoverColor = Color.white;
            }
            else
            {
                this.caseSensitivityButton.defaultColor = this.caseSensitivityButton.hoverColor = Color.gray;
            }
        }

        void SearchTextfieldSubmit(string terms)
        {
            terms = terms.Trim();
            this.search = terms;
            if (terms == "")
            {
                this.ResetButtonClick();
                return;
            }
            AddToHistory(terms);
            this.QueueUpdateItemList();
        }

        void AddToHistory(string terms)
        {
            var index = this.History.IndexOf(terms);
            if (index >= 0)
            {
                this.History.RemoveAt(index);
            }

            this.History.Insert(0, terms);

            var maxHistory = this.config.MaxHistory.Value;
            if (this.History.Count > maxHistory)
            {
                this.History.RemoveRange(maxHistory, this.History.Count - maxHistory);
            }

            this.historyDropdown.Choices = this.History;
        }

        void QueueUpdateItemList()
        {
            updateItemListQueued = true;
            controller.HidePanel();
        }

        void UpdateItemList() { 
            var termList = this.search
                .Split(' ')
                .Where(t => !string.IsNullOrEmpty(t))
                .Select(t => t.Trim())
                .ToArray();

            Log.LogVerbose("Clearing pending translations");
            this.pendingTranslations.Clear();
            this.TranslationProvider.ResetAsyncQueue();

            Log.LogVerbose("Performing filter");
            foreach (var item in controller.GetAllItems())
            {
                var matchesTermList = termList.Length == 0 || FilterItem(item, termList);
                item.Visible = matchesTermList && FilterType(item);
            }

            controller.ShowPanel();
            controller.ResetView();
        }

        bool FilterItem(EditMenuPanelItem item, string[] termList)
        {
            var inName = SearchInName;
            var inInfo = SearchInInfo;
            var inFilename = SearchInFilename;
            var inDirectoryname = SearchInDirectoryName;
            var translatedNameAvailable = false;
            var translatedInfoAvailable = false;

            if (SearchInName)
            {
                var inOriginalName = StringContains(item.Name, termList);
                var inLocalizedName = SearchLocalized && StringContains(item.LocalizedName, termList);
                var inTranslatedName = SearchMTL && TranslationContains(item.Name, termList, out translatedNameAvailable);
                inName = inOriginalName || inLocalizedName || inTranslatedName;
            }

            if (SearchInInfo)
            {
                var inOriginalInfo = StringContains(item.Info, termList);
                var inLocalizedInfo = SearchLocalized && StringContains(item.LocalizedInfo, termList);
                var inTranslatedInfo = SearchMTL && TranslationContains(item.Info, termList, out translatedInfoAvailable);
                inInfo = inOriginalInfo || inLocalizedInfo || inTranslatedInfo;
            }

            if (SearchInFilename)
            {
                inFilename = StringContains(item.Filename, termList);
            }

            if(SearchInDirectoryName)
            {
                var directoryName = this.GetDirectoryName(item.Filename);
                inDirectoryname = StringContains(directoryName, termList);
            }

            return inName || inInfo || inFilename || inDirectoryname;
        }

        void QueueTranslation(EditMenuPanelItem item, string text)
        {
            if (SendMTL)
            {
                Log.LogVerbose("Sending string for translation: {0}", text);
                this.pendingTranslations.Add(new PendingTranslation()
                {
                    Item = item,
                    Result = TranslationProvider.TranslateAsync(text)
                });
            }
        }

        bool FilterType(EditMenuPanelItem item)
        {
            if (!IncludeMods && item.IsMod)
            {
                Log.LogVerbose("Hiding {0}, mods excluded", item.Filename);
                return false;
            }

            if (!IncludeCompat && item.IsCompat)
            {
                Log.LogVerbose("Hiding {0}, compat excluded", item.Filename);
                return false;
            }

            if (!IncludeVanilla && item.IsVanilla && !item.IsCompat)
            {
                Log.LogVerbose("Hiding {0}, vanilla excluded", item.Filename);
                return false;
            }

            return true;
        }

        private string GetDirectoryName(string filename)
        {
            // TODO: Implement this
            return "";
        }

        bool StringContains(string str, string[] terms)
        {
            foreach(var term in terms)
            {
                if (StringContains(str, term))
                {
                    if(!SearchAllTerms)
                    {
                        return true;
                    }

                } else if(SearchAllTerms)
                {
                    return false;
                }
            }

            return SearchAllTerms;
        } 

        bool StringContains(string str, string term)
        {
            if(IgnoreCase)
            {
                CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                int num = compareInfo.IndexOf(str, term, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
                return (num >= 0);
            }

            return str.IndexOf(term) >= 0;
        }

        bool TranslationContains(string str, string[] terms, out bool available)
        {
            var result = TranslationProvider.Translate(str);
            if(result.IsTranslationSuccessful)
            {
                available = true;
                return StringContains(result.TranslatedText, terms);

            }
            available = false;
            return false;
        }

        void ResetButtonClick()
        {
            this.search = "";
            this.searchTextField.Value = "";
            this.ItemTypeFilter = ItemTypeFilterEnum.ALL;
            this.itemTypeFilterDropdown.SetValue(this.ItemTypeFilter);
            this.TranslationProvider.ResetAsyncQueue();
            controller.ShowAll();
            controller.ResetView();
        }

        public enum SearchTextModeEnum
        {
            ALL,
            NAME,
            DESCRIPTION,
            FILENAME,
            DIRECTORYNAME,
        }

        public enum ItemTypeFilterEnum
        {
            ALL,
            VANILLA,
            COMPAT,
            MOD
        }
    }
}
