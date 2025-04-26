using System;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Hubs;
using UnityEngine;
using System.Linq;
using Eridu.Common;

namespace Eridu.Rpg {
    // Implements RPC service in the server project.
    // The implementation class must inherit `ServiceBase<IMyFirstService>` and `IMyFirstService`
    [GroupConfiguration(typeof(ConcurrentDictionaryGroupRepositoryFactory))]
    public class EriduRpgEntitiesHub : StreamingHubBase<IEriduRpgEntitiesHub, IEriduRpgEntitiesHubReceiver>, IEriduRpgEntitiesHub {
        //testing regenerstion of model files in ci/cs
        bool test = false;
        IGroup room;
        EriduPlayer self;
        Matrix4x4 lastKnownPosition;
        IInMemoryStorage<EriduPlayer> _clientStorage;
        //IInMemoryStorage<EriduCharacter> _characterStorage;

        private bool leftRoom = false;

        #region IEriduRpgEntitiesHub Methods

        public async Task<EriduPlayer[]> JoinAsync(string roomName, EriduPlayer player) {
            //_clientIDTransformIndex = new Dictionary<int, int>();
            self = player;

            //Group can bundle many connections and it has inmemory-storage so add any type per group
            (room, _clientStorage) = await Group.AddAsync(roomName, self);
            ClientStorage.Instance.AddClient(self.clientId, this.Context.ContextId);

            BroadcastExceptSelf(room).OnJoin(self);

            return _clientStorage.AllValues.ToArray();
        }

        public async Task LeaveAsync() {
            await LeaveRoom();
        }

        protected async override ValueTask OnDisconnected() {
            await LeaveRoom();
        }

        private async Task LeaveRoom() {
            if (!leftRoom) {
                if(room != null) {
                    ClientStorage.Instance.RemoveClient(self.clientId);
                    await room.RemoveAsync(this.Context);
                    Broadcast(room).OnLeave(self);
                    leftRoom = true;
                }
            }
        }

        protected override ValueTask OnConnecting() {
            return CompletedTask;
        }

        public IEriduRpgEntitiesHub FireAndForget() {
            return this;
        }

        public Task DisposeAsync() {
            return Task.CompletedTask;
        }

        public Task WaitForDisconnect() {
            return Task.CompletedTask;
        }

        #endregion

        public Task UpdateEntityHP(int rpgEntityId, bool isPlayer, int damageTaken, int baseValue, int currentValue) {
            if(room != null)
                Broadcast(room).OnUpdateEntityHP(rpgEntityId, isPlayer, damageTaken, baseValue, currentValue);
            return Task.CompletedTask;
        }

        public Task EntityDie(int rpgEntityId, bool isPlayer) {
            if(room != null)
                Broadcast(room).OnEntityDie(rpgEntityId, isPlayer);
            return Task.CompletedTask;
        }

        public Task RequestResurrectPlayer(int rpgEntityId) {
            if (room != null)
                Broadcast(room).OnRequestResurrectPlayer(rpgEntityId);
            return Task.CompletedTask;
        }

        public Task ResurrectPlayer(int rpgEntityId) {
            if (room != null)
                Broadcast(room).OnResurrectPlayer(rpgEntityId);
            return Task.CompletedTask;
        }

        public Task SpawnCollectableItemForPlayer(string itemGuid, int playerId, int itemId, Vector3 worldPosition) {
            var connectionId = ClientStorage.Instance.GetConnectionIdFromClientId(playerId);
            if (room != null && connectionId.HasValue) {
                BroadcastTo(room, connectionId.Value).OnSpawnCollectableItemForPlayer(itemGuid, playerId, itemId, worldPosition);
            }
            return Task.CompletedTask;
        }

        public Task DespawnCollectableItemForPlayer(int itemId, int playerId) {
            var connectionId = ClientStorage.Instance.GetConnectionIdFromClientId(playerId);
            if(room != null && connectionId.HasValue) {
                BroadcastTo(room, connectionId.Value).OnDespawnCollectableItem(playerId, itemId);
            }
            return Task.CompletedTask;
        }

        public Task PrototypeMessage(string messageType, string messageData) {
            if (room != null)
                Broadcast(room).OnPrototypeMessageReceived(messageType, messageData);
            return Task.CompletedTask;
        }
    }
}