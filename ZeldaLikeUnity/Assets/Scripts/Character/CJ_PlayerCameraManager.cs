using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Management;

namespace Player
{
    public class CJ_PlayerCameraManager : Singleton<CJ_PlayerCameraManager>
    {
        public List<GameObject> ennemyList;
        public CinemachineVirtualCamera camera1;
        [HideInInspector]  public CinemachineFramingTransposer cameraProfile;
        bool camSwitch = true;

        private void Start()
        {
            cameraProfile = camera1.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update()
        {
            if(ennemyList.Count > 0 &&camSwitch == true)
            {
                CombatCamera();
                camSwitch = false;
            }
            else if (ennemyList.Count == 0 && camSwitch == false)
            {
                ExplorationCamera();
                camSwitch = true;
            }
        }

        void ExplorationCamera()
        {
            cameraProfile.m_LookaheadTime = 0.15f;
            cameraProfile.m_LookaheadSmoothing = 15f;
            cameraProfile.m_XDamping = 1.11f;
            cameraProfile.m_YDamping = 1.11f;
            cameraProfile.m_SoftZoneHeight = 0.6f;
            cameraProfile.m_SoftZoneHeight = 0.6f;
        }

        void CombatCamera()
        {
            cameraProfile.m_LookaheadTime = 0.01f;
            cameraProfile.m_SoftZoneHeight = 0.2f;
            cameraProfile.m_SoftZoneHeight = 0.2f;
        }
    }
}
