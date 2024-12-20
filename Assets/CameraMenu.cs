using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    public Transform mainPosition;
    public Transform playPosition;
    public Transform optionPosition;
    private Transform _targetPosition;
    public float moveSpeed;
    public float rotationSpeed;
    public CameraPosition cameraPosition;  

    // calculation necessary variables 
    private float _distanceToTarget;
    private float _angleToTarget;
    private float _adjustedMoveSpeed;
    private float _adjustedRotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _targetPosition = mainPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraPosition == CameraPosition.MAIN)
            _targetPosition = mainPosition;
        if(cameraPosition == CameraPosition.PLAY)
            _targetPosition = playPosition;
        if(cameraPosition == CameraPosition.OPTION)
            _targetPosition = optionPosition;
        
        _distanceToTarget = Vector3.Distance(transform.position, _targetPosition.position);
        _angleToTarget = Quaternion.Angle(transform.rotation, _targetPosition.rotation);

        _adjustedMoveSpeed = moveSpeed;
        _adjustedRotationSpeed = rotationSpeed;

        if (_distanceToTarget > 0 && _angleToTarget > 0)
        {
            float positionTime = _distanceToTarget / moveSpeed;
            float rotationTime = _angleToTarget / rotationSpeed;

            if (positionTime > rotationTime)
            {
                _adjustedRotationSpeed = _angleToTarget / positionTime; // Ralentir la rotation
            }
            else
            {
                _adjustedMoveSpeed = _distanceToTarget / rotationTime; // Ralentir le déplacement
            }
        }

        // Déplacement et rotation synchronisés
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _adjustedMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetPosition.rotation, _adjustedRotationSpeed * Time.deltaTime);
    }

    public void SetMainPosition()
    {
        this.cameraPosition = CameraPosition.MAIN;
    }

    public void SetPlayPosition()
    {
        this.cameraPosition = CameraPosition.PLAY;
    }

    public void SetOptionPosition()
    {
        this.cameraPosition = CameraPosition.OPTION;
    }
}

public enum CameraPosition
{
    MAIN,
    PLAY,
    OPTION
}
