using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KinectTest : MonoBehaviour {

    public RawImage kinectImg;
    public Canvas canvas;
    public Image rightHand, startButton, settingButton, exitButton;
    public Sprite[] handStateSprites;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //print("dw = " + KinectManager.Instance.GetDepthImageWidth() + " dh = " + KinectManager.Instance.GetDepthImageHeight());

        bool isInit = KinectManager.Instance.IsInitialized();
        if (isInit)
        {
            //初始化完成

            if (kinectImg.texture == null)
            {
                //Texture2D kinectPic = KinectManager.Instance.GetUsersClrTex(); //获取彩色数据
                Texture2D kinectUserPic = KinectManager.Instance.GetUsersLblTex();//获取深度数据
                kinectImg.texture = kinectUserPic; //显示彩色数据
            }
        }

        if(KinectManager.Instance.IsUserDetected())
        {
            long userId = KinectManager.Instance.GetPrimaryUserID();
            Vector3 userPos = KinectManager.Instance.GetUserPosition(userId);

            int jointType = (int)KinectInterop.JointType.HandRight;
            if(KinectManager.Instance.IsJointTracked(userId, jointType))
            {
                Vector3 rightHandPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);
                Vector3 rightHandScreenPos = Camera.main.WorldToScreenPoint(rightHandPos); //屏幕坐标
                Vector2 rightHandSenPos = new Vector2(rightHandScreenPos.x, rightHandScreenPos.y);
                Vector2 rightHandUguiPos;


                if(RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, 
                                                                            rightHandSenPos, Camera.main,out rightHandUguiPos))
                {
                    KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(userId);
                    if (rightHandState == KinectInterop.HandState.Closed)
                    {
                        rightHand.sprite = handStateSprites[1];
                    }
                    else
                    {
                        rightHand.sprite = handStateSprites[0];
                    }
                    
                    RectTransform rightRecTf = rightHand.transform as RectTransform;
                    rightRecTf.anchoredPosition = rightHandUguiPos;

                    if(RectTransformUtility.RectangleContainsScreenPoint(startButton.rectTransform, rightHandSenPos, Camera.main))
                    {
                        if(rightHandState == KinectInterop.HandState.Closed)
                        {
                            rightHand.sprite = handStateSprites[1];
                            print("开始");
                        }

                    }
                    else if(RectTransformUtility.RectangleContainsScreenPoint(settingButton.rectTransform, rightHandSenPos, Camera.main))
                    {
                        if (rightHandState == KinectInterop.HandState.Closed)
                        {
                            rightHand.sprite = handStateSprites[1];
                            print("设置");
                        }
                            
                    }
                    else if (RectTransformUtility.RectangleContainsScreenPoint(exitButton.rectTransform, rightHandSenPos, Camera.main))
                    {
                        if (rightHandState == KinectInterop.HandState.Closed)
                        {
                            rightHand.sprite = handStateSprites[1];
                            print("退出");
                        }
                            
                    }
                    else
                    {
                        print("无效");
                    }

                }

                

               
                         
            }
        }
	}
}
