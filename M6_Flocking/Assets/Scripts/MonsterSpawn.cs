using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject repelObstacle;
    public GameObject attractObstacle;
    GameObject[] agents;

    // Start is called before the first frame update
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");
    }

    void HandleObstaclePlacement(GameObject obstacle, bool isLeftClick)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Instantiate(obstacle, hit.point, obstacle.transform.rotation);

            foreach (GameObject agent in agents)
            {
                agent.GetComponent<AIController>().DetectNewObstacle(hit.point, isLeftClick);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleObstaclePlacement(repelObstacle, true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleObstaclePlacement(attractObstacle, false);
        }
    }
}
