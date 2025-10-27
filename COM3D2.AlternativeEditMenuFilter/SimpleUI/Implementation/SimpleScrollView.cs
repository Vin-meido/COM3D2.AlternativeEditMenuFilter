using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleScrollView: SimpleControl, IFixedLayout, IScrollView
    {
        SimpleFixedLayout innerLayout;
        UIScrollView uiScrollView;
        UIPanel uiPanel;
        UIScrollBar uiScrollBar;

        #region IFixedLayout innerLayout wrapper

        public int width => innerLayout.width;

        public int height => innerLayout.height;

        public int contentWidth => innerLayout.contentWidth;

        public int contentHeight => innerLayout.contentHeight;

        public bool Draggable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddLayoutCallback(UnityAction callback)
        {
            innerLayout.AddLayoutCallback(callback);
        }

        public IAutoLayout Area(Rect rect, IAutoLayoutOptions options)
        {
            return innerLayout.Area(rect, options);
        }

        public IBox Box(Rect rect, string content)
        {
            return innerLayout.Box(rect, content);
        }

        public IButton Button(Rect rect, string content, UnityAction onClick = null)
        {
            return innerLayout.Button(rect, content, onClick);
        }

        public IEnumerable<T> GetChildren<T>() where T : IControl
        {
            return innerLayout.GetChildren<T>();
        }

        public IEnumerable<IControl> GetChildren()
        {
            return innerLayout.GetChildren();
        }

        public IFixedLayout Group(Rect rect)
        {
            return innerLayout.Group(rect);
        }

        public ISlider HorizontalSlider(Rect rect, float initial, float minimum, float maximum, UnityAction<float> onChange = null)
        {
            return innerLayout.HorizontalSlider(rect, initial, minimum, maximum, onChange);
        }

        public ILabel Label(Rect rect, string content)
        {
            return innerLayout.Label(rect, content);
        }

        public void RemoveLayoutCallback(UnityAction callback)
        {
            innerLayout.RemoveLayoutCallback(callback);
        }

        public IScrollView ScrollView(Rect rect, Vector2 inner)
        {
            return innerLayout.ScrollView(rect, inner);
        }

        public ISelectionGrid SelectionGrid(Rect rect, int initial, string[] selectionStrings, int columns, UnityAction<int> onChange = null)
        {
            return innerLayout.SelectionGrid(rect, initial, selectionStrings, columns, onChange);
        }

        public ITextArea TextArea(Rect rect, string initial, UnityAction<string> onChange = null)
        {
            return innerLayout.TextArea(rect, initial, onChange);
        }

        public ITextField TextField(Rect rect, string initial, UnityAction<string> onChange = null)
        {
            return innerLayout.TextField(rect, initial, onChange);
        }

        public IToggle Toggle(Rect rect, string content, bool initial, UnityAction<bool> onChange = null)
        {
            return innerLayout.Toggle(rect, content, initial, onChange);
        }

        public IToolbar Toolbar(Rect rect, int initial, string[] toolbarStrings, UnityAction<int> onChange = null)
        {
            return innerLayout.Toolbar(rect, initial, toolbarStrings, onChange);
        }

        public ISlider VerticalSlider(Rect rect, float initial, float minimum, float maximum, UnityAction<float> onChange = null)
        {
            return innerLayout.VerticalSlider(rect, initial, minimum, maximum, onChange);
        }

        #endregion

        public override void InitControl()
        {
            SetupInnerLayout();
            SetupScrollBar();
            SetupPanel();

            uiScrollView = innerLayout.gameObject.AddComponent<UIScrollView>();
            uiScrollView.horizontalScrollBar = uiScrollBar;
        }

        protected void SetupInnerLayout()
        {
            innerLayout = NGUITools.AddChild<SimpleFixedLayout>(this.gameObject);
            innerLayout.Init(null);
            innerLayout.SetSize(this.size, false);
        }

        protected void SetupScrollBar()
        {
            uiScrollBar = NGUITools.AddChild<UIScrollBar>(this.gameObject);
        }

        protected void SetupPanel()
        {
            uiPanel = innerLayout.gameObject.GetComponent<UIPanel>();
            uiPanel.clipping = UIDrawCall.Clipping.SoftClip;
            uiPanel.baseClipRegion = new Vector4(0, 0, this.width, this.height);
        }

        public override void UpdateUI()
        {
            //throw new NotImplementedException();
        }
    }
}
