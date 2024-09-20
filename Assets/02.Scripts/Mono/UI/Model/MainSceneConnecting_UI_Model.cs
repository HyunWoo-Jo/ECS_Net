using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Game.Mono.UI {
    // 내부를 internal로 설계
    public class MainSceneConnecting_UI_Model : UI_Model<MainSceneConnecting_UI_Presenter>
    {
        private List<RoomData> roomDataList = new();
        private List<GameObject> roomButtonUIList = new();
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
        /// 룸 데이터 초기화
        /// </summary>
        internal void ClearRoomData() {
            roomDataList.Clear();
        }

        /// <summary>
        /// room UI Button 추가
        /// </summary>
        /// <param name="room"></param>
        internal void AddRoomUI(GameObject room) {
            roomButtonUIList.Add(room);
        }

        /// <summary>
        /// room UI Button List 목록
        /// </summary>
        /// <returns></returns>
        internal ReadOnlyCollection<GameObject> GetRoomUI_RO() {
            return roomButtonUIList.AsReadOnly();
        }
        /// <summary>
        /// room UI Button List 초기화
        /// </summary>
        internal void ClearRoomUI() {  
            roomButtonUIList.Clear(); 
        }
    }

    public struct RoomData {
        public string roomHash;
        public string roomName;
        public string userName;
        public bool isPublic;
    }
}
