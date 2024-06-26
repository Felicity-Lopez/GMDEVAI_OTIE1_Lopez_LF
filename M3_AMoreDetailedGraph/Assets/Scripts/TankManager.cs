using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public GameObject greenTank;
    public GameObject redTank;
    public GameObject blueTank;

    private GameObject selectedTank;

    public void SelectGreen()
    {
        selectedTank = greenTank;
        Debug.Log("Green tank selected");
    }

    public void SelectRed()
    {
        selectedTank = redTank;
        Debug.Log("Red tank selected");
    }

    public void SelectBlue()
    {
        selectedTank = blueTank;
        Debug.Log("Blue tank selected");
    }

    public void GoToHelipad()
    {
        Debug.Log("GoToHelipad called");
        MoveSelectedTank(0);
    }

    public void GoToRuins()
    {
        Debug.Log("GoToRuins called");
        MoveSelectedTank(8);
    }

    public void GoToTwinMountains()
    {
        Debug.Log("GoToTwinMountains called");
        MoveSelectedTank(2);
    }

    public void GoToBarrack()
    {
        Debug.Log("GoToBarrack called");
        MoveSelectedTank(15);
    }

    public void GoToCommandCenter()
    {
        Debug.Log("GoToCommandCenter called");
        MoveSelectedTank(6);
    }

    public void GoToOilRefinery()
    {
        Debug.Log("GoToOilRefinery called");
        MoveSelectedTank(13);
    }

    public void GoToTanker()
    {
        Debug.Log("GoToTanker called");
        MoveSelectedTank(10);
    }

    public void GoToMiddle()
    {
        Debug.Log("GoToMiddle called");
        MoveSelectedTank(14);
    }

    public void GoToRadar()
    {
        Debug.Log("GoToRadar called");
        MoveSelectedTank(4);
    }

    public void GoToCommandPost()
    {
        Debug.Log("GoToCommandPost called");
        MoveSelectedTank(5);
    }

    private void MoveSelectedTank(int waypointIndex)
    {
        if (selectedTank != null)
        {
            Debug.Log("Moving tank: " + selectedTank.tag + " to waypoint: " + waypointIndex);
            selectedTank.GetComponent<FollowPath>().SetDestination(waypointIndex);
        }
        else
        {
            Debug.LogError("No tank selected.");
        }
    }
}
