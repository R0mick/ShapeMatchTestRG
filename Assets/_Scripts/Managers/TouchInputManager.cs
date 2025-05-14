using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Managers
{
    public class TouchInputManager : MonoBehaviour
    {

        private bool _isTouchActive;


        private void OnEnable()
        {
            SimpleEventManager.Instance.OnUpdateTouchActive += SetTouchActive;
        }

        private void OnDisable()
        {
            SimpleEventManager.Instance.OnUpdateTouchActive -= SetTouchActive;
        }


        void Update()
        {
            if (_isTouchActive)
            {
                HandleTouch();
            }
        }

        private void HandleTouch()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Camera.main != null)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                    if (hit.collider != null)
                    {
                        var touchable = hit.collider.GetComponent<ITouchable>();
                        touchable?.OnTouch();
                    }
                }
            }
        }

        private void SetTouchActive(bool active)
        {
            //Debug.Log("Touch active = " + active);
            _isTouchActive = active;
        }
    }
}