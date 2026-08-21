using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Camera mainCamera;        // メインカメラ
    public Camera followCamera;      // プレイヤー追従カメラ

    void Start()
    {
        // 初期状態：メインカメラをON、フォローカメラをOFF
        mainCamera.enabled = true;
        followCamera.enabled = false;
    }

    void Update()
    {
        // Fキーでフォローカメラへ切り替え
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchToFollowCamera();
        }

        // Gキーでメインカメラへ戻す
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchToMainCamera();
        }
    }

    void SwitchToFollowCamera()
    {
        mainCamera.enabled = false;
        followCamera.enabled = true;
    }

    void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        followCamera.enabled = false;
    }
}

