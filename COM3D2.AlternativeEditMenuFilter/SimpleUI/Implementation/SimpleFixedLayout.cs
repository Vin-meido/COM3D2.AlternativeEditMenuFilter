using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;



namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleFixedLayout : BaseLayout, IFixedLayout, ILayoutComponent
    {
        internal static SimpleFixedLayout Create(BaseLayout parent)
        {
            var layout = NGUITools.AddChild<SimpleFixedLayout>(parent.gameObject);
            layout.Init(parent);
            return layout;
        }

        internal void InitRoot()
        {
            this.gameObject.name = "SimpleUIRoot";
            uiPanel = this.gameObject.AddComponent<UIPanel>();
            uiPanel.depth = 100;

            var width = UIRoot.GetPixelSizeAdjustment(this.gameObject) * Screen.width;
            var height = UIRoot.GetPixelSizeAdjustment(this.gameObject) * Screen.height;

            this._size = new Vector2(width, height);
        }

        public IAutoLayout Area(Rect rect, IAutoLayoutOptions options)
        {
            return Child<SimpleAutoLayout>(rect);
        }

        public IBox Box(Rect rect, string content)
        {
            var control = Child<SimpleBox>(rect);
            control.text = content;
            return control;
        }

        public IButton Button(Rect rect, string content, UnityAction onClick=null)
        {
            var btn = Child<SimpleButton>(rect);
            btn.text = content;
            if(onClick != null)
            {
                btn.Click.AddListener(onClick);
            }

            return btn;
        }

        public IFixedLayout Group(Rect rect)
        {
            return this.Child<SimpleFixedLayout>(rect);
        }

        public ISlider Slider(Rect rect, SimpleSlider.SliderDirection direction, float initial, float minimum, float maximum, UnityAction<float> onChange)
        {
            var c = Child<SimpleSlider>(rect);
            c.SetValues(initial, minimum, maximum);
            c.direction = direction;
            if (onChange != null)
            {
                c.AddChangeCallback(onChange);
            }
            return c;
        }

        public ISlider HorizontalSlider(Rect rect, float initial, float minimum, float maximum, UnityAction<float> onChange = null)
        {
            return Slider(rect, SimpleSlider.SliderDirection.HORIZONTAL, initial, minimum, maximum, onChange);
        }

        public ILabel Label(Rect rect, string content)
        {
            var c = Child<SimpleLabel>(rect);
            c.text = content;
            return c;
        }

        protected override void Relayout()
        {
            this.contentHeight = 0;
            this.contentWidth = 0;

            foreach (var o in this.layoutComponents)
            {
                if (!o.Visible) continue;

                o.gameObject.transform.localPosition = PostionToLocalTransform(o.position, o.size);

                var width = Mathf.FloorToInt(o.position.x + o.size.x + 0.5f);
                var height = Mathf.FloorToInt(o.position.y + o.size.y + 0.5f);

                if (width > this.contentWidth) this.contentWidth = width;
                if (height > this.contentHeight) this.contentHeight = height;
            }
        }

        public IScrollView ScrollView(Rect rect, Vector2 inner)
        {
            throw new NotImplementedException();
        }

        public ISelectionGrid SelectionGrid(Rect rect, int initial, string[] selectionStrings, int columns, UnityAction<int> onChange = null)
        {
            throw new NotImplementedException();
        }

        public ITextArea TextArea(Rect rect, string initial, UnityAction<string> onChange = null)
        {
            throw new NotImplementedException();
        }

        public ITextField TextField(Rect rect, string initial, UnityAction<string> onChange = null)
        {
            var control = this.Child<SimpleTextField>(rect);
            control.text = initial;
            return control;
        }

        public IToggle Toggle(Rect rect, string label, bool initial, UnityAction<bool> onChange = null)
        {
            var toggle = this.Child<SimpleToggle>(rect);
            toggle.text = label;
            toggle.Value = initial;
            return toggle;
        }

        public IToolbar Toolbar(Rect rect, int initial, string[] choices, UnityAction<int> onChange = null)
        {
            var toolbar = this.Child<SimpleToolbar>(rect);
            toolbar.Choices = choices;
            toolbar.Value = initial;

            if(onChange != null)
            {
                toolbar.AddChangeCallback(onChange);
            }
            return toolbar;
        }

        public ISlider VerticalSlider(Rect rect, float initial, float minimum, float maximum, UnityAction<float> onChange = null)
        {
            return Slider(rect, SimpleSlider.SliderDirection.HORIZONTAL, initial, minimum, maximum, onChange);
        }
    }
}
