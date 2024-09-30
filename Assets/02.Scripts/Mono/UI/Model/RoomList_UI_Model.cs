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
		/// ���߰�
		/// </summary>
		/// <param name="roomData"></param>
		internal void AddRoomData(RoomData roomData) {
			_roomDataList.Add(roomData);
		}
		/// <summary>
		/// �� ������ �ʱ�ȭ
		/// </summary>
		internal void ClearRoomDataList() {
			_roomDataList.Clear();
		}
		/// <summary>
		/// �� ��� 
		/// </summary>
		/// <returns> readonly List<RoomData> </returns>
		internal ReadOnlyCollection<RoomData> GetRoomDataList_RO() {
			return _roomDataList.AsReadOnly();
		}


    }
}
