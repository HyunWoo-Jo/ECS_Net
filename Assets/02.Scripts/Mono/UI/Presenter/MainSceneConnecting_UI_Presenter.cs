using JetBrains.Annotations;
using UnityEngine;
using System;
using Game.Network;
using System.Net;

namespace Game.Mono.UI {
    // 내부를 internal로 설계
    public class MainSceneConnecting_UI_Presenter : UI_Presenter<MainSceneConnecting_UI_View, MainSceneConnecting_UI_Model>
    {
        /// <summary>
        /// 유효성 검사및 NetworkManager 할당
        /// </summary>
        /// <param name="str"></param>
        internal void IpVarification(string str) {
            if (IPAddress.TryParse(str, out IPAddress ip)) {
                NetworkManager.Instance.SetIP(str);
            } else {
                NetworkManager.Instance.SetIP("0.0.0.0");
            }
        }
        
        /// <summary>
        /// 유효성 검사및 NetworkManager 할당
        /// </summary>
        /// <param name="str"></param>
        /// <returns> pre Port</returns>
        internal string PortVarification(string str) {
            if(ushort.TryParse(str, out var port)) {
                _model._prePort = str;
                NetworkManager.Instance.SetPort(port);
            }else if(string.IsNullOrEmpty(str)) {
                _model._prePort = string.Empty;
                NetworkManager.Instance.SetPort(0);
            }
            return _model._prePort;
        }
    }
}
 