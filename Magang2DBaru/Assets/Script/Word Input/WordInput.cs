using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInput : MonoBehaviour
{
    public InputField input;
    public PlayerInventory playerInventory;
    public List<InventoryItem> daftarHuruf = new List<InventoryItem>();
    public List<InventoryItem> daftarItem = new List<InventoryItem>();
    [SerializeField] private InventoryItem thisItem;

    private void OnEnable()
    {
        input.text = "";
    }

    public void getText()
    {
        string text = input.text.ToLower();
        if (cekKata(text))
        {
            if (cekHuruf(text) == text.Length)
            {
                Substraction(text);
                MakeItem();
            }
            else
            {
                Debug.Log("Maaf huruf yang anda punya kurang!");
            }
        }
        else
        {
            Debug.Log("Maaf inputan salah");
        }
    }

    bool cekKata(string cek)
    {
        for(int i = 0; i < daftarItem.Count; i++)
        {
            if (cek.Equals(daftarItem[i].itemName))
            {
                thisItem = daftarItem[i];
                return true;
            }
        }
        return false;
    }


    int cekHuruf(string data)
    {
       int counter = 0;
       for(int i = 0; i < data.Length; i++)
        {
            for(int j = 0; j < daftarHuruf.Count; j++)
            {
                if (data[i].ToString().Equals(daftarHuruf[j].itemName))
                {
                    if(daftarHuruf[j].numberHeld > 0)
                    {
                        counter++;
                    }
                }
            }
        }
        return counter;
    }

    void Substraction(string data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < daftarHuruf.Count; j++)
            {
                if (data[i].ToString().Equals(daftarHuruf[j].itemName))
                {
                    if (daftarHuruf[j].numberHeld > 0)
                    {
                        daftarHuruf[j].numberHeld--;
                    }
                }
            }
        }
    }

    void MakeItem()
    {
        if (!playerInventory.myInventory.Contains(thisItem))
        {
            playerInventory.myInventory.Add(thisItem);
            thisItem.numberHeld++;
        }
        else
        {
            if (thisItem.usable)
            {
                thisItem.numberHeld++;
            }
        }
    }

}
