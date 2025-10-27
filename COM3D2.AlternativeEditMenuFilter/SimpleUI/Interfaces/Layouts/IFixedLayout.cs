using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

namespace COM3D2.SimpleUI
{
    public interface IFixedLayout: IControlContainer, ILayout
    {
        IBox Box(Rect rect, string content);
       
        IButton Button(Rect rect, string content, UnityAction onClick=null);

        ILabel Label(Rect rect, string content);

        ITextField TextField(Rect rect, string initial, UnityAction<string> onChange=null);

        ITextArea TextArea(Rect rect, string initial, UnityAction<string> onChange = null);

        IToggle Toggle(Rect rect, string content, bool initial, UnityAction<bool> onChange = null);

        IToolbar Toolbar(Rect rect, int initial, string[] toolbarStrings, UnityAction<int> onChange = null);

        ISelectionGrid SelectionGrid(Rect rect, int initial, string[] selectionStrings, int columns, UnityAction<int> onChange = null);

        ISlider HorizontalSlider(Rect rect, float initial, float minimum, float maximum, UnityAction<float> onChange = null);

        ISlider VerticalSlider(Rect rect, float initial, float minimum, float maximum, UnityAction<float> onChange = null);

        IScrollView ScrollView(Rect rect, Vector2 inner);

        IFixedLayout Group(Rect rect);

        IAutoLayout Area(Rect rect, IAutoLayoutOptions options);
    }
}
