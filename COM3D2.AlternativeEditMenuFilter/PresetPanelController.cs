using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.AlternativeEditMenuFilter
{
    public class PresetPanelController
    {
        UIScrollView m_scrollView;
        Transform m_gridTableTrans;
        UIScrollBar m_scrollBar;
        SceneEdit m_sceneEdit;
        UITable m_table;

        public PresetPanelController(GameObject go)
        {
            this.m_scrollView = go.GetComponentInChildren<UIScrollView>(false);
            Assert.IsNotNull(m_scrollView, $"Could not find UIScrollView for {go}");

            this.m_table = this.m_scrollView.GetComponentInChildren<UITable>(false);
            Assert.IsNotNull(this.m_table, $"Could not find UITable for {go}");

            this.m_gridTableTrans = this.m_table.transform;

            this.m_sceneEdit = GameObject.Find("__SceneEdit__").GetComponent<SceneEdit>();
            Assert.IsNotNull(m_sceneEdit, $"Could not find SceneEdit");

            this.m_scrollBar = go.GetComponentInChildren<UIScrollBar>(false);
            Assert.IsNotNull(m_scrollBar, $"Could not find UIScrollBar for {go}");
        }


/*        private void AddLabels()
        {
            Font font = GameObject.Find("SystemUI Root").GetComponentsInChildren<UILabel>()[0].trueTypeFont;
            foreach(var presetItem in this.GetAllItems())
            {
                var item = presetItem.gameObject;
                UITexture component = item.GetComponent<UITexture>();
                if (component)
                {
                    UILabel uilabel = NGUITools.AddChild<UILabel>(item);
                    if (uilabel)
                    {
                        uilabel.trueTypeFont = font;
                        uilabel.fontSize = 22;
                        uilabel.width = component.width;
                        uilabel.height = component.height;
                        uilabel.pivot = UIWidget.Pivot.TopLeft;
                        uilabel.overflowMethod = UILabel.Overflow.ResizeHeight;
                        uilabel.effectStyle = UILabel.Effect.Outline;
                        uilabel.text = "[00FF00]" + item.name.Replace(".preset", "");
                        uilabel.depth = component.depth + 2;
                    }
                }

            }
        }
*/

        private bool _isCurrentActivePreset(Transform trans)
        {
            if (trans == null)
            {
                return false;
            }
            if (PresetMgr.m_currentActiveFilterBtnName == PresetMgr.Filter.NotFilter)
            {
                return true;
            }
            UISprite getComponentInChildren = trans.GetComponentInChildren<UISprite>(true);
            Assert.IsNotNull(getComponentInChildren, "could not find identifier");
            if (getComponentInChildren)
            {
                switch (PresetMgr.m_currentActiveFilterBtnName)
                {
                    case PresetMgr.Filter.All:
                        return (
                            getComponentInChildren.spriteName.EndsWith("kindicon_clothes_body") ||
                            getComponentInChildren.spriteName.EndsWith("kindicon_clothes_body_en")
                        );
                    case PresetMgr.Filter.Wear:
                        return (
                            getComponentInChildren.spriteName.EndsWith("kindicon_clothes") ||
                            getComponentInChildren.spriteName.EndsWith("kindicon_clothes_en")
                        );
                    case PresetMgr.Filter.Body:
                        return (
                            getComponentInChildren.spriteName.EndsWith("kindicon_body") ||
                            getComponentInChildren.spriteName.EndsWith("kindicon_body_en")
                        );
                }
            }
            return false;
        }

        public IEnumerable<PresetPanelItem> GetAllItems()
        {

            return (from i in Enumerable.Range(0, this.m_gridTableTrans.childCount)
                    select this.m_gridTableTrans.GetChild(i) into item
                    where _isCurrentActivePreset(item)
                    select new PresetPanelItem(item.gameObject));
        }

        public void ResetView()
        {
            this.m_table.Reposition();
            this.m_scrollView.ResetPosition();
            this.m_scrollBar.value = 0f;
        }

        public void ShowAll()
        {
            foreach (var r in GetAllItems())
            {
                if (!r.gameObject.activeSelf)
                {
                    r.gameObject.SetActive(true);
                }
            }
        }

        public void ShowLabels()
        {
            foreach(var item in GetAllItems())
            {
                item.ShowName();
            }
        }

        public void HideLabels()
        {
            foreach (var item in GetAllItems())
            {
                item.HideName();
            }

        }

    }
}
