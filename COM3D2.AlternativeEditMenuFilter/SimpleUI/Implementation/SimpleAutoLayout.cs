using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;
using UnityEngine.Events;



namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleAutoLayout : BaseLayout, IAutoLayout
    {
        public int spacing { get; set; } = 10;

        LayoutDirection _layoutDirection = LayoutDirection.HORIZONTAL;
        public LayoutDirection layoutDirection
        {
            get => _layoutDirection;
            set
            {
                _layoutDirection = value;
                _dirty = true;
            }
        }

        public IAutoLayout Area(Vector2 size, IAutoLayoutOptions options)
        {
            return Child<SimpleAutoLayout>(size);
        }

        public IBox Box(Vector2 size, string content)
        {
            var box = Child<SimpleBox>(size);
            box.text = content;
            return box;
        }

        public IButton Button(Vector2 size, string content)
        {
            var btn = Child<SimpleButton>(size);
            btn.text = content;
            return btn;
        }

        public IButton Button(Vector2 size, string content, UnityAction callback=null)
        {
            var btn = Child<SimpleButton>(size);
            btn.text = content;
            if (callback != null)
            {
                btn.Click.AddListener(callback);
            }
            return btn;
        }

        public IFixedLayout Group(Vector2 size)
        {
            return  Child<SimpleFixedLayout>(size);
        }

        public ISlider Slider(Vector2 size, SimpleSlider.SliderDirection direction, float initial, float minimum, float maximum, UnityAction<float> onChange)
        {
            var c = Child<SimpleSlider>(size);
            c.SetValues(initial, minimum, maximum);
            c.direction = direction;
            if (onChange != null)
            {
                c.AddChangeCallback(onChange);
            }
            return c;

        }

        public ISlider HorizontalSlider(Vector2 size, float initial, float minimum, float maximum, UnityAction<float> onChange)
        {
            return Slider(size, SimpleSlider.SliderDirection.HORIZONTAL, initial, minimum, maximum, onChange);
        }

        public ILabel Label(Vector2 size, string content)
        {
            var c = Child<SimpleLabel>(size);
            c.text = content;
            return c;
        }

        protected override void Relayout()
        {
            float currentX = 0;
            float currentY = 0;

            this.contentWidth = 0;
            this.contentHeight = 0;

            foreach (var component in this.layoutComponents)
            {
                if (!component.Visible) continue;

                var position = new Vector2(currentX, currentY);
                component.SetPosition(position, false);
                component.gameObject.transform.localPosition = PostionToLocalTransform(
                    position, component.size);

                if (this.layoutDirection == LayoutDirection.HORIZONTAL)
                {
                    currentX += component.size.x + this.spacing;
                } else
                {
                    currentY += component.size.y + this.spacing;
                }

                var width = Mathf.FloorToInt(position.x + component.size.x + 0.5f);
                var height = Mathf.FloorToInt(position.y + component.size.y + 0.5f);

                if (width > this.contentWidth) this.contentWidth = width;
                if (height > this.contentHeight) this.contentHeight = height;
            }
        }

        public IScrollView ScrollView(Vector2 size, Vector2 inner)
        {
            throw new NotImplementedException();
        }

        public ISelectionGrid SelectionGrid(Vector2 size, int initial, string[] selectionStrings, int columns, UnityAction<int> onSelect)
        {
            throw new NotImplementedException();
        }

        public override void SetSize(Vector2 size, bool triggerLayout)
        {
            this._size = size;
            this._dirty = triggerLayout;
        }

        public ITextArea TextArea(Vector2 size, string initial, UnityAction<string> onChange)
        {
            throw new NotImplementedException();
        }

        public ITextField TextField(Vector2 size, string initial, UnityAction<string> onChange)
        {
            var control = Child<SimpleTextField>(size);
            control.Value = initial;
            if (onChange != null)
            {
                control.AddChangeCallback(onChange);
            } 
            return control;
        }

        public IToggle Toggle(Vector2 size, string content, bool initial, UnityAction<bool> onSelected=null)
        {
            var component = Child<SimpleToggle>(size);
            component.text = content;
            component.Value = initial;
            component.text = content;
            if (onSelected != null)
            {
                component.OnSelected.AddListener(onSelected);
            }

            return component;
        }

        public IToolbar Toolbar(Vector2 size, int initial, string[] toolbarStrings, UnityAction<int> onSelect)
        {
            var component = Child<SimpleToolbar>(size);
            component.Choices = toolbarStrings;
            component.Value = initial;
            if(onSelect != null)
            {
                component.AddChangeCallback(onSelect);
            }

            return component;
        }

        public ISlider VerticalSlider(Vector2 size, float initial, float minimum, float maximum, UnityAction<float> onChange)
        {
            return Slider(size, SimpleSlider.SliderDirection.VERTICAL, initial, minimum, maximum, onChange);
        }

        public enum LayoutDirection {
            VERTICAL,
            HORIZONTAL,
        }

        public IDropdown Dropdown(Vector2 size, string content, string initial, IEnumerable<string> choices, UnityAction<string> onChange = null)
        {
            var o = Child<SimpleDropdown>(size);
            o.text = content;
            o.Choices = choices;
            o.Value = initial;
            if (onChange != null)
            {
                o.AddChangeCallback(onChange);
            }
            return o;
        }

        public IGenericDropdown GenericDropdown(Vector2 size, string content, UnityAction<object> onChange = null)
        {
            var o = Child<SimpleGenericDropdown>(size);
            o.text = content;
            if(onChange != null)
            {
                o.AddChangeCallback(onChange);
            }
            return o;

        }
    }
}
