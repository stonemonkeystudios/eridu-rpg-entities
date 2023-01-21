using Eridu.Common;
using MagicOnion;
using MessagePack;
using System.Threading.Tasks;
using UnityEngine;

namespace HQDotNet.Presence {
    public interface IEriduRpgEntitiesHubReceiver : IDispatchListener
    {
        // return type should be `void` or `Task`, parameters are free.
        void OnJoin(EriduPlayer player);
        void OnLeave(EriduPlayer player);
        void OnUpdateEntityHP(int rpgEntityId, bool isPlayer, int damageTaken, int baseValue, int currentValue);
        void OnEntityDie(int rpgEntityId, bool isPlayer);
        void OnResurrectPlayer(int rpgEntityId);
        void OnRequestResurrectPlayer(int rpgEntityId);
        void OnPrototypeMessageReceived(string messageType, string messageData);
    }

    public interface IEriduRpgEntitiesHub : IStreamingHub<IEriduRpgEntitiesHub, IEriduRpgEntitiesHubReceiver> {
        // return type should be `Task` or `Task<T>`, parameters are free.
        Task<EriduPlayer[]> JoinAsync(string roomName, EriduPlayer player);
        Task UpdateEntityHP(int rpgEntityId, bool isPlayer, int damageTaken, int baseValue, int currentValue);
        Task EntityDie(int rpgEntityId, bool isPlayer);
        Task RequestResurrectPlayer(int rpgEntityId);
        Task ResurrectPlayer(int rpgEntityId);
        Task PrototypeMessage(string messageType, string messageData);

        Task LeaveAsync();
    }
}
