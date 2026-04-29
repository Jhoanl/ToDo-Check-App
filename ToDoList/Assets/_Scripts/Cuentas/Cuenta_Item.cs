using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cuenta_Item : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI amountLeft;
    [SerializeField] private Image progressImage;
    [SerializeField] private Gradient progressGradient;
    [SerializeField] private GameObject completeVisuals;
    [Space]
    [SerializeField] private Button editAmountBut;
    [SerializeField] private Button editRemoveAmountBut;
    [SerializeField] private Button editStartAmountBut;
    [SerializeField] private Button deleteBut;

    public bool destroying = false;

    private Cuenta_Data data;

    public Cuenta_Data Data { get => data; set => data = value; }

    private void Awake()
    {
        editAmountBut.onClick.AddListener(OnEdit);
        editRemoveAmountBut.onClick.AddListener(OnEditRemove);
        editStartAmountBut.onClick.AddListener(OnEditStartAmount);
        deleteBut.onClick.AddListener(OnDeleteBut);
    }

    private void OnEditRemove()
    {
        Action<string> OnPayed = (x) =>
        {
            Data.amount -= float.Parse(x);
            if (Data.amount < 0)
                data.amount = 0;
            UpdateUI();
            Manager_Cuentas.instance.Save();
        };

        UI_Cuentas.instance.inputTextPanel.Show("Desea quitar del abono:",
            "0", OnPayed);
    }

    private void OnDeleteBut()
    {
        Action OnDelete = () =>
        {
            Debug.Log("Deleted");
            Destroy(gameObject);
            destroying = true;
            Manager_Cuentas.instance.Save();
        };

        UI_Cuentas.instance.modalPanel.ShowConfirm("¿Borrar?", "Desea borrar esta cuenta?",
            null, OnDelete, null);
    }

    private void OnEditStartAmount()
    {
        Action<string> OnEdit = (x) =>
        {
            Data.startAmount = float.Parse(x);
            if (Data.startAmount < 0)
                data.startAmount = 0;

            UpdateUI();
            Manager_Cuentas.instance.Save();
        };

        UI_Cuentas.instance.inputTextPanel.Show("Monto a pagar:",
            Data.startAmount.ToString(), OnEdit);
    }

    private void OnEdit()
    {
        Action<string> OnPayed = (x) =>
        {
            Data.amount += float.Parse(x);
            if (Data.amount < 0)
                data.amount = 0;

            UpdateUI();
            Manager_Cuentas.instance.Save();
        };

        UI_Cuentas.instance.inputTextPanel.Show("Desea abonar:",
            "0", OnPayed);

        //Manager_Cuentas.instance.Edit(this);
    }

    public void Setup(Cuenta_Data data)
    {
        this.Data = data;
        UpdateUI();
    }

    public void UpdateUI()
    {
        string amount = $"<color=green>{GetAmountWithDots(data.amount.ToString())}</color> / " +
            $"<color=orange>{GetAmountWithDots(data.startAmount.ToString())}</color>";

        amountLeft.text = amount;

        float value = (float)data.amount / (float)data.startAmount;

        progressImage.fillAmount = value;
        progressImage.color = progressGradient.Evaluate(value);

        completeVisuals.SetActive(value > 1);
    }

    private string GetAmountWithDots(string amount)
    {
        string amountString = amount;
        string newString = amount;
        //for (int i = amountString.Length-1; i >=0 ; i--)

        if (amount.Length > 3)
            newString = amount.Insert(amount.Length - 3, ".");

        if (amount.Length > 6)
            newString = newString.Insert(amount.Length - 6, ".");

        return newString;
    }
}
