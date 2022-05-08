using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roadPref;

    private float roadSize = 2.16f;

    enum dir { left, right, up, down }
    private dir direction = dir.up;
    private Vector2 roadPos;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 500; ++i)
        {
            RoadGeneration();
        }
    }

    private void RoadGeneration()
    {
        GameObject road = Instantiate(roadPref) as GameObject;

        Vector2 pos = Vector2.zero;
        switch(direction)
        {
            case dir.up:
                pos = Vector2.up;
                break;

            case dir.left:
                pos = Vector2.left;
                break;

            case dir.right:
                pos = Vector2.right;
                break;
        }

        roadPos += pos;

        count += 1;

        if(count > 2)
        {
            if (direction == dir.up)
            {
                switch (Random.Range(0, 9))
                {
                    case 0:
                        direction = dir.left;
                        count = 0;
                        break;

                    case 1:
                        direction = dir.right;
                        count = 0;
                        break;
                }
            }

            else if (direction == dir.left)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        direction = dir.up;
                        count = 0;
                        break;
                }
            }

            else if (direction == dir.right)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        direction = dir.up;
                        count = 0;
                        break;
                }
            }
        }

        road.transform.position = roadPos * roadSize;
    }
}
