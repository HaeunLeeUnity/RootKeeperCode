#if UNITY_IOS
using System;
using Unity.Advertisement.IosSupport;
using UnityEngine;

// IOS 의 경우 앱 추적을 요구한다.
namespace Title
{

    public class IDFARequest : MonoBehaviour
    {
        public event Action sentTrackingAuthorizationRequest;
        public static IDFARequest instance;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PleaseGiveMeIDFA()
        {

            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
                sentTrackingAuthorizationRequest?.Invoke();
            }
        }

        public bool IsDetermined()
        {
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
               ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                return false;
            }

            return true;
        }
    }
}
#endif