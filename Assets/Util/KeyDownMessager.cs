using UnityEngine;
using UnityEngine.EventSystems;

public interface IKeyDownEvent : IEventSystemHandler
{ void OnKeyDown(KeyCode key); }

public class KeyDownMessager : MonoBehaviour
{
    public KeyCode key;
    public void Update()
    {
        if (Input.GetKeyDown(key))
        {
            ExecuteEvents.Execute<IKeyDownEvent>(gameObject, null, (x, y) => x.OnKeyDown(key));
        }
    }
}
