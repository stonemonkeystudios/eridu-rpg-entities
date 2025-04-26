using Eridu.Common;
using MagicOnion;
using MessagePack;
using System.Threading.Tasks;
using UnityEngine;
using HQDotNet;

namespace Eridu.Rpg {
    public interface IEriduRpgEntitiesHubReceiver : IDispatchListener
    {
        // return type should be `void` or `Task`, parameters are free.
        void OnJoin(EriduPlayer player);
        void OnLeave(EriduPlayer player);
        void OnUpdateEntityHP(int rpgEntityId, bool isPlayer, int damageTaken, int baseValue, int currentValue);
        void OnEntityDie(int rpgEntityId, bool isPlayer);
        void OnResurrectPlayer(int rpgEntityId);
        void OnRequestResurrectPlayer(int rpgEntityId);

        void OnSpawnCollectableItemForPlayer(string itemGuid, int playerId, int itemId, Vector3 worldPosition);
        void OnDespawnCollectableItem(int playerId, int itemId);
        void OnPrototypeMessageReceived(string messageType, string messageData);
    }

    public interface IEriduRpgEntitiesHub : IStreamingHub<IEriduRpgEntitiesHub, IEriduRpgEntitiesHubReceiver> {
        // return type should be `Task` or `Task<T>`, parameters are free.
        Task<EriduPlayer[]> JoinAsync(string roomName, EriduPlayer player);
        Task UpdateEntityHP(int rpgEntityId, bool isPlayer, int damageTaken, int baseValue, int currentValue);
        Task EntityDie(int rpgEntityId, bool isPlayer);
        Task RequestResurrectPlayer(int rpgEntityId);
        Task ResurrectPlayer(int rpgEntityId);
        Task SpawnCollectableItemForPlayer(string itemGuid, int playerId, int itemId, Vector3 worldPosition);
        Task DespawnCollectableItemForPlayer(int itemId, int playerId);
        Task PrototypeMessage(string messageType, string messageData);

        Task LeaveAsync();
    }
}
