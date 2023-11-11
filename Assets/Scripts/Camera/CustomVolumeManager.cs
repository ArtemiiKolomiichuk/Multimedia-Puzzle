using UnityEngine;
using UnityEngine.Events;

public class CustomVolumeManager : MonoBehaviour
{
    private readonly Vector3 goodTilePosition = new(-106.05f, -1.54f, 20.92f);
    private readonly Vector3 badTilePosition = new(-109.62f, -1.54f, 24.49f);

    public ManagerState state = ManagerState.Off;
    public static UnityEvent<float?> onVolumeChange;

    public static CustomVolumeManager Instance { get; private set; }

    private void Awake()
    {
        onVolumeChange = new UnityEvent<float?>();
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        switch (state)
        {
            case ManagerState.Off:
                return;
            case ManagerState.Good:
                ChangeVolume(goodTilePosition);
                break;
            case ManagerState.Bad:
                ChangeVolume(badTilePosition);
                break;
        }
    }

    private void ChangeVolume(Vector3 tilePosition)
    {
        if(CameraController.Instance.currentFocusPoint != CameraController.FocusPointType.Board)
        {
            onVolumeChange.Invoke(null);
            return;
        }
        Vector3 cursorPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
        if (Physics.Raycast(ray, out var hit))
        {
            float distance = Vector3.Distance(hit.point, tilePosition);
            if(distance > 14)
            {
                onVolumeChange.Invoke(null);
                return;
            }
            float volume = (15 - distance - 1)/14;
            if (volume < 0)
            {
                volume = 0;
            }
            onVolumeChange.Invoke(volume);
            return;
        }
        onVolumeChange.Invoke(null);
    }

    public enum ManagerState
    {
        Off,
        Good,
        Bad
    }
}
