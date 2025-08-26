using System;
using System.Collections.Generic;
using System.Text;

public interface IControlPlan_Child
{
    void usp_ControlPlan_ChildInsert(Class_ControlPlan_Child objName);

    void usp_ControlPlan_ChildUpdate(Class_ControlPlan_Child objName);

    void usp_ControlPlan_ChildDelete(Class_ControlPlan_Child objName);

    IList<Class_ControlPlan_Child> usp_ControlPlan_ChildSelect();
}
