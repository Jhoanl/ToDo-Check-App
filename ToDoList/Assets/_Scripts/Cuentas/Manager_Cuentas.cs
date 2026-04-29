using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager_Cuentas : MonoBehaviour
{
    public static Manager_Cuentas instance;

    [SerializeField] private List<Cuenta_Item> cuenta_Items;

    

    ModalPanel modalPanel;

    private void Awake()
    {
        instance = this;

        UI_Cuentas.OnAddItem += UI_Cuentas_OnAddItem;

        cuenta_Items = new List<Cuenta_Item>();

        int amountSaved = PlayerPrefs.GetInt("AmountSaved", 0);

        for (int i = 0; i < amountSaved; ++i)
        {
            string startAmount = PlayerPrefs.GetString("StartAmount" + i, "100");
            string amount = PlayerPrefs.GetString("Amount" + i, "0");

            Cuenta_Item cuentaItem = UI_Cuentas.instance.ForceCreateItem();
            Cuenta_Data data = new Cuenta_Data();
            data.startAmount = double.Parse(startAmount);
            data.amount= double.Parse(amount);
            cuentaItem.Setup(data);

            Debug.Log("Loaded" + i);

            cuenta_Items.Add(cuentaItem);
        }
    }

    private void OnDestroy()
    {
        UI_Cuentas.OnAddItem -= UI_Cuentas_OnAddItem;
    }

    private void UI_Cuentas_OnAddItem(Cuenta_Item item)
    {
        item.Setup(new Cuenta_Data());

        cuenta_Items.Add(item);

        Save();
    }

    public void Edit(Cuenta_Item cuenta_Item)
    {
        UI_Cuentas.instance.OpenEdit(cuenta_Item);
    }

    public void Save()
    {
        cuenta_Items.RemoveAll((x) => x == null);
        cuenta_Items.RemoveAll((x) => x.destroying);

        int amountSaved = cuenta_Items.Count;

        for (int i = 0; i < cuenta_Items.Count; ++i) {
            if (cuenta_Items[i] == null)
                return;

            PlayerPrefs.SetString("StartAmount" + i, cuenta_Items[i].Data.startAmount.ToString());
            PlayerPrefs.SetString("Amount" + i, cuenta_Items[i].Data.amount.ToString());
        }

        PlayerPrefs.SetInt("AmountSaved", amountSaved);

        Debug.Log("Saved  " + amountSaved);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
