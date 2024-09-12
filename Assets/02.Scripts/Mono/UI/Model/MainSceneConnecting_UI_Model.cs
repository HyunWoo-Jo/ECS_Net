using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mono.UI {
    // ���θ� internal�� ����
    public class MainSceneConnecting_UI_Model : UI_Model<MainSceneConnecting_UI_Presenter>
    {
        internal List<RoomData> roomDataList;
    }

    public struct RoomData {
        public string roomName;
        public string roomHash;
        public string userName;
    }
}
