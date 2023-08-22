using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target; // 따라갈 오브젝트
    public float moveSpeed; // 카메라 속도
    public BoxCollider2D bound; // 카메라 맵 탈출 X
    static public CameraManager instance; // 오브젝트 중복 에러 방지
    private Vector3 targetPosition; // 카메라 위치
    private Vector3 minBound; // 박스 컬라이더 영역의 최소 xyz값을 지님.
    private Vector3 maxBound; // 박스 컬라이더 영역의 최대 xyz값을 지님.
    private float halfWidth; // 카메라 반 너비 
    private float halfHeight; // 카메라 반 높이 
    private Camera theCamera; // 카메라 반높이값을 구한 속성을 이용하기 위한 변수

    private void Awake() // start보다 먼저 발생하는 내장함수
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    void Start()
    {
        theCamera = GetComponent<Camera>();
        if (bound != null)
        {
            minBound = bound.bounds.min;
            maxBound = bound.bounds.max;
        }
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height; // 공식
    }
    void Update()
    {
        if (target != null && target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
