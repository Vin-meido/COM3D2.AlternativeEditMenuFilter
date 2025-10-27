using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using UnityEngine.Events;

namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleButton : SimpleControl, IButton
    {
        UISprite uiSprite;
        UILabel uiLabel;
        UIButton uiButton;

        bool _isEnabled = true;

        public bool isEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = true;
                this.SetDirty();
            }
        }

        readonly UnityEvent click = new UnityEvent();
        public UnityEvent Click { get => click; }

        public Color defaultColor {
            get => this.uiButton.defaultColor;
            set => this.uiButton.defaultColor = value;
        }

        public Color hoverColor { 
            get => this.uiButton.hover;
            set => this.uiButton.hover = value;
        }

        public Color disabledColor { 
            get => this.uiButton.disabledColor;
            set => this.uiButton.disabledColor = value;
        }

        public Color activeColor {
            get => this.uiButton.pressed;
            set => this.uiButton.pressed = value;
        }

        public override void InitControl()
        {
            var atlas = UIUtils.GetAtlas("AtlasCommon");
            var spriteName = "cm3d2_common_plate_white";

            this.uiSprite = NGUITools.AddSprite(this.gameObject, atlas, spriteName);
            NGUITools.AddWidgetCollider(uiSprite.gameObject);

            this.uiButton = this.uiSprite.gameObject.AddComponent<UIButton>();
            uiButton.hover = Color.white;
            uiButton.defaultColor = new Color(.9f, .9f, .9f);
            EventDelegate.Add(uiButton.onClick, new EventDelegate.Callback(this.Click.Invoke));

            this.uiLabel = NGUITools.AddWidget<UILabel>(uiSprite.gameObject);
            uiLabel.trueTypeFont = UIUtils.GetFont("NotoSansCJKjp-DemiLight");
            uiLabel.color = Color.black;
        }

        public override void UpdateUI()
        {
            uiSprite.width = uiLabel.width = Mathf.FloorToInt(size.x + 0.5f);
            uiSprite.height = uiLabel.height = Mathf.FloorToInt(size.y + 0.5f);
            uiSprite.ResizeCollider();

            uiLabel.text = this.text;

            uiButton.isEnabled = this.isEnabled;
        }
    }
}
