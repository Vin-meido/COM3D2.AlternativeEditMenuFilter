using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.AlternativeEditMenuFilter
{
    public class EditMenuPanelController
    {
		UIGrid m_grid;
		UIScrollView m_scrollView;
		Transform m_gridTableTrans;
		UIScrollBar m_scrollBar;
		SceneEdit m_sceneEdit;

		public EditMenuPanelController(GameObject go)
        {
			this.m_scrollView = go.GetComponentInChildren<UIScrollView>(false);
			Assert.IsNotNull(m_scrollView, $"Could not find UIScrollView for {go}");

			this.m_grid = this.m_scrollView.GetComponentInChildren<UIGrid>();
			Assert.IsNotNull(m_grid, $"Could not find UIGrid for {go}");

			this.m_gridTableTrans = this.m_grid.transform;

			this.m_sceneEdit = GameObject.Find("__SceneEdit__").GetComponent<SceneEdit>();
			Assert.IsNotNull(m_sceneEdit, $"Could not find SceneEdit");

			this.m_scrollBar = go.GetComponentInChildren<UIScrollBar>(false);
			Assert.IsNotNull(m_scrollBar, $"Could not find UIScrollBar for {go}");
		}

		public IEnumerable<EditMenuPanelItem> GetAllGameObjectMenus()
		{
			return (from i in Enumerable.Range(0, this.m_gridTableTrans.childCount)
					select this.m_gridTableTrans.GetChild(i) into item
					where item != null
					select item.Find("Button") into btn
					where btn != null
					select btn.GetComponent<ButtonEdit>() into edit
					where edit != null && edit.m_MenuItem != null
					where edit.m_MenuItem.m_strMenuFileName != ""
					where edit.m_MenuItem.m_strMenuName != "無し"
					where !edit.m_MenuItem.m_strMenuName.Contains("脱ぐ・外す")
					select new EditMenuPanelItem(edit.transform.parent.gameObject, edit.m_MenuItem)
					);
		}

		public void ResetView()
		{
			bool enabled = this.m_grid.enabled;
			this.m_grid.enabled = true;
			this.m_grid.Reposition();
			this.m_scrollView.ResetPosition();
			this.m_scrollBar.value = 0f;
			this.m_grid.enabled = enabled;
			this.m_sceneEdit.HoverOutCallback();
		}

		public void ShowAll()
        {
			foreach (var r in GetAllGameObjectMenus())
            {
				if (!r.gameObject.activeSelf)
                {
					r.gameObject.SetActive(true);
                }
            }
        }
	}
}
