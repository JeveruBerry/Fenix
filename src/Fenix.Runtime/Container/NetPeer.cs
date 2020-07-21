﻿
using DotNetty.Buffers; 
using DotNetty.KCP;
using DotNetty.Transport.Channels;
using Fenix.Common.Rpc;
using Fenix.Common.Utils;
using MessagePack;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fenix
{
    public class NetPeer
    { 
        public uint ConnId { get; set; }

        protected Ukcp kcpChannel { get; set; }
        
        protected IChannel tcpChannel { get; set; }

        protected TcpContainerClient tcpClient { get; set; }

        protected KcpContainerClient kcpClient { get; set; }

        public event Action<NetPeer, IByteBuffer> OnReceive;

        public event Action<NetPeer> OnClose;

        public event Action<NetPeer, IByteBuffer> OnSend;

        public enum NetworkType
        {
            TCP = 0x0,
            KCP = 0x1
        }

        public NetworkType networkType => tcpChannel != null ? NetworkType.TCP : NetworkType.KCP;

        public bool IsActive
        {
            get
            {
                if (this.tcpChannel != null)
                    return tcpChannel.Active;
                if (this.kcpChannel != null)
                    return kcpChannel.isActive();
                if (this.kcpClient != null)
                    return true;
                if (this.tcpClient != null)
                    return this.tcpClient.IsActive;
                return false;
            }
        }

        // public static NetPeer Create(IChannel channel)
        // {
        //     var obj = new NetPeer(); 
        //     obj.ConnId = 0;
        //     obj.tcpChannel = channel;
        //     return obj;
        // }
        
        protected NetPeer()
        {

        }

        //protected NetPeer(uint connId, bool isTcp)
        //{
        //    if (isTcp)
        //    {
        //        CreateTcpClient(connId);
        //    }
        //    else
        //    { 
        //        var addr = Global.IdManager.GetContainerAddr(connId);
        //        var parts = addr.Split(':');
        //        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));

        //        kcpClient = KcpContainerClient.Create(ep);
        //        kcpClient.OnReceive += KcpClient_OnReceive; 
        //    }
        //}

        protected bool InitTcpClient(uint connId)
        {
            var addr = Global.IdManager.GetContainerAddr(connId);
            if(addr == null)
            {
                //container 不存在
                return false;
            }
            var parts = addr.Split(':');
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));

            tcpClient = TcpContainerClient.Create(ep);

            if (tcpClient == null)
                return false;

            tcpClient.Receive += TcpClient_OnReceive;
            return true;
        }

        protected bool InitKcpClient(uint connId)
        {
            var addr = Global.IdManager.GetContainerAddr(connId);
            if (addr == null)
            {
                //container 不存在
                return false;
            }

            var parts = addr.Split(':');
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));

            kcpClient = KcpContainerClient.Create(ep);
            kcpClient.OnReceive += KcpClient_OnReceive;
            return true;
        }

        public static NetPeer Create(uint connId, Ukcp kcpCh)
        {
            var obj = new NetPeer();
            obj.ConnId = connId;
            obj.kcpChannel = kcpCh;
            return obj;
        }

        public static NetPeer Create(uint connId, IChannel tcpCh)
        {
            var obj = new NetPeer();
            obj.ConnId = connId;
            obj.tcpChannel = tcpCh;
            return obj;
        }

        public static NetPeer Create(uint connId, bool isTcp)
        {
            var obj = new NetPeer();
            obj.ConnId = connId;
            if (isTcp)
            {
                if (!obj.InitTcpClient(connId))
                    return null;
            }
            else
            {
                if (!obj.InitKcpClient(connId))
                    return null;
            }
            return obj;
        }

        private void TcpClient_OnReceive(IChannel channel, IByteBuffer buffer)
        {
            OnReceive?.Invoke(this, buffer);
        }

        private void KcpClient_OnReceive(Ukcp ukcp, IByteBuffer buffer)
        {
            OnReceive?.Invoke(this, buffer);
        }

        ~NetPeer()
        {
            if (tcpClient != null)
                tcpClient.Receive -= TcpClient_OnReceive;
        }

        public void Send(byte[] bytes)
        {
            kcpChannel?.send(bytes);
            tcpChannel?.WriteAndFlushAsync(Unpooled.WrappedBuffer(bytes));
            kcpClient?.Send(bytes);
            tcpClient?.Send(bytes);
        }

        public async Task SendAsync(byte[] bytes)
        {
            await Task.Run(() => {
                this.Send(bytes);
            });
        }

        public void Send(Packet packet)
        { 
            this.Send(packet.Pack());
        }
    }
}
