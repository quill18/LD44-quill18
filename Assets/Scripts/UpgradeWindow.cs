using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeWindow : MonoBehaviour
{
    PlayerUnit playerUnit;

    UpgradeManager UpgradeManager;

    Upgrade[] availableUpgrades;

    public TextMeshProUGUI RerollLabel;

    int numUpgradesToShow = 3;

    int rerollCost;
    private void OnEnable()
    {
        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
        UpgradeManager = GameObject.FindObjectOfType<UpgradeManager>();
        rerollCost = 1; // will become 2
        RefreshUpgrades();
    }

    public void RefreshUpgrades()
    {
        rerollCost++;
        UpgradeManager.Upgrades.Shuffle();
        availableUpgrades = new Upgrade[numUpgradesToShow];


        // Display the first three Upgrades


        for (int i = 0; i < numUpgradesToShow; i++)
        {
            availableUpgrades[i] = UpgradeManager.Upgrades[i];
            SetupButton(i);
        }


        RerollLabel.text = "Re-Roll Upgrade Options!\n("+ rerollCost +" Scrap)";
        UpdateButtonAvailability();
    }

    void UpdateButtonAvailability()
    {
        Transform t = this.transform.GetChild(0);
        for (int i = 0; i < numUpgradesToShow; i++)
        {
            Transform btn = t.GetChild(i);
            if (availableUpgrades[i].Cost > playerUnit.GetComponent<Unit>().Health)
            {
                // Disable button
                btn.GetComponent<CanvasGroup>().alpha = 0.5f;
                btn.GetComponent<CanvasGroup>().interactable = false;
            }
            else
            {
                btn.GetComponent<CanvasGroup>().alpha = 1f;
                btn.GetComponent<CanvasGroup>().interactable = true;
            }
        }


        Transform rerollBtn = RerollLabel.transform.parent;
        if (rerollCost > playerUnit.GetComponent<Unit>().Health)
        {
            rerollBtn.GetComponent<CanvasGroup>().alpha = 0.5f;
            rerollBtn.GetComponent<CanvasGroup>().interactable = false;
        }
        else
        {
            rerollBtn.GetComponent<CanvasGroup>().alpha = 1f;
            rerollBtn.GetComponent<CanvasGroup>().interactable = true;
        }
    }

    void SetupButton(int i)
    {
        Transform t = this.transform.GetChild(0);
        Transform btn = t.GetChild(i);
        btn.GetChild(0).GetComponent<TextMeshProUGUI>().text = availableUpgrades[i].Name;
        btn.GetChild(1).GetComponent<TextMeshProUGUI>().text = availableUpgrades[i].Description;
        btn.GetChild(2).GetComponent<TextMeshProUGUI>().text = availableUpgrades[i].Cost + " scrap" + (availableUpgrades[i].Cost > 1 ? "s" : "");
    }

    void RandomizeOneButton(int i)
    {
        Upgrade u = UpgradeManager.Upgrades[Random.Range(0, UpgradeManager.Upgrades.Count)];
        for (int j = 0; j < numUpgradesToShow; j++)
        {
            if(j != i && availableUpgrades[j] == u)
            {
                // We rolled an upgrade already showing.
                RandomizeOneButton(i);  // super dumb way
                return;
            }
        }

        // u is a new upgrade
        availableUpgrades[i] = u;
        SetupButton(i);
        UpdateButtonAvailability();
    }

    public void RerollButtonClicked()
    {
        if (playerUnit.GetComponent<Unit>().Health < rerollCost)
            return;

        playerUnit.GetComponent<Unit>().Health -= rerollCost;
        RefreshUpgrades();
    }

    public void UpgradeButtonClicked(int btnNumber)
    {
        Debug.Log("UpgradeButtonClicked");

        if (playerUnit.GetComponent<Unit>().Health < availableUpgrades[btnNumber].Cost)
            return;

        UpgradeManager.PurchaseUpgrade(availableUpgrades[btnNumber]);
        RandomizeOneButton(btnNumber);
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
