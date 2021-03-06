﻿//AUTOGEN, do not modify it!

using Fenix.Common.Utils;
using Fenix.Common;
using Fenix.Common.Attributes;
using Fenix.Common.Rpc;
using MessagePack; 
using System.ComponentModel;
using System; 

namespace Fenix.Common.Message
{
    [MessageType(OpCode.BIND_CLIENT_ACTOR_REQ)]
    [MessagePackObject]
    public class BindClientActorReq : IMessageWithCallback
    {
        [Key(0)]
        public String actorName { get; set; }

        [Key(1)]

        public Callback callback
        {
            get => _callback as Callback;
            set => _callback = value;
        } 

        [MessagePackObject]
        public class Callback : IMessage
        {
            [Key(0)]
            [DefaultValue(DefaultErrCode.ERROR)]
            public DefaultErrCode code { get; set; } = DefaultErrCode.ERROR;

            public override byte[] Pack()
            {
                return MessagePackSerializer.Serialize<Callback>(this, RpcUtil.lz4Options);
            }
            public new static Callback Deserialize(byte[] data)
            {
                return MessagePackSerializer.Deserialize<Callback>(data, RpcUtil.lz4Options);
            }
        }

        public override byte[] Pack()
        {
            return MessagePackSerializer.Serialize<BindClientActorReq>(this, RpcUtil.lz4Options);
        }
        public new static BindClientActorReq Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<BindClientActorReq>(data, RpcUtil.lz4Options);
        }
    }
}

