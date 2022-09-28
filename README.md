# Client-Server-App


## Table of Contents
* [General info](#general-info)
* [Tools](#tools)
* [Technologies](#technologies)
* [Description](#description)


## General Info

## Tools
* Visual Studio 2019
* HHD Virtual Serial Port Tools (Free Version)


## Technologies
* .Net Framework

## Useful Resources
* TcpChannel Class with example of use .Net Remoting
    * https://learn.microsoft.com/en-us/dotnet/api/system.runtime.remoting.channels.tcp.tcpchannel?view=netframework-4.8&viewFallbackFrom=netframework-4.8%5C

## Description

Project contains communication between server and clients which can use different communicators and services. 
Communicators use common interface with basic functions such as Start and Stop running conversation. 
Communication between client and server starts with choose right communicator e.g TCP on the client side.
After that client ask for service to execute and send it to server and waits for answer. 
Server needs to recognize service command and send data back to right Client.

* Communicators used in project:
- [x] TCP
- [x] UDP
- [x] RS232
- [x] .Net Remoting
- [x] Files communication

* Services used in project:
- [x] Ping
- [x] Chat
- [x] Configuration  
- [x] Help
- [x] Files


## TO DO

- [x] Correct .Net Remoting - Use appernt connection between server and client - .Net Remoting listener fullfill for set of connection 
- [] Many UDP clients sending data at the same time 
- [] new option for ping - ping [bytes-to-server] [bytes-from-server] [how-many-time] 
- [] new communicators gRPC,Bluetooth etc. 
- [] Decrease usage of processor