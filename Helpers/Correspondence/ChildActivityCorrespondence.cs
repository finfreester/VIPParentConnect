using System;
using CodeBridgeSoftware.Infrastructure.Email;
using CodeBridgeSoftware.Infrastructure.Correspondence;
using CodeBridgeSoftware.Infrastructure.Correspondence.Entities;
using CodeBridgeSoftware.CoreApps;
using System.Collections.Generic;
using System.Collections.Specialized;
using CodeBridgeSoftware.WebApps;
using VIPDayCareCenters.Models;
using VIPDayCareCenters.Repository;
using SP.Utilities.Report;
using System.Text;
using System.Linq;

public class ChildActivityCorrespondence : CorrespondenceBase
{

    public ChildActivityCorrespondence(string templateType,
                                       string runtimeEnvironment,
                                       string correspondenceFilePath,
                                       IEmailer emailMgr = null)
        : base(templateType, cCorrespondence.enumTemplateDeliveryMedium.Email, cCorrespondence.enumTemplateContentType.HTML, runtimeEnvironment, correspondenceFilePath, emailMgr)
    {
    }

    protected  override void onCorrespondenceTemplatePreMerge(ref bool bCancelMerge)
    {
        
        base.onCorrespondenceTemplatePreMerge(ref bCancelMerge);

        DailyGroupActivitiesDTO dailyGroupActivitiesDTO = (DailyGroupActivitiesDTO)(this.Parameters[0]);
        int childId = int.Parse(this.Parameters[1].ToString());
        
        var child = (from gc in dailyGroupActivitiesDTO.GroupChildren
                     where gc.Id == childId
                     select gc).FirstOrDefault();

        this.DistributionList.Add(new CorrespondenceEmailDistribution(child.Email));

    }

    protected override void onCorrespondenceTemplateMerge(NameValueCollection colMergeFields, string szCorrespondenceTemplate, ref bool bCancelMerge)
    {

        DailyGroupActivitiesDTO dailyGroupActivitiesDTO = (DailyGroupActivitiesDTO)(this.Parameters[0]);
        int childId = int.Parse(this.Parameters[1].ToString());
        string centerName = this.Parameters[2].ToString();
        string groupName = this.Parameters[3].ToString().ToUpper();
        string websiteAddress = this.Parameters[4].ToString();

        StringBuilder sbReport = new StringBuilder();

        var child = (from gc in dailyGroupActivitiesDTO.GroupChildren
                     where gc.Id == childId
                     select gc).FirstOrDefault();

        colMergeFields.Set("[WEBSITE]", websiteAddress);
        colMergeFields.Set("[CHILD_FIRSTNAME]", child.FirstName + " " + child.LastName.Substring(0,1) + ".");
        colMergeFields.Set("[PARENT_FIRSTNAME]", child.ParentFirstName);
        colMergeFields.Set("[CENTER]", centerName);
        colMergeFields.Set("[GROUP_NAME]", groupName);

        //Added to track and set what child email was last processed in order to check if same email as before and to AOL
        //We want to inject a wait time before sending out the email in those cases.
        colMergeFields.Set("[LAST_EMAIL]", child.Email);

        colMergeFields.Set("[TODAY]", string.Format("{0:dddd, MM/dd/yyyy}", dailyGroupActivitiesDTO.DailyGroup.ClassDate));

        sbReport.Length = 0;

        sbReport.AppendLine("<table id='tblGroupActivity' border='1' style='border-spacing: 8px 2px; padding: 6px;'>");

        sbReport.AppendLine("<tr class='HeaderGrid'>");

        StandardReport.BuildTableCell(sbReport, "Child Name", "white-space: nowrap; text-align: left; text-decoration: underline; font-weight: bold;");

        foreach (var activityName in dailyGroupActivitiesDTO.ChildActivityValues.Select(cav => cav.ActivityName).Distinct())
        {
            StandardReport.BuildTableCell(sbReport, activityName, "text-align: left; text-decoration: underline; font-weight: bold;");
        }

        sbReport.AppendLine("</tr>");

        sbReport.AppendLine("<tr class='Grid'>");

        StandardReport.BuildTableCell(sbReport, colMergeFields.Get("[CHILD_FIRSTNAME]"), "text-align: left;");

        foreach (var activityName in dailyGroupActivitiesDTO.ChildDailyGroupActivities.Select(cav => cav.ActivityName).Distinct())
        {
            var selectedItem = (from ca in dailyGroupActivitiesDTO.ChildDailyGroupActivities
                                where ca.ActivityName == activityName && ca.ChildId == childId
                                select ca.ActivityValue).FirstOrDefault();

            StandardReport.BuildTableCell(sbReport, selectedItem, "text-align: left;");
        }

        sbReport.AppendLine("</tr>");

        sbReport.AppendLine("</table>");

        colMergeFields.Set("[REPORT]", sbReport.ToString());

        base.onCorrespondenceTemplateMerge(colMergeFields, szCorrespondenceTemplate, ref bCancelMerge);

    }

}