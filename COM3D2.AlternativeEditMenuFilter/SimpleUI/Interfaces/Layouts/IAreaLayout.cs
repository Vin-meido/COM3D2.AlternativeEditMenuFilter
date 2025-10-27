using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.SimpleUI
{
    interface IAreaLayout: IControlContainer, ILayout
    {
        IBox Box(string content);

        IButton Button(string content);

        ILabel Label(string content);

        ITextField TextField(string initial);

        ITextArea TextArea(string initial);

        IToggle Toggle(bool initial);

        IToolbar Toolbar(int initial, string[] toolbarStrings);

        ISelectionGrid SelectionGrid(int initial, string[] selectionStrings, int columns);

        ISlider HorizontalSlider(float initial, float minimum, float maximum);

        ISlider VerticalSlider(float initial, float minimum, float maximum);

        IScrollView ScrollView(Vector2 inner);

        IFixedLayout Group();

        IAutoLayout Area(IAutoLayoutOptions options);

        void Relayout();
    }
}
