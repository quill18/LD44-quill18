using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
    }
    PlayerUnit playerUnit;

    UpgradeManager UpgradeManager;

    int[] availableUpgrades;

    public TextMeshProUGUI RerollLabel;


    int rerollCost;
    private void OnEnable()
    {
        UpgradeManager = GameObject.FindObjectOfType<UpgradeManager>();
        rerollCost = 1; // will become 2
        RefreshUpgrades();
    }

    public void RefreshUpgrades()
    {
        rerollCost++;
        UpgradeManager.Upgrades.Shuffle();

        // Display the first three Upgrades

        Transform t = this.transform.GetChild(0);

        for (int i = 0; i < 3; i++)
        {
            Transform btn = t.GetChild(i);
            btn.GetChild(0).GetComponent<TextMeshProUGUI>().text = UpgradeManager.Upgrades[i].Name;
            btn.GetChild(1).GetComponent<TextMeshProUGUI>().text = UpgradeManager.Upgrades[i].Description;
            btn.GetChild(2).GetComponent<TextMeshProUGUI>().text = UpgradeManager.Upgrades[i].Cost + " scrap" + (UpgradeManager.Upgrades[i].Cost > 1 ? "s" : "");
        }


        RerollLabel.text = "Re-Roll Upgrade Options!\n("+ rerollCost +" Scrap)";
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

        if (playerUnit.GetComponent<Unit>().Health < UpgradeManager.Upgrades[btnNumber].Cost)
            return;

        UpgradeManager.PurchaseUpgrade(UpgradeManager.Upgrades[btnNumber]);
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
