#region Using directives
using System;
using QPlatform.Core;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using QPlatform.CoreBase;
using QPlatform.Store;
using QPlatform.NetLogic;
using QPlatform.UI;
using QPlatform.HMIProject;
using QPlatform.Datalogger;
using QPlatform.Alarm;
using QPlatform.CommunicationDriver;
using QPlatform.Recipe;
using QPlatform.SQLiteStore;
using QPlatform.OPCUAServer;
using QPlatform.NativeUI;
using QPlatform.EventLogger;
using QPlatform.Retentivity;
#endregion

public class IotConnector : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
