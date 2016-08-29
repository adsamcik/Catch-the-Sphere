using UnityEngine;
using System.Collections;

public class GestureRecognition : MonoBehaviour {
    const float SWIPE_THRESHOLD = 10;
    bool init = false;
    bool active = false;
    bool swiped = false;
    Vector2 originalPosition;

    public delegate void SwipeAction(SwipeDirection dir);
    public static event SwipeAction OnSwipe;

    void OnStart() {
        if (init)
            throw new System.Exception("GestureRecognition shouldn't run more than once!");
        init = true;
    }

    void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (OnSwipe != null) {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnSwipe(SwipeDirection.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                OnSwipe(SwipeDirection.Right);
        }
#else

        if (Input.touchCount == 0) {
            active = false;
            swiped = false;
            return;
        } else if (swiped)
            return;

        if (!active) {
            originalPosition = Input.touches[0].position;
            active = true;
        } else if (OnSwipe != null) {
            float distance = (Input.touches[0].position - originalPosition).x;
            if (distance > SWIPE_THRESHOLD) {
                OnSwipe(distance > 0 ? SwipeDirection.Right : SwipeDirection.Left);
                swiped = true;
            }
        }

        active = true;
#endif
    }


    public enum SwipeDirection {
        Up,
        Right,
        Down,
        Left
    }
}
