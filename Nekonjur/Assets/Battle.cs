using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public UiManager uiManager;

    public GameObject tank;
    public GameObject dps;
    public GameObject support;

    private GameObject selectedCat;

    public void CheckCats()
    {
        if (!tank.activeSelf && !dps.activeSelf && !support.activeSelf)
        {
            uiManager.PrintGameOver();
        }
    }

    public void TankApproachEnemy()
    {
        selectedCat = tank;
        MoveSelectedCat(1);
    }

    public void TankReturnToPost()
    {
        selectedCat = tank;
        MoveSelectedCat(0);
    }

    public void DPSApproachEnemy()
    {
        selectedCat = dps;
        MoveSelectedCat(1);
    }

    public void DPSReturnToPost()
    {
        selectedCat = dps;
        MoveSelectedCat(0);
    }

    public void SupportApproachEnemy()
    {
        selectedCat = support;
        MoveSelectedCat(1);
    }

    public void SupportReturnToPost()
    {
        selectedCat = support;
        MoveSelectedCat(0);
    }

    private void MoveSelectedCat(int waypointIndex)
    {
        if (selectedCat != null)
        {
            selectedCat.GetComponent<FollowPath>().SetDestination(waypointIndex);
        }
    }

    public void HealTeammate(GameObject teammate)
    {
        CatBattle teammateScript = teammate.GetComponentInChildren<CatBattle>();
        teammateScript.GetHeal(20);
    }
}
