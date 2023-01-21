﻿using System;
using Grpc.Core;
using MagicOnion;
using MagicOnion.Server;
using UnityEngine;

namespace Eridu.Rpg {
    // Implements RPC service in the server project.
    public class LivelinessService : ServiceBase<ILivelinessService>, ILivelinessService {
        public async UnaryResult<string> Status() {
            return "ok";
        }
    }
}