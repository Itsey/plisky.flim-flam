//using Plisky.Plumbing.Legacy;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Plisky.Plumbing;

namespace Plisky.FlimFlam {

    internal class TCPRecieverThread {
        internal static bool continueRunning;   // Determines when to end the threads.

        internal static void InterceptTCPMessage() {
            //Bilge.E("TCPReciever Thread / thread Loop");
            try {
                //Bilge.Log("Brinigng TCP Listener Online");

                IPAddress bindIP;

                if (MexCore.TheCore.Options.IPAddressToBind == "127.0.0.1") {
                    //Bilge.Warning("Listening on local port only");
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPStatusMessage, UserMessageType.WarningMessage, "The TCP listener is bound to the local machine only.");
                }

                if (MexCore.TheCore.Options.IPAddressToBind == "*") {
                    bindIP = IPAddress.Any;
                } else {
                    try {
                        bindIP = Dns.GetHostEntry(MexCore.TheCore.Options.IPAddressToBind).AddressList[0];
                    } catch (SocketException) {
                        //Bilge.Dump(sox, "Socket Exception resolving the hostname entered, not starting TCP listener");
                        MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPListenerInvalidHostError, UserMessageType.ErrorMessage, "IP:" + MexCore.TheCore.Options.IPAddressToBind);
                        MexCore.TheCore.MessageManager.DeactivateTCPGatherer();
                        return;
                    }
                }

                int port = MexCore.TheCore.Options.PortAddressToBind;
                //Bilge.FurtherInfo("Binding to " + MexCore.TheCore.Options.IPAddressToBind + " on port " + port.ToString());

                continueRunning = true;

                var server = new TcpListener(bindIP, port);
                try {
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPStatusMessage, UserMessageType.InformationMessage, "The TCP listener is starting.");
                    server.Start();
                } catch (SocketException sex) {
                    //Bilge.Dump(sex, "Socket exception when starting to listen, something is already listening on this port");
                    continueRunning = false;
                    // TODO : Notify the user that the listener failed to start......
                    // TODO : When notification is working!
                    //Bilge.Warning("Socket exception, this instance of Mex is not listening for incomming connections");
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPStatusMessage, UserMessageType.WarningMessage, "The TCP listener was terminated due to an error." + sex.Message);
                }

                try {
                    // thread loop

                    while (continueRunning) {
                        if (server.Pending()) {
                            var client = server.AcceptTcpClient();
                            _ = ThreadPool.QueueUserWorkItem(new WaitCallback(ClientConnectionHandler), client);
                        } else {
                            Thread.Sleep(100); // sleep, check whether to continue running and then try again.
                        }
                    }
                } catch (SocketException ex) {
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPStatusMessage, UserMessageType.ErrorMessage, "The TCP listener has errored and is closing down." + ex.Message);
                } finally {
                    server.Stop();
                }
            } finally {
                //Bilge.X();
            }
        }

        /// <summary>
        /// Code taken from DS and modified by me.  This callback is made whenever a new TCP client is recieved to handle the input and
        /// place the data into Mex.
        /// </summary>
        /// <param name="tcpClient">The client recieved when the connection was made</param>
        private static void ClientConnectionHandler(object tcpClient) {
            var client = tcpClient as TcpClient;

            //Bilge.Assert(client != null, " Client was invalid when passed to client connection handler.  This code cant handle that");

            // Buffer for reading data (set as default 8192)
            byte[] bytes = new byte[client.ReceiveBufferSize];

            // Get a stream object for reading and writing
            var stream = client.GetStream();

            var incomingMessage = new StringBuilder();
            int bytesRead;

            // Loop to receive all the data sent by the client.

            // This was using while stream.DataAvailable but this did not seem to work predictably enough therefore gone to using the return from read.
            do {
                try {
                    // Read the data from the underlying socket connection.  If this closes then this read will blow and we simply return
                    // as we are unable to get any more data out.  In this case any partial messages recorded are lost.
                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                    if (bytesRead == 0) { continue; }
                } catch (IOException) {
                    //Bilge.Dump(iex, "Network connection closed, unable to retrieve more data");
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPStatusMessage, UserMessageType.InformationMessage, "The connection was forcibly closed by the other end of the TCP Stream. Connection Aborted");
                    return;
                } catch (ObjectDisposedException) {
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.TCPStatusMessage, UserMessageType.InformationMessage, "The connection was forcibly closed by the other end of the TCP Stream. Connection Aborted");
                    //Bilge.Dump(odx, "Network connection disposed, unable to retrieve more data");
                    return;
                }

                _ = incomingMessage.Append(System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRead));

                string temp = incomingMessage.ToString();
                int liMarker = temp.LastIndexOf(FlimFlamConstants.TCPEND_MARKERTAG);

                if (liMarker > -1) {
                    incomingMessage.Length = 0;
                    _ = incomingMessage.Append(temp[(liMarker + FlimFlamConstants.TCPEND_MARKERTAGLEN)..]);

                    string[] msgs = temp[..liMarker].Split(new string[1] { FlimFlamConstants.TCPEND_MARKERTAG }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in msgs) {
                        IncomingMessageManager.Current.AddIncomingMessage(InternalSource.TCPReciever, s, -1);
                    }
                }
            } while (bytesRead != 0);

            // Shutdown and end connection
            stream.Close();
            client.Close();
        }
    }
}