using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Player : MonoBehaviour
{
    private Player player = null;
    public GameObject HUD_move = null;
    public LineRenderer HUD_point = null;
    public Material HUD_pointMaterial = null;
    private Quaternion rotationHUD_point = new Quaternion(0, 0, 0, 0);
    private float scope = 0;

    public void Init(Player _player, float _scope)
    {
        player = _player;
        scope = _scope;
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }

    public void UpdateHUD_move()
    {
        HUD_move.transform.position = new Vector3(player.transform.position.x + GameManager.Instance.LevelController.HUD_level.moveJoystick.Horizontal, HUD_move.transform.position.y, player.transform.position.z + GameManager.Instance.LevelController.HUD_level.moveJoystick.Vertical);
        HUD_move.transform.LookAt(GameManager.Instance.LevelController.gameCamera.transform);
    }

    public void UpdateHUD_point(bool pointed_auto)
    {
        HUD_point.SetPosition(1, new Vector3(0, 0, scope));
        if (!pointed_auto)
        {
            if (new Vector3(GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction.x, 0, GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction.y) != Vector3.zero)
                rotationHUD_point = Quaternion.LookRotation(new Vector3(GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction.x, 0, GameManager.Instance.LevelController.HUD_level.attackJoystick.Direction.y));
        }
        else
        {
            if (player.transform.forward != Vector3.zero)
                rotationHUD_point = Quaternion.LookRotation(player.transform.forward);
        }
        HUD_point.transform.rotation = rotationHUD_point;
    }

    public void SetAttackColorHUD_point()
    {
        HUD_pointMaterial.color = Color.red;
    }

    private void SetNormalColorHUD_point()
    {
        HUD_pointMaterial.color = Color.yellow;
    }

    public void DisableHUD_point()
    {
        HUD_point.SetPosition(1, new Vector3(0, 0, 0));
        SetNormalColorHUD_point();
    }
}
