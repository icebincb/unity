using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
//[System.Serializable]
/*public class EventVector3 : UnityEvent<Vector3>
{

}*/
using UnityEngine.EventSystems;
public class Mousemanage : Singleton<Mousemanage>
{
 
    public Texture2D point, doorway, attack, target, arrow;
    RaycastHit hitInfo;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        SetCoursorTexture();
        if(!internectWithUI())
        MouseControll();
    }
    void SetCoursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hitInfo))
        {
            //切换鼠标贴图
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                Cursor.SetCursor(target, new Vector2(16, 16),CursorMode.Auto);
            } else if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
            }
            else if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
            }
            else if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
            }
            else
            {
               Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
            }
        }
    }
    void MouseControll()
    {
        if (Input.GetMouseButtonDown(0)&& hitInfo.collider!=null)
        {
           // Debug.Log("click");
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            } 
            // 
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                //Debug.Log("click");
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                //Debug.Log("click");
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                //Debug.Log("click");
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }


    bool internectWithUI()
    {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current != null)
            return true;
        return false;
    }
}
