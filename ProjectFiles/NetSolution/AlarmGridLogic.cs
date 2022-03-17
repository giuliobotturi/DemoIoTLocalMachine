#region StandardUsing
using System;
using QPlatform.Core;
using QPlatform.CoreBase;
using QPlatform.HMIProject;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using QPlatform.NetLogic;
using QPlatform.OPCUAServer;
using QPlatform.UI;
using QPlatform.Alarm;
using QPlatform.EventLogger;
using QPlatform.System;
using QPlatform.Datalogger;
using QPlatform.Store;
using QPlatform.SQLiteStore;
using QPlatform.Recipe;
using QPlatform.CommunicationDriver;
using QPlatform.Retentivity;
#endregion

public class AlarmGridLogic : QPlatform.NetLogic.BaseNetLogic
{
    public override void Start()
    {
        alarmsDatagrid = Owner.Children.Get<DataGrid>("AlarmsDataGrid");
        alarmsDatagridModel = alarmsDatagrid.Children.GetVariable("Model");

        affinityId = alarmsDatagrid.Context.AssignAffinityId();
        RegisterObserverOnSessionLocaleIdChanged(alarmsDatagrid.Context);
    }

    public override void Stop()
    {
        if (localeIdsRegistration != null)
        {
            localeIdsRegistration.Dispose();
            localeIdsRegistration = null;
        }

        if (localeIdChangedObserver != null)
            localeIdChangedObserver = null;

    }

    public void RegisterObserverOnSessionLocaleIdChanged(IContext context)
    {
        var currentSessionLocaleIds = context.Sessions.CurrentSessionInfo.SessionObject.Children["ActualLocaleIds"];

        localeIdChangedObserver = new CallbackVariableChangeObserver((IUAVariable variable, UAValue newValue, UAValue oldValue, uint[] _, ulong __) =>
        {
			//reset datagrid model variable to trigger locale changed event
			var dynamicLink = alarmsDatagridModel.GetVariable("DynamicLink");
			if (dynamicLink == null)
				return;

			string dynamicLinkValue = dynamicLink.Value;
			dynamicLink.Value = string.Empty;
			dynamicLink.Value = dynamicLinkValue;
        });

        localeIdsRegistration = currentSessionLocaleIds.RegisterEventObserver(
            localeIdChangedObserver, EventType.VariableValueChanged, affinityId);
    }

    IEventRegistration localeIdsRegistration;
    IEventObserver localeIdChangedObserver;
    uint affinityId;
    DataGrid alarmsDatagrid;
    IUAVariable alarmsDatagridModel;
}
