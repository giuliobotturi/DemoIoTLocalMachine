#region StandardUsing
using System;
using QPlatform.CoreBase;
using QPlatform.HMIProject;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using QPlatform.NetLogic;
using QPlatform.Core;
using QPlatform.Datalogger;
using QPlatform.Store;
using QPlatform.SQLiteStore;
using QPlatform.Recipe;
using QPlatform.CommunicationDriver;
using QPlatform.Retentivity;
#endregion

public class LoginButtonLogic : QPlatform.NetLogic.BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void PerformLogin(string username, string password, out bool loginResult)
    {
        var usersAlias = LogicObject.GetAlias("Users");
        if (usersAlias == null || usersAlias.NodeId == NodeId.Empty)
        {
            Log.Error("LoginForm", "Missing Users alias");
            loginResult = false;
            return;
        }

        var user = usersAlias.Get<User>(username);
        if (user == null)
        {
            Log.Error("LoginForm", "Could not find user " + username);
            loginResult = false;
            return;
        }

        try
        {
            user.PasswordVariable.RemoteRead();
            loginResult = Session.ChangeUser(username, password);
        }
        catch (Exception e)
        {
            Log.Error("LoginForm", e.Message);
            loginResult = false;
        }
    }

    [ExportMethod]
    public void ClosePopupIfSuccess(bool loginSuccess)
    {
        if (!loginSuccess)
            return;

        var loginPopup = Owner.Owner.Owner as QPlatform.UI.Popup;
        if (loginPopup == null)
            return;

        loginPopup.Close();
    }

}
