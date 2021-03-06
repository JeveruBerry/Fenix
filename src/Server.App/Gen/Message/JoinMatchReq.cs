﻿//AUTOGEN, do not modify it!

using Fenix.Common.Utils;
using Fenix.Common;
using Fenix.Common.Attributes;
using Fenix.Common.Rpc;
using MessagePack; 
using System.ComponentModel;
using Shared;
using Shared.Protocol;
using Shared.DataModel;
using System; 

namespace Shared.Message
{
    [MessageType(ProtocolCode.JOIN_MATCH_REQ)]
    [MessagePackObject]
    public class JoinMatchReq : IMessageWithCallback
    {
        [Key(0)]
        public String uid { get; set; }

        [Key(1)]
        public Int32 match_type { get; set; }

        [Key(2)]

        public Callback callback
        {
            get => _callback as Callback;
            set => _callback = value;
        } 

        [MessagePackObject]
        public class Callback : IMessage
        {
            [Key(0)]
            [DefaultValue(ErrCode.ERROR)]
            public ErrCode code { get; set; } = ErrCode.ERROR;

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
            return MessagePackSerializer.Serialize<JoinMatchReq>(this, RpcUtil.lz4Options);
        }
        public new static JoinMatchReq Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<JoinMatchReq>(data, RpcUtil.lz4Options);
        }
    }
}

