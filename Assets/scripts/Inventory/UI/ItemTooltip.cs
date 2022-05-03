using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemTooltip : MonoBehaviour
{
   public Text itemName;
   public Text itemInfo;
   private RectTransform rectTransform;

   private void Awake()
   {
      rectTransform = GetComponent<RectTransform>();
   }

   public void SetUPItemInfo(ItemData_SO item)
   {
      itemName.text = item.itemName;
      itemInfo.text = item.discription;
   }

   private void OnEnable()
   {
      UpdatePoisition();
   }

   private void Update()
   {
      UpdatePoisition();
   }

   void UpdatePoisition()
   {
      Vector3 mouse = Input.mousePosition;
      Vector3[] corners = new Vector3[4];
      rectTransform.GetWorldCorners(corners);
      float width = corners[3].x - corners[0].x;
      float height = corners[1].y - corners[0].y;
      if (mouse.y < height)
      {
         rectTransform.position = mouse + Vector3.up * height * 0.8f;
      }

      else  if (Screen.width - mouse.x > width)
      {
         rectTransform.position = mouse + Vector3.right * width * 0.6f;
      }
      else rectTransform.position = mouse + Vector3.left * width * 0.6f;
   }
}
