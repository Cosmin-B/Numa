using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Controller))]
public class Player_StateChanger : MonoBehaviour
{

    private Vector3 _cameraPosition = Vector3.zero;
    private Vector3 _targetCameraPosition = Vector3.zero;

    private float _cameraStartTime = 0.0f;

    [HideInInspector]
    public Player_Controller playerController;

    private Transform _camera;

    public bool hasWheels;

    public void Start()
    {
        hasWheels = false;
        playerController = GetComponent<Player_Controller>();
        Debug.Assert(playerController);

        _camera = GetComponentInChildren<Camera>().transform;
    }

    public void ChangeMovementMode(Player_Controller.MovementType movementType)
    {
        if (playerController.isChangeingWalkMode)
            return;
        if (playerController.GetMovementType() == movementType)
        {
            Debug.Log("Player is already has the movement state!");
            return;
        }

        Player_Controller.animator.Play("Equip");

        Game_Manager.Instance.GetSoundManager().PlaySoundForPlayer(Player_SoundHolder.transitionSound);
        SetMovementType(movementType);
    }

    public void SetMovementType(Player_Controller.MovementType movementType)
    {

        playerController.SetMovementType(movementType);

        _cameraPosition = _camera.localPosition;

        if (movementType == Player_Controller.MovementType.MOVE_WALK)
            _targetCameraPosition = _cameraPosition + Vector3.down * playerController.cameraOffset;
        else if(movementType == Player_Controller.MovementType.MOVE_WHEEL)
            _targetCameraPosition = _cameraPosition + Vector3.up * playerController.cameraOffset;

        playerController.isChangeingWalkMode = true;

        StartCoroutine(StartCameraLerping());

    }

    IEnumerator StartCameraLerping()
    {
        _cameraStartTime = Time.time;

        while (playerController.isChangeingWalkMode)
        {
            yield return RepeatLerp(_camera.transform.localPosition, _targetCameraPosition, playerController.cameraLerpTime);

            float currentTime = Time.time - _cameraStartTime;

            if (currentTime >= playerController.cameraLerpTime)
                playerController.isChangeingWalkMode = false;
        }

    }

    IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * playerController.cameraTransitionSpeed;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            _camera.transform.localPosition = Vector3.Slerp(a, b, i);
            yield return null;
        }
    }
}
