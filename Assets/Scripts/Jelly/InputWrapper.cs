using System.Collections.Generic;
using UnityEngine;

public class InputWrapper : Input {
    // enables access by fingerId
    private static Dictionary<int, Touch> activeTouches;

    public static Touch GetTouchByFingerId(int fingerId) {
        UpdateDictionary();      
        if (!activeTouches.ContainsKey(fingerId)) {
            throw new System.Exception("no touch found");
        }
        return activeTouches[fingerId];
    }

    private static void UpdateDictionary() {
        activeTouches = new Dictionary<int, Touch>();
        Touch[] touches = Input.touches;
        for (int i = 0; i < touches.Length; ++i) {
            activeTouches.Add(touches[i].fingerId, touches[i]);
        }
    }
}
