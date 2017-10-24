using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionsManager : MonoBehaviour 
{
        public static ActionsManager Current;

        [SerializeField] Button[] buttons;

        Action[] actionCalls = new Action[12];

        public ActionsManager ()
        {
                Current = this;
        }

        public void ClearButtons ()
        {
                foreach (Button button in buttons)
                {
                        button.gameObject.SetActive(false);
                }
        }

        public void AddButton (Sprite pic, Action onClick, int index)
        {
                buttons[index].gameObject.SetActive(true);
                buttons[index].GetComponent<Image>().sprite = pic;
                actionCalls[index] = onClick;
        }

        public void OnButtonClick (int index)
        {
                actionCalls[index]();
        }

	void Awake () 
        {
                if (buttons != null)
                {
                        for (int i = 0; i < buttons.Length; i++)
                        {
                                int index = i;
                                if (buttons[index].gameObject.activeSelf)
                                {
                                        buttons[index].onClick.AddListener(delegate() { OnButtonClick(index); });
                                }
                        }

                        ClearButtons();
                }
	}
}
