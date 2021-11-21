using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;


using COM3D2.SimpleUI;
using COM3D2.SimpleUI.Extensions;

namespace COM3D2.AlternativeEditMenuFilter
{
    public class PresetPanelFilter : MonoBehaviour
    {
        PresetPanelController controller;

        string search = "";
        bool updateItemListQueued = false;
        bool showNames = false;

        IButton caseSensitivityButton;
        IButton termIncludeButton;
        IButton showNamesButton;
        ITextField searchTextField;
        IDropdown historyDropdown;

        readonly List<string> History = new List<string>();

        PresetSearchConfig config;

        bool SearchAllTerms
        {
            get => config.SearchAllTerms.Value;
            set => config.SearchAllTerms.Value = value;
        }

        bool IgnoreCase
        {
            get => config.IgnoreCase.Value;
            set => config.IgnoreCase.Value = value;
        }

        void Awake()
        {
        }

        public void Init(PresetSearchConfig config, Vector3 localPosition)
        {
            this.controller = new PresetPanelController(this.gameObject.transform.parent.gameObject);
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

        void OnDisable()
        {
            Log.LogVerbose("Saving search history");
            this.config.History.Value = String.Join("\n", this.History.ToArray());
        }

        void OnEnable()
        {
            this.QueueUpdateItemList();
        }

        void Update()
        {
            if (updateItemListQueued)
            {
                updateItemListQueued = false;
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

            showNamesButton = area.Button(new Vector2(50, panelHeight), "Name", this.ShowNamesButtonClick);
        }

        private void ShowNamesButtonClick()
        {
            showNames = !showNames;
            if(showNames)
            {
                controller.ShowLabels();
            } else
            {
                controller.HideLabels();
            }
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

            if(this.showNames)
            {
                showNamesButton.defaultColor = showNamesButton.hoverColor = Color.white;
            }
            else
            {
                showNamesButton.defaultColor = showNamesButton.hoverColor = Color.gray;
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
        }

        void UpdateItemList()
        {
            var termList = this.search
                .Split(' ')
                .Where(t => !string.IsNullOrEmpty(t))
                .Select(t => t.Trim())
                .ToArray();

            if (termList.Length == 0)
            {
                controller.ShowAll();
                controller.ResetView();
                return;
            }

            Log.LogVerbose("Performing filter");

            foreach (var item in controller.GetAllItems())
            {
                FilterItem(item, termList);
            }

            controller.ResetView();
        }

        void FilterItem(PresetPanelItem item, string[] termList)
        {
            var inName = StringContains(item.Name, termList);
            if (!inName)
            {
                item.Visible = false;
                return;
            }

            item.Visible = true;
        }

        bool StringContains(string str, string[] terms)
        {
            foreach (var term in terms)
            {
                if (StringContains(str, term))
                {
                    if (!SearchAllTerms)
                    {
                        return true;
                    }

                }
                else if (SearchAllTerms)
                {
                    return false;
                }
            }

            return SearchAllTerms;
        }

        bool StringContains(string str, string term)
        {
            if (IgnoreCase)
            {
                CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                int num = compareInfo.IndexOf(str, term, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
                return (num >= 0);
            }

            return str.IndexOf(term) >= 0;
        }


        void ResetButtonClick()
        {
            this.search = "";
            this.searchTextField.Value = "";
            controller.ShowAll();
            controller.ResetView();
        }
    }
}
