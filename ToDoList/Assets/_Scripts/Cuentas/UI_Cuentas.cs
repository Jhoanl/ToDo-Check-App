using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cuentas : MonoBehaviour
{
    public static UI_Cuentas instance;

    public ModalPanel modalPanel;
    public InputTextPanel inputTextPanel;

    public static event Action<Cuenta_Item> OnAddItem;

    [Header("Cuentas")]
    [SerializeField] private Cuenta_Item[] cuenta_Items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemsParent;

    [Header("Cuentas Panel")]
    [SerializeField] private Button addItem;

    [Header("Edit Panel")]
    [SerializeField] private GameObject editPanelGo;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private InputField addInputField;
    [SerializeField] private Button confirmBut;

    private Cuenta_Item selectedItem;

    private void Awake()
    {
        instance = this;

        addItem.onClick.AddListener(AddItem);
    }

    public Cuenta_Item ForceCreateItem()
    {
        Cuenta_Item item = Instantiate(itemPrefab, itemsParent)
           .GetComponent<Cuenta_Item>();

        return item;
    }

    public void AddItem()
    {

        Cuenta_Item item = Instantiate(itemPrefab, itemsParent)
            .GetComponent<Cuenta_Item>();

        OnAddItem?.Invoke(item);
    }

    public void OpenEdit(Cuenta_Item cuenta_Item)
    {
        selectedItem = cuenta_Item;

        amount.text = cuenta_Item.Data.amount.ToString();  
    }
}
