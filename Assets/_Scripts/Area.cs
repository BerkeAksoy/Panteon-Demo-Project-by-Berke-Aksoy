using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private enum AreaType
    {
        RotPlatEntry,
        RotPlatExit,
        PaintEntry,
        FinishLine
    }

    private GameObject mainCam;
    private CameraManager cameraFollow;
    private GameManager gm;
    [SerializeField] AreaType areaType;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFollow = mainCam.GetComponent<CameraManager>();
        gm = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(areaType == AreaType.FinishLine)
        {
            if (!other.CompareTag("Player"))
            {
                Enemy colEnemy = other.GetComponent<Enemy>();
                if (colEnemy)
                    colEnemy.StopRunning();
            }
            else
            {

            }
        }

        if (!cameraFollow)
            return;

        switch (areaType)
        {
            case AreaType.RotPlatEntry:
                cameraFollow.SetCamForRotPlatforms(other.transform);
                break;
            case AreaType.RotPlatExit:
                cameraFollow.SetCamForRun();
                break;
            case AreaType.PaintEntry:
                GameObject paintWall = GameObject.FindGameObjectWithTag("Paintable");
                cameraFollow.SetCamForPainting(paintWall.transform);
                gm.SetGameState(GameManager.GameState.Paint);

                break;
            default:
                break;
        }

    }
}
