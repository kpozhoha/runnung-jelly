using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private static GameObject activeSkin;
    public static GameObject ActiveSkin {
        get {
            return activeSkin;
        }
    }

    public static void MakeActive(GameObject newActive) {
        if (activeSkin != null) {
            Deactivate(activeSkin);
        }
        Activate(newActive);
    }

    public static void ChangeSlotMode(GameObject slot, SlotMode mode) {
        switch (mode) {
            case SlotMode.Active:
                Activate(slot);
                break;
            case SlotMode.Use:
                Deactivate(slot);
                break;
        }
    }

    private static void Deactivate(GameObject shopSlot) {
        GameObject text = FindChildWithName(shopSlot, "Text");
        text.GetComponent<Text>().text = "USE";
        GameObject frame = FindChildWithName(shopSlot, "Frame");
        frame.GetComponent<Image>().sprite = shopSlot
            .GetComponentInParent<ShopItemBuyBuyButton>().EmptyFrame;
    }

    private static void Activate(GameObject shopSlot) {
        GameObject text = FindChildWithName(shopSlot, "Text");
        text.GetComponent<Text>().text = "ACTIVE";
        GameObject frame = FindChildWithName(shopSlot, "Frame");
        frame.GetComponent<Image>().sprite = shopSlot
            .GetComponentInParent<ShopItemBuyBuyButton>().ChosenFrame;
        activeSkin = shopSlot;
    }

    public static GameObject FindChildWithName(GameObject parent, string name) {
        RectTransform transform = parent.GetComponent<RectTransform>();
        Transform childTransform = transform.Find(name);
        return childTransform ? childTransform.gameObject : null;
    }
}
