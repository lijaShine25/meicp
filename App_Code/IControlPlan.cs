using System;
using System.Collections.Generic;
using System.Text;

public interface IControlPlan
{
    void usp_ControlPlanInsert(Class_ControlPlan objName);

    void usp_ControlPlanUpdate(Class_ControlPlan objName);

    void usp_ControlPlanDelete(Class_ControlPlan objName);

    IList<Class_ControlPlan> usp_ControlPlanSelect();

    //void usp_InitiateControlPlanRevision(int cp_slno, string rev_reason);
    void usp_InitiateControlPlanRevision(int cp_slno, string rev_reason, string urev_no, string urev_date,int dcr);

    void usp_ControlPlanSmartCopy(int part_slno, int operation_slno, int newpart_slno, string newpartno);
}
