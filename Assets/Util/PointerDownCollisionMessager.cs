using UnityEngine;
using UnityEngine.EventSystems;

public interface IPointerDownCollision : IEventSystemHandler
{ void OnPointerDown(Vector3 clickHit); }

public class PointerDownCollisionMessager : MonoBehaviour
{
    public PointerEventData.InputButton button;
    public void Update()
    {
        if (Input.GetMouseButtonDown((int)button))
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit))
            {
                ExecuteEvents.Execute<IPointerDownCollision>(gameObject, null, (x,y) => x.OnPointerDown(hit.point));
            }
        }
    }
}
