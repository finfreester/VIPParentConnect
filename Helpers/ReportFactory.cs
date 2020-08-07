using System;
using System.Collections.Specialized;
using CodeBridgeSoftware.Infrastructure.Email;
using CodeBridgeSoftware.Infrastructure.Correspondence;
using System.Configuration;
using CodeBridgeSoftware.WebApps;

public class ReportFactory
{

    private static readonly bool TEST_MODE = (ConfigurationManager.AppSettings["TestMode"] == "on");
    private static readonly string RUNTIME_ENVIRONMENT = ConfigurationManager.AppSettings["PlatformName"];
    private static readonly string TEST_MODE_EMAIL = ConfigurationManager.AppSettings["TestModeEmail"];
    private static readonly string INTERNAL_COPY_EMAIL = ConfigurationManager.AppSettings["notificationemailaddress"];

    private IEmailer _emailMgr;
    private CorrespondenceBase _correspondence;

    public static CorrespondenceBase getReportCorrespondence(string templateType,
                                                             string correspondenceFilePath,
                                                             IEmailer emailMgr)
    {

        templateType = templateType.ToLower().Trim();

        switch (templateType)
        {
            case "childactivity":
                return new ChildActivityCorrespondence(templateType, RUNTIME_ENVIRONMENT, correspondenceFilePath, emailMgr);

            default:
                throw new ArgumentNullException("templateType provided could not be found.");

        }

    }

    public void generateReportCorrespondence(string templateType,
                                             string correspondenceFilePath,
                                             IEmailer emailMgr,
                                             params object[] paramValues)
    {

        _emailMgr = emailMgr;

        _correspondence = getReportCorrespondence(templateType, correspondenceFilePath, emailMgr);

        _correspondence.CorrespondenceDelivered += _correspondence_CorrespondenceDelivered;
        _correspondence.StatusUpdate += _correspondence_StatusUpdate;

        _correspondence.Generate(TEST_MODE, TEST_MODE_EMAIL, INTERNAL_COPY_EMAIL, templateType, paramValues);

    }

    protected void _correspondence_CorrespondenceDelivered(object sender, CorrespondenceBase.CorrespondenceDeliveredArgs e)
    {
        
        if (e.EmailStatusId == 3) //Email Failed
        {
            //try to resend
        
            //_emailMgr.Send(e.Subject, )
        }

    }

    protected void _correspondence_StatusUpdate(object sender, string message)
    {
    
    }

}

