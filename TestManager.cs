using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
public class TestManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            InventoryManager.instance.CalculateMoney(1000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.instance.GetWeapon(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InventoryManager.instance.GetWeapon(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InventoryManager.instance.GetWeapon(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InventoryManager.instance.GetWeapon(9);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            InventoryManager.instance.GetWeapon(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            InventoryManager.instance.GetWeapon(11);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            InventoryManager.instance.GetWeapon(12);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            InventoryManager.instance.GetWeapon(13);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            InventoryManager.instance.GetWeapon(14);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            InventoryManager.instance.GetWeapon(15);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            InventoryManager.instance.GetWeapon(16);
        }
    }
}
