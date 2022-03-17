#region StandardUsing
using System;
using QPlatform.Core;
using QPlatform.CoreBase;
using QPlatform.HMIProject;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using QPlatform.NetLogic;
using QPlatform.UI;
using QPlatform.Recipe;
using QPlatform.OPCUAServer;
using QPlatform.Store;
using System.Timers;
using QPlatform.Retentivity;
#endregion

public class RecipesEditorOutputMessageLogic : QPlatform.NetLogic.BaseNetLogic
{
    public override void Start()
    {
        messageVariable = Owner.Children.Get<IUAVariable>("Message");
        if (messageVariable == null)
            throw new ArgumentNullException("Unable to find variable Message in OutputMessage label");
    }

	public override void Stop()
    {
        lock (lockObject)
        {
            task?.Dispose();
        }
	}

	[ExportMethod]
	public void SetOutputMessage(string message)
	{
        lock (lockObject)
        {
            task?.Dispose();

            messageVariable.Value = message;
            task = new DelayedTask(() => { messageVariable.Value = ""; }, 5000, LogicObject);
            task.Start();
        }
	}
    
	[ExportMethod]
	public void SetOutputLocalizedMessage(LocalizedText message)
	{
        SetOutputMessage(InformationModel.LookupTranslation(message).Text);
	}

	DelayedTask task;
	IUAVariable messageVariable;
    object lockObject = new object();
}
