using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private bool followPlayer = true;
    private Vector3 distancePlayer = Vector3.zero;

    public void InitLevel()
    {
        distancePlayer = transform.position - GameManager.Instance.LevelController.player.transform.position;
    }

    private void Update ()
    {
        if (followPlayer)
            transform.position = GameManager.Instance.LevelController.player.transform.position + distancePlayer;	
	}
}
