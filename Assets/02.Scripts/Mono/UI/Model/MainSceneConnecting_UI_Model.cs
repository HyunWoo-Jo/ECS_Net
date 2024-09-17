using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Game.Mono.UI {
    // ���θ� internal�� ����
    public class MainSceneConnecting_UI_Model : UI_Model<MainSceneConnecting_UI_Presenter>
    {
        private List<RoomData> roomDataList = new();

        internal void AddRoomDataList(RoomData roomData) {
            roomDataList.Add(roomData);
        }
        /// <summary>
        /// readonly roomDataList
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<RoomData> GetRoomList_RO() {
            return roomDataList.AsReadOnly();
        }
        /// <summary>
        /// �� ������ �ʱ�ȭ
        /// </summary>
        internal void ClearRoomData() {
            roomDataList.Clear();
        }
    }

    public struct RoomData {
        public string roomHash;
        public string roomName;
        public string userName;
        public bool isPublic;
    }
}
