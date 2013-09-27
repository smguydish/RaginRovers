using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace RaginRovers
{
    class Networking
    {
        static NetPeerConfiguration config;
        NetServer server; 

        NetConnection recipient;
        NetPeer netpeer; 

        public Networking()
        {
            config = new NetPeerConfiguration("test_console");
            config.Port = 14242;
            server = new NetServer(config);
            //recipient = new NetConnection();
            netpeer = new NetPeer(config);
        }

        public void Receive()
        {
            NetIncomingMessage msg;
            string Stuff;
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    default:
                        //Console.WriteLine("Unhandled type: " + msg.MessageType);
                        //Transfer Data from message into string
                        Stuff = msg.ReadString();
                        break;
                }
                server.Recycle(msg); //what this do
            }
        }
        public void Send(string serializedData)
        {
            NetOutgoingMessage sendMsg = server.CreateMessage();

            sendMsg.Write(serializedData);

            server.SendMessage(sendMsg, recipient, NetDeliveryMethod.ReliableUnordered);
        }
    }
}
