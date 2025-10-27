using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

namespace COM3D2.SimpleUI
{
    public interface IAutoLayout: IControlContainer, ILayout
    {
        IBox Box(Vector2 size, string content);

        IButton Button(Vector2 size, string content, UnityAction onClick = null);

        ILabel Label(Vector2 size, string content);

        ITextField TextField(Vector2 size, string initial, UnityAction<string> onChange = null);

        ITextArea TextArea(Vector2 size, string initial, UnityAction<string> onChange = null);

        IToggle Toggle(Vector2 size, string content, bool initial, UnityAction<bool> onChange = null);

        IToolbar Toolbar(Vector2 size, int initial, string[] toolbarStrings, UnityAction<int> onChange = null);

        ISelectionGrid SelectionGrid(Vector2 size, int initial, string[] selectionStrings, int columns, UnityAction<int> onChange = null);

        ISlider HorizontalSlider(Vector2 size, float initial, float minimum, float maximum, UnityAction<float> onChange = null);

        ISlider VerticalSlider(Vector2 size, float initial, float minimum, float maximum, UnityAction<float> onChange = null);

        IDropdown Dropdown(Vector2 size, string content, string initial, IEnumerable<string> choices, UnityAction<string> onChange = null);

        IGenericDropdown GenericDropdown(Vector2 size, string content, UnityAction<object> onChange = null);

        IScrollView ScrollView(Vector2 size, Vector2 inner);

        IFixedLayout Group(Vector2 size);

        IAutoLayout Area(Vector2 size, IAutoLayoutOptions options);
    }
}
