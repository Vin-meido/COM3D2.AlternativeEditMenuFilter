using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

using COM3D2.SimpleUI.Events;

namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleDropdown : SimpleControl, IDropdown
    {
        protected bool ready = false;

        public string Value { 
            get => uiPopupList.value;
            set => uiPopupList.value = value;
        }

        IEnumerable<string> _choices;
        public IEnumerable<string> Choices
        {
            get => _choices;
            set
            {
                _choices = value;
                uiPopupList.Clear();
                foreach (var choice in value)
                {
                    uiPopupList.AddItem(choice);
                }
            }
        }

        protected UIPopupList uiPopupList;
        protected UISprite uiSprite;
        protected UIButton uiButton;
        protected UILabel uiLabel;

        readonly TextChangeEvent onChange = new TextChangeEvent();

        public override void InitControl()
        {
            var atlas = UIUtils.GetAtlas("AtlasCommon");
            var spriteName = "cm3d2_common_plate_white";

            uiSprite = NGUITools.AddSprite(this.gameObject, atlas, spriteName);
            NGUITools.AddWidgetCollider(uiSprite.gameObject);
            uiPopupList = uiSprite.gameObject.AddComponent<UIPopupList>();
            uiPopupList.atlas = atlas;
            uiPopupList.backgroundSprite = "cm3d2_common_plate_white";
            uiPopupList.backgroundColor = Color.white;
            uiPopupList.highlightSprite = "cm3d2_common_plate_white";
            uiPopupList.highlightColor = new Color(.8f, .8f, .8f);
            uiPopupList.textColor = Color.black;
            uiPopupList.padding = new Vector2(4, 10);
            uiPopupList.trueTypeFont = UIUtils.GetFont("NotoSansCJKjp-DemiLight");
            uiPopupList.position = UIPopupList.Position.Below;
            uiPopupList.value = null;
            EventDelegate.Add(uiPopupList.onChange, new EventDelegate.Callback(this.uiPopupListChange));

            uiButton = this.uiSprite.gameObject.AddComponent<UIButton>();
            uiButton.hover = Color.white;
            uiButton.defaultColor = new Color(.9f, .9f, .9f);
            
            this.uiLabel = NGUITools.AddWidget<UILabel>(uiSprite.gameObject);
            uiLabel.trueTypeFont = UIUtils.GetFont("NotoSansCJKjp-DemiLight");
            uiLabel.color = Color.black;
        }

        void Start()
        {
            // UIPopupList has an annoying feature of firing change event as soon as initialization
            // This delays forwarding change events untill that happens
            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart()
        {
            yield return null;
            this.ready = true;
        }
        
        public void AddChangeCallback(UnityAction<string> callback)
        {
            this.onChange.AddListener(callback);
        }

        public void RemoveChangeCallback(UnityAction<string> callback)
        {
            this.onChange.RemoveListener(callback);
        }

        protected virtual void uiPopupListChange()
        {
            if(this.ready)
            {
                this.onChange.Invoke(this.uiPopupList.value);
            }
        }

        public override void UpdateUI()
        {
            uiLabel.text = this.text;

            uiSprite.SetDimensions(
                Mathf.FloorToInt(this.size.x + .5f),
                Mathf.FloorToInt(this.size.y + .5f));
            uiSprite.ResizeCollider();
        }
    }
}
