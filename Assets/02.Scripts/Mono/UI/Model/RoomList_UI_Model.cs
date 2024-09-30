//MVP Generater
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Mono.UI
{
	public class RoomList_UI_Model : UI_Model<RoomList_UI_Presenter>, IModel {
		// internal
		private readonly List<RoomData> _roomDataList = new();

		/// <summary>
		/// 방추가
		/// </summary>
		/// <param name="roomData"></param>
		internal void AddRoomData(RoomData roomData) {
			_roomDataList.Add(roomData);
		}
		/// <summary>
		/// 방 데이터 초기화
		/// </summary>
		internal void ClearRoomDataList() {
			_roomDataList.Clear();
		}
		/// <summary>
		/// 방 목록 
		/// </summary>
		/// <returns> readonly List<RoomData> </returns>
		internal ReadOnlyCollection<RoomData> GetRoomDataList_RO() {
			return _roomDataList.AsReadOnly();
		}


    }
}
