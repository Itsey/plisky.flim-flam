// See https://aka.ms/new-console-template for more information
using Plisky.Diagnostics;
using Plisky.Diagnostics.Listeners;
using Plisky.FlimFlam;

Bilge b = new Bilge("FF-ODS");
Bilge.AddHandler(new TCPHandler(new TCPHandlerOptions("127.0.0.1",9060)));

new ODSDataGathererThread().InterceptODS();

